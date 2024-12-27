using BepInEx;
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

    public bool isInteractive = false;
    public bool isMiniGame = false;


    public override void Load()
    {
        Instance = this;
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.DEBUG = true;
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
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    Plugin.Instance.isInteractive = !Plugin.Instance.isInteractive;
                    Plugin.Log.LogInfo($"isInteractive: {Plugin.Instance.isInteractive}");
                }

                if (Input.GetKeyDown(KeyCode.F2))
                {
                    Plugin.Instance.isMiniGame = !Plugin.Instance.isMiniGame;
                    foreach(var x in GameObject.FindObjectsOfType<MT_GameCnowballs>())
                    {
                        if (!x.play && !x.resultShow)
                            x.PlayTimeStart();
                    }
                    Plugin.Log.LogInfo($"isMiniGame: {Plugin.Instance.isMiniGame}");
                }

                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    {
                        if (x.enemyComplexity > 0)
                            x.enemyComplexity -= 1;
                        Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
                    }
                }

                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    {
                        x.enemyComplexity += 1;
                        Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
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
