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
            }
        }
    }
}
