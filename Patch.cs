using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

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
                if (Plugin.Instance.isInteractive)
                {
                    if(__instance.name != "Interactive Dance")
                        __instance.active = true;
                }
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

        [HarmonyPatch(typeof(Location7_GameDance), "RestartDataSpheres")]
        [HarmonyPrefix]
        private static void Location7_GameDanceRestartDataSpheres(Location7_GameDance __instance)
        {
            if (__instance.indexFinishMita >= 3)
            {
                __instance.indexDialogue = 0;
                //__instance.eventFinishPlayer.Invoke();
                GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart/Canvas").GetComponent<ObjectInteractive_CaseInfo>().dontDestroyAfter = true;
                GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart").GetComponent<ObjectInteractive>().active = true;
            }

            //foreach(var x in GameObject.FindObjectsOfType<Shadow>())
            //{
            //    if(x.name == "Sphere(Clone)")
            //    {
            //        Plugin.Log.LogError($"GODDDDDD");
            //    }
            //}
        }

        [HarmonyPatch(typeof(Location7_GameDance), "EndGame")]
        [HarmonyPrefix]
        private static bool Location7_GameDanceEndGame(Location7_GameDance __instance)
        {
            //__instance.eventFinishPlayer.Invoke();
            //__instance.transform.parent.gameObject.SetActive(false);
            __instance.screenNextStart = false;
            //__instance.indexScreenGame = 0;

            return false;
        }
    }
}
