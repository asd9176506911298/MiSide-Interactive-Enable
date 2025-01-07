using BepInEx;
using EPOOutline;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UniverseLib.UI;

namespace InteractiveEnable.UI
{
 

    public class UIManager
    {
        public static UniverseLib.AssetBundle ab;
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
            ab = UniverseLib.AssetBundle.LoadFromFile($"{Paths.PluginPath}/maxwell");
        }

        private static void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F4))
            {
                
                if (ab != null)
                {
                    //var x = ab.LoadAsset<GameObject>("maxwell");
                    //var g = GameObject.Instantiate(x);
                    //var x = ab.LoadAsset<GameObject>("fish");
                    //var g = GameObject.Instantiate(x);

                    GameObject toClone = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Book/");
                    GameObject cloned = UnityEngine.Object.Instantiate(toClone);
                    GameObject interactives = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
                    cloned.transform.parent = interactives.transform;
                    cloned.transform.position = new Vector3(-24.7664f, 0.712f, 6.1761f);
                    Outlinable outlineable = cloned.GetComponent<Outlinable>();
                    outlineable.OutlineTargets.Clear();
                    Renderer bed = GameObject.Find("World/House/Bedroom/Bedroom/Bed").GetComponent<Renderer>();
                    outlineable.OutlineTargets.Add(new OutlineTarget(bed, ""));
                    ObjectInteractive interactable = cloned.GetComponent<ObjectInteractive>();
                    interactable.active = true;
                    interactable.eventClick = new UnityEvent();
                    interactable.eventClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Delegate)(Action)delegate
                    {
                        var x = ab.LoadAsset<GameObject>("fish");
                        var g = GameObject.Instantiate(x);
                        g.transform.position = new Vector3(-24.76f, 1.21f, 6.67f);
                        g.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                        GameObject.Destroy(g, 5f);
                    }));
                    ObjectInteractive_CaseInfo caseInfoTarget = cloned.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    ObjectInteractive_CaseInfo caseInfoSource = toClone.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    caseInfoTarget.cameraT = caseInfoSource.cameraT;
                    caseInfoTarget.frameText = caseInfoSource.frameText;
                    caseInfoTarget.text = caseInfoSource.text;
                    caseInfoTarget.colorGradient1 = caseInfoSource.colorGradient1;
                    caseInfoTarget.colorGradient2 = caseInfoSource.colorGradient2;
                    caseInfoTarget.dontDestroyAfter = true;
                    Location34_Communication communication = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой").GetComponent<Location34_Communication>();
                    //CollectionExtensions.AddItem<GameObject>((IEnumerable<GameObject>)communication.addonInteractive, cloned);
                    Il2CppReferenceArray<GameObject> newArr = new Il2CppReferenceArray<GameObject>((long)(((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count + 1));
                    for (int i = 0; i < ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count; i++)
                    {
                        ((Il2CppArrayBase<GameObject>)(object)newArr)[i] = ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive)[i];
                    }
        ((Il2CppArrayBase<GameObject>)(object)newArr)[((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count] = cloned;
                    communication.addonInteractive = newArr;
                }
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {

                if (ab != null)
                {
                    //var x = ab.LoadAsset<GameObject>("maxwell");
                    //var g = GameObject.Instantiate(x);
                    //var x = ab.LoadAsset<GameObject>("fish");
                    //var g = GameObject.Instantiate(x);

                    GameObject toClone = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Book/");
                    GameObject cloned = UnityEngine.Object.Instantiate(toClone);
                    GameObject interactives = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
                    cloned.transform.parent = interactives.transform;
                    cloned.transform.position = new Vector3(-24.7664f, 0.712f, 6.1761f);
                    Outlinable outlineable = cloned.GetComponent<Outlinable>();
                    outlineable.OutlineTargets.Clear();
                    Renderer bed = GameObject.Find("World/House/Bedroom/Bedroom/Bed").GetComponent<Renderer>();
                    outlineable.OutlineTargets.Add(new OutlineTarget(bed, ""));
                    ObjectInteractive interactable = cloned.GetComponent<ObjectInteractive>();
                    interactable.active = true;
                    interactable.eventClick = new UnityEvent();
                    interactable.eventClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Delegate)(Action)delegate
                    {
                        var x = ab.LoadAsset<GameObject>("maxwell");
                        var fake = new GameObject("fakeMaxwell");
                        fake.transform.position = new Vector3(-24.76f, 0.81f, 6.67f);
                        fake.transform.rotation = Quaternion.Euler(0f,90f,0f);
                        var g = GameObject.Instantiate(x,fake.transform);
                        g.GetComponent<AudioSource>().volume = 0.5f;
                        GameObject.Destroy(fake, 7f);
                    }));
                    ObjectInteractive_CaseInfo caseInfoTarget = cloned.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    ObjectInteractive_CaseInfo caseInfoSource = toClone.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    caseInfoTarget.cameraT = caseInfoSource.cameraT;
                    caseInfoTarget.frameText = caseInfoSource.frameText;
                    caseInfoTarget.text = caseInfoSource.text;
                    caseInfoTarget.colorGradient1 = caseInfoSource.colorGradient1;
                    caseInfoTarget.colorGradient2 = caseInfoSource.colorGradient2;
                    caseInfoTarget.dontDestroyAfter = true;
                    Location34_Communication communication = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой").GetComponent<Location34_Communication>();
                    //CollectionExtensions.AddItem<GameObject>((IEnumerable<GameObject>)communication.addonInteractive, cloned);
                    Il2CppReferenceArray<GameObject> newArr = new Il2CppReferenceArray<GameObject>((long)(((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count + 1));
                    for (int i = 0; i < ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count; i++)
                    {
                        ((Il2CppArrayBase<GameObject>)(object)newArr)[i] = ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive)[i];
                    }
        ((Il2CppArrayBase<GameObject>)(object)newArr)[((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count] = cloned;
                    communication.addonInteractive = newArr;
                }
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {

                if (ab != null)
                {
                    //var x = ab.LoadAsset<GameObject>("maxwell");
                    //var g = GameObject.Instantiate(x);
                    //var x = ab.LoadAsset<GameObject>("fish");
                    //var g = GameObject.Instantiate(x);

                    GameObject toClone = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Book/");
                    GameObject cloned = UnityEngine.Object.Instantiate(toClone);
                    GameObject interactives = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
                    cloned.transform.parent = interactives.transform;
                    cloned.transform.position = new Vector3(-24.7664f, 0.712f, 6.1761f);
                    Outlinable outlineable = cloned.GetComponent<Outlinable>();
                    outlineable.OutlineTargets.Clear();
                    Renderer bed = GameObject.Find("World/House/Bedroom/Bedroom/Bed").GetComponent<Renderer>();
                    outlineable.OutlineTargets.Add(new OutlineTarget(bed, ""));
                    ObjectInteractive interactable = cloned.GetComponent<ObjectInteractive>();
                    interactable.active = true;
                    interactable.eventClick = new UnityEvent();
                    interactable.eventClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Delegate)(Action)delegate
                    {
                        var x = ab.LoadAsset<GameObject>("oii");
                        var g = GameObject.Instantiate(x);
                        g.transform.position = new Vector3(-24.76f, 1.21f, 6.67f);
                        g.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                        GameObject.Destroy(g, 5f);
                    }));
                    ObjectInteractive_CaseInfo caseInfoTarget = cloned.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    ObjectInteractive_CaseInfo caseInfoSource = toClone.GetComponentInChildren<ObjectInteractive_CaseInfo>();
                    caseInfoTarget.cameraT = caseInfoSource.cameraT;
                    caseInfoTarget.frameText = caseInfoSource.frameText;
                    caseInfoTarget.text = caseInfoSource.text;
                    caseInfoTarget.colorGradient1 = caseInfoSource.colorGradient1;
                    caseInfoTarget.colorGradient2 = caseInfoSource.colorGradient2;
                    caseInfoTarget.dontDestroyAfter = true;
                    Location34_Communication communication = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой").GetComponent<Location34_Communication>();
                    //CollectionExtensions.AddItem<GameObject>((IEnumerable<GameObject>)communication.addonInteractive, cloned);
                    Il2CppReferenceArray<GameObject> newArr = new Il2CppReferenceArray<GameObject>((long)(((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count + 1));
                    for (int i = 0; i < ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count; i++)
                    {
                        ((Il2CppArrayBase<GameObject>)(object)newArr)[i] = ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive)[i];
                    }
        ((Il2CppArrayBase<GameObject>)(object)newArr)[((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count] = cloned;
                    communication.addonInteractive = newArr;
                }
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var x = ab.LoadAsset<GameObject>("pochita");
                var g = GameObject.Instantiate(x);

                //g.transform.position = GameObject.Find("World/Quests/Quest1 Побег по коридорам/Mita/MitaPerson Mita/Armature/Hips/Spine/Chest/Right shoulder/Right arm/Right elbow/Right wrist/Right item").transform.position;

                g.transform.position = new Vector3(3.55f, 0.45f, 4.05f);
                g.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                g.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
                g.transform.parent = GameObject.Find("World/Quests/Quest1 Побег по коридорам/Mita/MitaPerson Mita/Armature/Hips/Spine/Chest/Right shoulder/Right arm/Right elbow/Right wrist/Right item").transform;

                var f1 = GameObject.Instantiate(ab.LoadAsset<GameObject>("future"));
                f1.transform.position = new Vector3(-1.5f, 1.7f, -3.8f);
                f1.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                f1.transform.localScale = new Vector3(1.126f, 2f, 2f);

                var f2 = GameObject.Instantiate(ab.LoadAsset<GameObject>("future"));
                f2.transform.position = new Vector3(2.5f, 1.7f, -3.8f);
                f2.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                f2.transform.localScale = new Vector3(1.126f, 2f, 2f);

                var f3Right = GameObject.Instantiate(ab.LoadAsset<GameObject>("future"));
                f3Right.transform.position = new Vector3(-3.99f, 1.1f, 0.48f);
                f3Right.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                f3Right.transform.localScale = new Vector3(1.126f, 2.3f, 2f);

                var f4Left = GameObject.Instantiate(ab.LoadAsset<GameObject>("future"));
                f4Left.transform.position = new Vector3(4.98f, 1.1f, 0.48f);
                f4Left.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                f4Left.transform.localScale = new Vector3(1.126f, 2.3f, 2f);

                var l1 = new GameObject("light");
                var light1 = l1.AddComponent<Light>();
                l1.transform.position = new Vector3(0f, 4.3f, 0f);
                light1.range = 40f;
                light1.color = new Color(1f, 0.55f, 1f, 1f);

                var l2 = new GameObject("light");
                var light2 = l2.AddComponent<Light>();
                l2.transform.position = new Vector3(0.5f, 1f, 6.31f);
                light2.range = 20f;

                //g.transform.parent = GameObject.Find("World/Quests/Quest1 Побег по коридорам/Mita/MitaPerson Mita/Armature/Hips/Spine/Chest/Right shoulder/Right arm/Right elbow/Right wrist/Right item").transform;
            }
            if (Input.GetKeyDown(Plugin.Instance.ShowMenuKey.Value))
            {
                Enabled = !Enabled;
                

        //        var ab = UniverseLib.AssetBundle.LoadFromFile($"{Paths.PluginPath}/maxwell");
        //        if (ab != null)
        //        {
        //            //var x = ab.LoadAsset<GameObject>("maxwell");
        //            //var g = GameObject.Instantiate(x);
        //            //var x = ab.LoadAsset<GameObject>("fish");
        //            //var g = GameObject.Instantiate(x);

        //            GameObject toClone = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Book/");
        //            GameObject cloned = UnityEngine.Object.Instantiate(toClone);
        //            GameObject interactives = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
        //            cloned.transform.parent = interactives.transform;
        //            cloned.transform.position = new Vector3(-24.7664f, 0.712f, 6.1761f);
        //            Outlinable outlineable = cloned.GetComponent<Outlinable>();
        //            outlineable.OutlineTargets.Clear();
        //            Renderer bed = GameObject.Find("World/House/Bedroom/Bedroom/Bed").GetComponent<Renderer>();
        //            outlineable.OutlineTargets.Add(new OutlineTarget(bed, ""));
        //            ObjectInteractive interactable = cloned.GetComponent<ObjectInteractive>();
        //            interactable.active = true;
        //            interactable.eventClick = new UnityEvent();
        //            interactable.eventClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Delegate)(Action)delegate
        //            {
        //                var x = ab.LoadAsset<GameObject>("oii");
        //                var g = GameObject.Instantiate(x);
        //                GameObject.Destroy(g, 5f);
        //            }));
        //            ObjectInteractive_CaseInfo caseInfoTarget = cloned.GetComponentInChildren<ObjectInteractive_CaseInfo>();
        //            ObjectInteractive_CaseInfo caseInfoSource = toClone.GetComponentInChildren<ObjectInteractive_CaseInfo>();
        //            caseInfoTarget.cameraT = caseInfoSource.cameraT;
        //            caseInfoTarget.frameText = caseInfoSource.frameText;
        //            caseInfoTarget.text = caseInfoSource.text;
        //            caseInfoTarget.colorGradient1 = caseInfoSource.colorGradient1;
        //            caseInfoTarget.colorGradient2 = caseInfoSource.colorGradient2;
        //            caseInfoTarget.dontDestroyAfter = true;
        //            Location34_Communication communication = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой").GetComponent<Location34_Communication>();
        //            //CollectionExtensions.AddItem<GameObject>((IEnumerable<GameObject>)communication.addonInteractive, cloned);
        //            Il2CppReferenceArray<GameObject> newArr = new Il2CppReferenceArray<GameObject>((long)(((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count + 1));
        //            for (int i = 0; i < ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count; i++)
        //            {
        //                ((Il2CppArrayBase<GameObject>)(object)newArr)[i] = ((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive)[i];
        //            }
        //((Il2CppArrayBase<GameObject>)(object)newArr)[((Il2CppArrayBase<GameObject>)(object)communication.addonInteractive).Count] = cloned;
        //            communication.addonInteractive = newArr;

        //            //var x = ab.LoadAsset<GameObject>("oii");
        //            //var g = GameObject.Instantiate(x);
        //            //GameObject toClone = GameObject.Find("World/Quests/Quest 1/Addon/Interactive PhotoMita");
        //            //GameObject cloned = GameObject.Instantiate<GameObject>(toClone);
        //            //g.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //            //g.transform.position = GameObject.Find("GameController/Player/Person/Arms").transform.position;
        //            //g.transform.SetParent(GameObject.Find("GameController/Player/Person/Arms").transform);
        //        }
                return;
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
