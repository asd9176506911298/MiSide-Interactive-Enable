using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

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
                if(Plugin.Instance.isInteractive)
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
                if (Plugin.Instance.isInteractive)
                {
                    __instance.active = true;
                    __instance.dontDestroyAfter = true;
                }
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
            if (Plugin.Instance.isInteractive)
            {
                __instance.destroyAfter = false;
                foreach (var x in __instance.games)
                {
                    x.playead = false;
                }
            }
        }

        [HarmonyPatch(typeof(MT_GameCnowballs), "Start")]
        [HarmonyPrefix]
        private static void MT_GameCnowballsStart(MT_GameCnowballs __instance)
        {
            if (Plugin.Instance.isMiniGame)
            {
                if (GameObject.Find("World/Quests/Quest 1/Game Aihastion/Dialogues/Pinguin/Dialogue Start/DMita 4") == null)
                {
                    __instance.PlayTimeStart();
                    Plugin.Log.LogInfo($"PlayTimeStart");
                }
            }
        }

        [HarmonyPatch(typeof(MT_GameCnowballs), "Update")]
        [HarmonyPrefix]
        private static void MT_GameCnowballsUpdate(MT_GameCnowballs __instance)
        {
            try
            {
                if (Plugin.Instance.isMiniGame)
                {
                    if (__instance.resultShow && __instance.resultTimeShow > 7.5f)
                    {
                        __instance.Continue();
                    }
                }
                else
                {
                    // Ensure the games array and its index are valid
                    if (__instance.gameController.main.games != null && __instance.gameController.main.games.Length > 1)
                    {
                        var game = __instance.gameController.main.games[1];
                        var win = game.countWin;
                        var lose = game.countLose;

                        if (win + lose >= 2)
                        {
                            __instance.countPlayed = 2;
                            __instance.Continue();
                        }
                    }
                    else
                    {
                        Plugin.Log.LogWarning("Games array is null or out of bounds.");
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Unexpected exception in MT_GameCnowballsUpdate: {ex.Message}");
            }
        }


        [HarmonyPatch(typeof(Location4Fight), "Update")]
        [HarmonyPrefix]
        private static void Location4FightUpdate(Location4Fight __instance)
        {
            if (Plugin.Instance.isMiniGame)
            {
                var game = __instance.gameController.main.games[0];
                var win = game.countWin;
                var lose = game.countLose;
                if ((win + lose >= 4) && (__instance.playerMain.health <= 0 || __instance.playerEnemy.health <= 0f || (!__instance.play && !__instance.playEnemy)))
                {
                    __instance.Continue();
                }
            }
            else
            {
                var game = __instance.gameController.main.games[0];
                var win = game.countWin;
                var lose = game.countLose;
                if ((win + lose >= 4))
                {
                    __instance.loseG = 4;
                    __instance.Continue();
                }
            }
        }
    }
}
