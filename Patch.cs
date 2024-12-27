using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveEnable
{
    internal class Patch
    {
        [HarmonyPatch(typeof(ObjectInteractive), "Update")]
        [HarmonyPrefix]
        private static void HookObjectInteractive(ObjectInteractive __instance)
        {
            try
            {
                __instance.active = true;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Error in HookObjectInteractive: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(ObjectInteractive_CaseInfo), "Update")]
        [HarmonyPrefix]
        private static void HookObjectInteractive_CaseInfo(ObjectInteractive_CaseInfo __instance)
        {
            try
            {
                __instance.dontDestroyAfter = true;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Error in HookObjectInteractive_CaseInfo: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(MinigamesTelevisionController), "Update")]
        [HarmonyPrefix]
        private static void HookMinigamesTelevisionController(MinigamesTelevisionController __instance)
        {
            __instance.destroyAfter = false;
            foreach (var x in __instance.games)
            {
                x.playead = false;
            }
        }

        [HarmonyPatch(typeof(MT_GameCnowballs), "Start")]
        [HarmonyPrefix]
        private static void HookStart(MT_GameCnowballs __instance)
        {
            Plugin.Log.LogInfo($"DMita444: {GameObject.Find("World/Quests/Quest 1/Game Aihastion/Dialogues/Pinguin/Dialogue Start/DMita 4") == null}");
            if(GameObject.Find("World/Quests/Quest 1/Game Aihastion/Dialogues/Pinguin/Dialogue Start/DMita 4") == null)
            {
                __instance.PlayTimeStart();
                Plugin.Log.LogInfo($"PlayTimeStart");
            }
        }

        [HarmonyPatch(typeof(MinigamesTelevisionGame), "GameLose")]
        [HarmonyPrefix]
        private static void HookGameLose(MinigamesTelevisionGame __instance, UnityAction call)
        {
            try
            {
                if(__instance != null)
                    if(__instance.objectGame.GetComponent<MT_GameCnowballs>() != null)
                    {
                        Plugin.Log.LogInfo($"Tsetest: {__instance.objectGame.GetComponent<MT_GameCnowballs>().countPlayed}");
                        if(__instance.objectGame.GetComponent<MT_GameCnowballs>().countPlayed >= 2)
                            __instance.ExitGame();
                        Plugin.Log.LogInfo("GameLose ExitGame.");
                    }
                        
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Error in GameLose: {ex.Message}");
            }
        }

        [HarmonyPatch(typeof(MinigamesTelevisionGame), "GameWin")]
        [HarmonyPrefix]
        private static void HookGameWin(MinigamesTelevisionGame __instance, UnityAction call)
        {
            try
            {
                // Safely calling ExitGame() and logging
                __instance.ExitGame();
                Plugin.Log.LogInfo("GameWin ExitGame.");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Error in GameWin: {ex.Message}");
            }
        }
    }
}
