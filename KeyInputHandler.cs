using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace InteractiveEnable
{
    public class KeyInputHandler : MonoBehaviour
    {
        public static KeyInputHandler Instance { get; private set; }

        public event Action onUpdate;

        void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            try
            {
                onUpdate?.Invoke();
                // Check for specific key presses
                if (Input.GetKeyDown(Plugin.interactiveKey.Value))
                {
                    Plugin.Instance.isInteractive = !Plugin.Instance.isInteractive;
                    Plugin.Log.LogInfo($"isInteractive: {Plugin.Instance.isInteractive}");
                }

                if (Input.GetKeyDown(Plugin.miniGameKey.Value))
                {
                    Plugin.Instance.isMiniGame = !Plugin.Instance.isMiniGame;
                    foreach (var x in GameObject.FindObjectsOfType<MT_GameCnowballs>())
                    {
                        if (!x.play && !x.resultShow)
                            x.PlayTimeStart();
                    }
                    Plugin.Log.LogInfo($"isMiniGame: {Plugin.Instance.isMiniGame}");
                }

                if (Input.GetKeyDown(Plugin.hardKey.Value))
                {
                    //foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    //{
                    //    if (x.enemyComplexity > 0)
                    //        x.enemyComplexity -= 1;
                    //    Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
                    //}

                    foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
                    {
                        if (x.musicIndexPlay >= 0 && x.musicIndexPlay < 2)
                            x.musicIndexPlay += 1;

                        x.ChangeMusic();
                        Plugin.Log.LogInfo($"musicName: {GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1)}");
                    }
                }

                if (Input.GetKeyDown(Plugin.weakKey.Value))
                {
                    //foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
                    //{
                    //    x.enemyComplexity += 1;
                    //    Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
                    //}

                    foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
                    {
                        if (x.musicIndexPlay > 0 && x.musicIndexPlay <= 2)
                            x.musicIndexPlay -= 1;

                        x.ChangeMusic();
                        Plugin.Log.LogInfo($"musicName: {GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1)}");
                    }
                }

                if (Input.GetKeyDown(Plugin.showTVHintKey.Value))
                {
                    foreach (var x in GameObject.FindObjectsOfType<MinigamesTelevisionController>())
                    {
                        x.KeysMenuActiveTrue();
                        Plugin.Log.LogInfo($"KeysMenuActiveTrue");
                    }

                    foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
                    {
                        x.eventFinishPlayer.Invoke();
                        if (x.indexFinishMita >= 3)
                        {
                            GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart/Canvas").GetComponent<ObjectInteractive_CaseInfo>().dontDestroyAfter = true;
                            GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart").GetComponent<ObjectInteractive>().active = true;
                        }
                        Plugin.Log.LogInfo($"DanceGame eventFinishPlayer");
                    }
                }

# if DEBUG
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    GameObject quest4 = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой");
                    if (quest4 != null) quest4.SetActive(true);

                    GameObject quest4Interactive = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
                    if (quest4Interactive != null) quest4Interactive.SetActive(true);

                    GameObject quest4HouseMain = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/House/Main");
                    if (quest4HouseMain != null) quest4HouseMain.SetActive(true);

                    GameObject actMitaKepka = GameObject.Find("World/Acts/Mita Кепка");
                    if (actMitaKepka != null) actMitaKepka.SetActive(true);

                    GameObject interactiveDance = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Dance");
                    if (interactiveDance != null)
                    {
                        ObjectInteractive danceObjectInteractive = interactiveDance.GetComponent<ObjectInteractive>();
                        if (danceObjectInteractive != null) danceObjectInteractive.active = true;
                    }

                    GlobalTag.player.transform.position = new Vector3(-18.63f, 0f, -1.47f);

                    GameObject interactiveDoor = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Door");
                    if (interactiveDoor != null)
                    {
                        ObjectInteractive doorObjectInteractive = interactiveDoor.GetComponent<ObjectInteractive>();
                        if (doorObjectInteractive != null)
                        {
                            doorObjectInteractive.active = true;
                            //doorObjectInteractive.Click();
                        }
                    }

                }
# endif
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
