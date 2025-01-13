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
using System.Runtime.InteropServices.ObjectiveC;

namespace InteractiveEnable.UI
{
 

    public class UIManager
    {
        public static List<GameObject> prefabObjects = new List<GameObject>();
        public static List<GameObject> interactiveObjects = new List<GameObject>();

        public static GameObject enemyPrefab = null;

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
            ab = UniverseLib.AssetBundle.LoadFromFile($"{Paths.PluginPath}/InteractiveEnable/maxwell");
        }

        private static void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var enemy = GameObject.Find("World/Quest/Quest 1 (Room 1 - Room 6)/Mita Maneken 1/MitaManeken 1");
                if(enemy != null)
                {
                    if(enemyPrefab == null)
                        enemyPrefab = enemy;
                }

                var e = GameObject.Instantiate(enemyPrefab, new Vector3(-12.037f, -3.6816f, -88.1799f), Quaternion.identity);
                e.GetComponent<Mob_Maneken>().enabled = true;
            }

            foreach (var x in GameObject.FindObjectsOfType<Mob_Maneken>())
            {
                x.speedNav = 2f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                //Array emotionValues = Enum.GetValues(typeof(EmotionType));

                //// Iterate through all MitaPerson objects
                //foreach (var x in GameObject.FindObjectsOfType<MitaPerson>())
                //{
                //    // Randomly pick an EmotionType
                //    EmotionType randomEmotion = (EmotionType)emotionValues.GetValue(UnityEngine.Random.Range(0, emotionValues.Length));

                //    // Apply the random emotion
                //    x.FaceEmotionType(randomEmotion);
                //}
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
                        if (hitObject.TryGetComponent<Mob_Maneken>(out var mobManeken))
                        {
                            // Destroy the game object if it has the Mob_Maneken component
                            GameObject.Destroy(hitObject);
                        }
                    }
                    else
                    {
                        Plugin.Log.LogWarning("No GameObject hit by the raycast.");
                    }
                }
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
