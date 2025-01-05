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
                //Enabled = !Enabled;

                foreach (var obj in GameObject.FindObjectsOfType<MeshRenderer>())
                {
                    if (!(obj.gameObject.TryGetComponent<BoxCollider>(out var collider)))
                    {
                        var x = obj.gameObject.AddComponent<BoxCollider>();
                        x.isTrigger = true;
                    }
                }

                Camera mainCamera = GlobalTag.cameraPlayer.GetComponent<Camera>();
                if (mainCamera == null)
                {
                    Plugin.Log.LogWarning("Main camera not found!");
                    return;
                }

                Vector3 centerScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);

                // Create a ray from the center of the screen
                Ray ray = mainCamera.ScreenPointToRay(centerScreenPosition);

                // Store hit information
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Log the name of the hit GameObject
                    GameObject hitObject = hit.collider.gameObject;
                    Plugin.Log.LogWarning($"Hit GameObject: {hitObject.name}");

                    // Example: Add custom behavior to the hit object
                    //hitObject.GetComponent<Renderer>()?.material.SetColor("_Color", Color.red);
                    //GlobalTag.player.GetComponent<PlayerMove>().RemoveItem();
                    if (hitObject != null)
                    {
                        ObjectItem o = null;
                        if (!(hitObject.TryGetComponent<ObjectItem>(out var c)))
                        {
                            o = hitObject.AddComponent<ObjectItem>();
                            //o.castBack = 0f;
                            //o.castBackDistance = 10f;
                            o.itemInHand = hitObject;
                        }

                        foreach (var x in Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetType("PlayerHandIK_Prefab")))
                        {
                            if (o != null)
                            {
                                o.mainHand = x.TryCast<PlayerHandIK_Prefab>();
                                break;
                            }
                        }
                        GlobalTag.player.GetComponent<PlayerMove>().TakeItem(hitObject);

                    }
                    else
                    {
                        Plugin.Log.LogWarning("No GameObject hit by the raycast.");
                    }

                    //GlobalTag.player.GetComponent<PlayerMove>().RemoveItem();
                    //if(GameObject.Find("World/House/Room Start/InfinityRoom/Decorations/DigitalClock") != null)
                    //{
                    //    var o = GameObject.Find("World/House/Room Start/InfinityRoom/Decorations/DigitalClock").AddComponent<ObjectItem>();
                    //    o.castBack = 0.2f;
                    //    o.castBackDistance = 1f;
                    //    o.itemInHand = GameObject.Find("World/House/Room Start/InfinityRoom/Decorations/DigitalClock");

                    //    foreach(var x in Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetType("ObjectItem"))) {
                    //        Plugin.Log.LogInfo(x);
                    //        var objectItem = x.TryCast<ObjectItem>();
                    //        Plugin.Log.LogInfo(objectItem);
                    //        Plugin.Log.LogInfo(objectItem.mainHand);
                    //        if (objectItem.mainHand != null)
                    //            o.mainHand = objectItem.mainHand;
                    //    }


                    //    GlobalTag.player.GetComponent<PlayerMove>().TakeItem(GameObject.Find("World/House/Room Start/InfinityRoom/Decorations/DigitalClock"));
                    //}
                }

            }
        }
    }
}
