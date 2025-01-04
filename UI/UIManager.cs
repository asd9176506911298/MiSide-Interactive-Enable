using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib.UI;

namespace InteractiveEnable.UI
{
 

    public class UIManager
    {
        public static bool Enabled
        {
            get => UiBase is not null and { Enabled: true };
            set
            {
                if (UiBase is null || UiBase.Enabled == value)
                {
                    return;
                }

                UniversalUI.SetUIActive(MyPluginInfo.PLUGIN_GUID, value);
            }
        }

        internal static UIBase? UiBase { get; private set; }
        internal static MainPanel? Panel { get; private set; }

        public static void Init()
        {
            UiBase = UniversalUI.RegisterUI(MyPluginInfo.PLUGIN_GUID, null);

            Panel = new(UiBase);

            KeyInputHandler.Instance.onUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (Input.GetKeyDown(Plugin.Instance.ShowMenuKey.Value))
            {
                Enabled = !Enabled;

                if(GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/House/Toilet") != null)
                    GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/House/Toilet").SetActive(true);

                if(GameObject.Find("World/House/Bedroom/Bedroom") != null)
                    GameObject.Find("World/House/Bedroom/Bedroom").SetActive(true);

                if (GameObject.Find("World/House/Bedroom/DoorCage Bedroom-Hall") != null)
                    GameObject.Find("World/House/Bedroom/DoorCage Bedroom-Hall").SetActive(true);

                if (GameObject.Find("World/Acts/Mita Кепка/MitaPerson Know/RightItem/Vibrator") != null)
                {
                    GameObject.Find("World/Acts/Mita Кепка/MitaPerson Know/RightItem/Vibrator").SetActive(true);
                    GameObject.Find("World/Acts/Mita Кепка/MitaPerson Know/RightItem/Vibrator").transform.localPosition = new Vector3(0.04f, 0.07f, 0.06f);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GlobalTag.player.GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.VelocityChange);
            }

            if(GameObject.Find("World/Acts/Mita Кепка") != null)
            {
                GameObject.Find("World/Acts/Mita Кепка").GetComponent<MitaPerson>().AiWalkToTarget(GlobalTag.player.transform);
            }
        }
    }
}
