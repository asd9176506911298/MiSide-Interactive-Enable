using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

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

        interactiveKey = Config.Bind<KeyCode>("", "InteractiveKey", KeyCode.PageUp, "");
        miniGameKey = Config.Bind<KeyCode>("", "MiniGameKey", KeyCode.PageDown, "");
        hardKey = Config.Bind<KeyCode>("", "HardKey", KeyCode.KeypadPlus, "");
        weakKey = Config.Bind<KeyCode>("", "WeakKey", KeyCode.KeypadMinus, "");
        showTVHintKey = Config.Bind<KeyCode>("", "ShowTVHintKey", KeyCode.Insert, "");

        harmony.PatchAll(typeof(Patch));

        // Add the MonoBehaviour that handles Update and Input
        IL2CPPChainloader.AddUnityComponent(typeof(KeyInputHandler));
    }

    private class KeyInputHandler : MonoBehaviour
    {
        private void Update()
        {
            try
            {
                // Check for specific key presses
                if (Input.GetKeyDown(interactiveKey.Value))
                {
                    Plugin.Instance.isInteractive = !Plugin.Instance.isInteractive;
                    Plugin.Log.LogInfo($"isInteractive: {Plugin.Instance.isInteractive}");
                }

                if (Input.GetKeyDown(miniGameKey.Value))
                {
                    Plugin.Instance.isMiniGame = !Plugin.Instance.isMiniGame;
                    foreach(var x in GameObject.FindObjectsOfType<MT_GameCnowballs>())
                    {
                        if (!x.play && !x.resultShow)
                            x.PlayTimeStart();
                    }
                    Plugin.Log.LogInfo($"isMiniGame: {Plugin.Instance.isMiniGame}");
                }

                if (Input.GetKeyDown(hardKey.Value))
                {
                    foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    {
                        if (x.enemyComplexity > 0)
                            x.enemyComplexity -= 1;
                        Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
                    }
                }

                if (Input.GetKeyDown(weakKey.Value))
                {
                    foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    {
                        x.enemyComplexity += 1;
                        Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
                    }
                }

                if (Input.GetKeyDown(showTVHintKey.Value))
                {
                    foreach (var x in GameObject.FindObjectsOfType<MinigamesTelevisionController>())
                    {
                        x.KeysMenuActiveTrue();
                        Plugin.Log.LogInfo($"KeysMenuActiveTrue");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"Error in Update: {ex.Message}");
            }
        }

        private void PerformAction()
        {
            // Example: Trigger some functionality in your plugin
            Plugin.Log.LogInfo("Action performed!");
        }
    }
}
