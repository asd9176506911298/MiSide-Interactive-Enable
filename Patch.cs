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
        private static void MT_GameCnowballsStart(MT_GameCnowballs __instance)
        {
            Plugin.Log.LogInfo($"DMita444: {GameObject.Find("World/Quests/Quest 1/Game Aihastion/Dialogues/Pinguin/Dialogue Start/DMita 4") == null}");
            if(GameObject.Find("World/Quests/Quest 1/Game Aihastion/Dialogues/Pinguin/Dialogue Start/DMita 4") == null)
            {
                __instance.PlayTimeStart();
                Plugin.Log.LogInfo($"PlayTimeStart");
            }
        }

        [HarmonyPatch(typeof(MT_GameCnowballs), "Update")]
        [HarmonyPrefix]
        private static void MT_GameCnowballsUpdate(MT_GameCnowballs __instance)
        {
            if (__instance.resultShow && __instance.resultTimeShow > 10.0f)
                __instance.Continue();
        }
    }
}
