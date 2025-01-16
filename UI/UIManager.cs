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
using UnityEngineInternal.Video;
using UnityEngine.Video;
using System.Net.Sockets;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using Colorful;

namespace InteractiveEnable.UI
{
 

    public class UIManager
    {
        public static List<GameObject> prefabObjects = new List<GameObject>();
        public static List<GameObject> interactiveObjects = new List<GameObject>();

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
            ab = UniverseLib.AssetBundle.LoadFromFile($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/maxwell");
            //Plugin.Log.LogError($"{Assembly.GetExecutingAssembly().Location}");
            //Plugin.Log.LogError($"Path: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            //Plugin.Log.LogError($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/maxwell");
        }

        private static void OnUpdate()
        {
            if (Input.GetKeyDown(Plugin.Instance.ShowMenuKey.Value))
            {
                Enabled = !Enabled;
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                //GameObject toClone = GameObject.FindObjectsOfType<ObjectInteractive>().First().gameObject;
                //GameObject cloned = UnityEngine.Object.Instantiate(toClone);
                //cloned.transform.position = GameObject.Find("World/Quests/Quest 2 Walking/DialogueQuest Mita/InteractiveMita").transform.position;
                //Outlinable outlineable = cloned.GetComponent<Outlinable>();
                //outlineable.OutlineTargets.Clear();
                //Renderer bed = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaBody").GetComponent<Renderer>();
                //outlineable.OutlineTargets.Add(new OutlineTarget(bed, ""));

                var outlineables = GameObject.FindObjectsOfType<Outlinable>(true)
                 .Where(mat => mat.gameObject.name.Contains("Mita"))
                 .ToArray();

                if (outlineables.Length > 0)
                {
                    var mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaArms").GetComponent<Renderer>();
                    OutlineTarget target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents= Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaBody").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaZkns").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaEyes").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaFace").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaHead").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);

                    mesh = GameObject.Find("World/Acts/Act General/MitaPerson Mita/MayaTeth").GetComponent<Renderer>();
                    target = new OutlineTarget(mesh)
                    {
                        BoundsMode = BoundsMode.Manual,
                        CullMode = UnityEngine.Rendering.CullMode.Off,
                        Bounds = new Bounds(Vector3.forward, Vector3.one) { extents = Vector3.one }
                    };
                    outlineables[0].OutlineTargets.Add(target);
                }

                Plugin.Log.LogWarning("Press F4");
            }
        }

        public static void CreateInteractiveObject(string prefabName, string animatorName = "")
        {
            GameObject toClone = GameObject.FindObjectsOfType<ObjectInteractive>().First().gameObject;
            GameObject cloned = UnityEngine.Object.Instantiate(toClone);

            // Load the "fish" prefab and instantiate it
            var prefab = ab.LoadAsset<GameObject>(prefabName);
            GameObject prefabInstance = GameObject.Instantiate(prefab);
            prefabInstance.transform.position = GlobalTag.cameraPlayer.transform.position;

            // Configure the cloned GameObject
            cloned.name = $"Interactive {prefabName}";
            cloned.transform.position = prefabInstance.transform.position;
            //cloned.transform.parent = prefabInstance.transform;

            // Synchronize BoxCollider settings
            BoxCollider clonedCollider = cloned.GetComponent<BoxCollider>();
            BoxCollider prefabCollider = prefabInstance.GetComponent<BoxCollider>();
            clonedCollider.center = prefabCollider.center;
            clonedCollider.size = prefabCollider.size;
            clonedCollider.extents = prefabCollider.extents;
            clonedCollider.isTrigger = true;
            prefabCollider.isTrigger = true;
            

            // Try to configure the VideoPlayer on the fish instance if it exists
            if (prefabInstance.TryGetComponent<VideoPlayer>(out VideoPlayer prefabVideoPlayer))
            {
                prefabVideoPlayer.playOnAwake = false;
                prefabVideoPlayer.isLooping = false;
            }

            // Add the Renderer of the fish to the outline targets
            Outlinable outlineable = cloned.GetComponent<Outlinable>();
            outlineable.OutlineTargets.Clear();
            Renderer prefabRender;
            if (animatorName != "")
                prefabRender = prefabInstance.transform.Find($"Animator/{prefabName}").GetComponent<Renderer>();
            else
                prefabRender = prefabInstance.GetComponent<Renderer>();
            //Renderer prefabRender = prefabInstance.transform.Find("Animator/Maxwell").GetComponent<Renderer>();
            outlineable.OutlineTargets.Add(new OutlineTarget(prefabRender, ""));

            var aduio = prefabInstance.GetComponentInChildren<AudioSource>();
            //aduio.mute = true;

            // Configure the interactive behavior
            ObjectInteractive interactable = cloned.GetComponent<ObjectInteractive>();
            interactable.active = true;
            interactable.eventClick = new UnityEvent();
            interactable.eventClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>((Delegate)(Action)delegate
            {
                if (Plugin.Instance.isMute.Value)
                    aduio.mute = true;
                else
                    aduio.mute = false;

                if (prefabVideoPlayer != null)
                {
                    prefabVideoPlayer.isLooping = true;
                    if (prefabVideoPlayer.clockTime == 0.0 || (prefabVideoPlayer.clockTime >= prefabVideoPlayer.length))
                        prefabVideoPlayer.Play();
                    else
                        prefabVideoPlayer.Stop();

                }

                if(animatorName != "")
                {
                    var animator = prefabInstance.GetComponentInChildren<Animator>();
                    animator.SetBoolString("rotate", !animator.GetBoolString("rotate"));

                    var aduio = prefabInstance.GetComponentInChildren<AudioSource>();
                    if (animator.GetBoolString("rotate"))
                        aduio.Play();
                    else
                        aduio.Stop();
                }
                
            }));

            // Copy properties from the source case info to the target case info
            ObjectInteractive_CaseInfo caseInfoTarget = cloned.GetComponentInChildren<ObjectInteractive_CaseInfo>();
            ObjectInteractive_CaseInfo caseInfoSource = toClone.GetComponentInChildren<ObjectInteractive_CaseInfo>();
            if (caseInfoSource != null && caseInfoTarget != null)
            {
                caseInfoTarget.cameraT = caseInfoSource.cameraT;
                caseInfoTarget.frameText = caseInfoSource.frameText;
                caseInfoTarget.text = caseInfoSource.text;
                caseInfoTarget.colorGradient1 = caseInfoSource.colorGradient1;
                caseInfoTarget.colorGradient2 = caseInfoSource.colorGradient2;
                caseInfoTarget.dontDestroyAfter = true;
            }

            prefabObjects.Add(prefabInstance);
            interactiveObjects.Add(cloned);
        }

        public static void DestroyAllMeme()
        {
            foreach(var x in prefabObjects)
            {
                GameObject.Destroy(x);
            }

            foreach (var y in interactiveObjects)
            {
                GameObject.Destroy(y);
            }
        }
    }
}
