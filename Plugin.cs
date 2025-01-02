using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using UnityEngine;
using UniverseLib.Config;
using UniverseLib;
using System.IO;
using InteractiveEnable.UI;

namespace InteractiveEnable;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public static Plugin Instance { get; private set; }
    internal static new ManualLogSource Log;
    internal static Harmony harmony = new Harmony("InteractiveEnable");

    public static ConfigEntry<KeyCode> interactiveKey;
    public static ConfigEntry<KeyCode> miniGameKey;
    public static ConfigEntry<KeyCode> hardKey;
    public static ConfigEntry<KeyCode> weakKey;
    public static ConfigEntry<KeyCode> showTVHintKey;

    public bool isInteractive = false;
    public bool isMiniGame = false;

    public override void Load()
    {
        Instance = this;
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.DEBUG = true;

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        interactiveKey = Config.Bind<KeyCode>("", "InteractiveKey", KeyCode.PageUp, "");
        miniGameKey = Config.Bind<KeyCode>("", "MiniGameKey", KeyCode.PageDown, "");
        hardKey = Config.Bind<KeyCode>("", "HardKey", KeyCode.KeypadPlus, "");
        weakKey = Config.Bind<KeyCode>("", "WeakKey", KeyCode.KeypadMinus, "");
        showTVHintKey = Config.Bind<KeyCode>("", "ShowTVHintKey", KeyCode.Insert, "");

        harmony.PatchAll(typeof(Patch));

        // Add the MonoBehaviour that handles Update and Input
        IL2CPPChainloader.AddUnityComponent(typeof(KeyInputHandler));

        Universe.Init(
            0f,
            LateInitUI,
            LogHandler,
            new()
            {
                Disable_EventSystem_Override = false,
                Force_Unlock_Mouse = true,
                Unhollowed_Modules_Folder = Path.Combine(Paths.BepInExRootPath, "interop"),
            }
        );
    }


    void LateInitUI()
    {
        UIManager.Init();
    }

    void LogHandler(string message, LogType type)
    {
        // ...
    }

}
