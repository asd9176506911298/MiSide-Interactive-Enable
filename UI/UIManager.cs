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
using UnityEngine.UI;

namespace InteractiveEnable.UI
{
 

    public class UIManager
    {
        public static List<GameObject> prefabObjects = new List<GameObject>();
        public static List<GameObject> interactiveObjects = new List<GameObject>();

        public static UniverseLib.AssetBundle ab;
        public static GameObject PointCameraLook;
        private static LineRenderer lineRenderer;
        private static GameObject lineObject;
        private static GameObject rectangleObject;

        private static GameObject uiLineObject;
        private static RectTransform uiLineTransform;


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
            if (Input.GetKeyDown(Plugin.Instance.ShowMenuKey.Value))
            {
                Enabled = !Enabled;

                // Ensure Canvas exists
                Canvas canvas = GetOrCreateCanvas();

                // Create the rectangle
                if (rectangleObject == null)
                {
                    rectangleObject = new GameObject("ScreenRectangle");
                    rectangleObject.transform.SetParent(canvas.transform);

                    // Create four sides for the rectangle
                    GameObject topSide = CreateSide("TopSide");
                    GameObject bottomSide = CreateSide("BottomSide");
                    GameObject leftSide = CreateSide("LeftSide");
                    GameObject rightSide = CreateSide("RightSide");

                    // Set each side as a child of the rectangle object
                    topSide.transform.SetParent(rectangleObject.transform);
                    bottomSide.transform.SetParent(rectangleObject.transform);
                    leftSide.transform.SetParent(rectangleObject.transform);
                    rightSide.transform.SetParent(rectangleObject.transform);

                    // Adjust the rectangle's sides based on the target objects' positions
                    AdjustRectangleSides(topSide, bottomSide, leftSide, rightSide);
                }

                // Create the line UI overlay
                if (uiLineObject == null)
                {
                    uiLineObject = new GameObject("ScreenToObjectLine");
                    uiLineObject.AddComponent<Image>();
                    uiLineObject.transform.SetParent(canvas.transform, false);

                    // Add RectTransform for position/sizing
                    uiLineTransform = uiLineObject.GetComponent<RectTransform>();
                    uiLineTransform.anchorMin = Vector2.zero;
                    uiLineTransform.anchorMax = Vector2.zero;
                    uiLineTransform.pivot = new Vector2(0.5f, 0.5f);

                    // Style the line
                    var lineImage = uiLineObject.GetComponent<Image>();
                    lineImage.color = Color.green;
                }
            }

            if (rectangleObject != null && uiLineObject != null)
            {
                // Update based on the target's visibility
                UpdateRectangleAndLine();
            }
            //if (PointCameraLook != null)
            //{
            //    // Get the camera reference
            //    Camera camera = GameObject.Find("MenuGame/Camera").GetComponent<Camera>();

            //    // Get the mouse position in screen space
            //    Vector3 mouseScreenPosition = Input.mousePosition;

            //    // Calculate the depth: distance between camera and PointCameraLook
            //    float depth = Mathf.Abs(camera.transform.position.z - PointCameraLook.transform.position.z);

            //    // Convert the screen position to a world position using the calculated depth
            //    Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(
            //        new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, depth)
            //    );

            //    // Set the position of PointCameraLook to the converted world position
            //    PointCameraLook.transform.position = mouseWorldPosition;

            //    // Log information using Plugin.Log.LogInfo
            //    Plugin.Log.LogInfo($"mouseScreenPosition: {mouseScreenPosition}");
            //    Plugin.Log.LogInfo($"mouseWorldPosition: {mouseWorldPosition}");
            //    Plugin.Log.LogInfo($"depth: {depth}");
            //}
        }
        private static void UpdateRectangleAndLine()
        {
            // Find the target objects
            GameObject headObject = GameObject.Find("World/General/Mita/MitaPerson Mita/Armature/Hips/Spine/Chest/Neck2/Neck1/Head");
            GameObject hipObject = GameObject.Find("World/General/Mita/MitaPerson Mita/Armature/Hips");

            // Ensure both objects exist
            if (headObject == null || hipObject == null)
            {
                // Hide both elements if any target is missing
                SetActiveState(false);
                return;
            }

            // Get world positions
            Vector3 headPosition = headObject.transform.position;
            Vector3 hipPosition = hipObject.transform.position;

            // Convert positions to screen space
            Vector3 headScreenPoint = Camera.main.WorldToScreenPoint(headPosition);
            Vector3 hipScreenPoint = Camera.main.WorldToScreenPoint(hipPosition);

            // Check if the hip object is within the camera's view
            if (IsWithinScreenBounds(hipScreenPoint))
            {
                // Target is visible, update the rectangle and line
                SetActiveState(true);

                if (Plugin.Instance.isRect.Value)
                    UpdateRectangle(hipScreenPoint);
                else
                    rectangleObject?.SetActive(false);

                if (Plugin.Instance.isLine.Value)
                    UpdateLine(headScreenPoint);
                else
                    uiLineObject?.SetActive(false);
            }
            else
            {
                // Hide the rectangle and line if the hip object is out of view
                SetActiveState(false);
            }
        }

        private static bool IsWithinScreenBounds(Vector3 screenPoint)
        {
            // Check if the point is within the screen's bounds and in front of the camera
            return screenPoint.z > 0 &&
                   screenPoint.x > 0 && screenPoint.x < Screen.width &&
                   screenPoint.y > 0 && screenPoint.y < Screen.height;
        }

        private static void SetActiveState(bool isActive)
        {
            rectangleObject?.SetActive(isActive);
            uiLineObject?.SetActive(isActive);
        }

        private static void UpdateRectangle(Vector3 targetScreenPosition)
        {
            rectangleObject.SetActive(true);

            // Define rectangle dimensions (adjust width and height as needed)
            float rectWidth = 100f;
            float rectHeight = 250f;

            // Define rectangle corners in screen space
            Vector3 topLeft = new Vector3(targetScreenPosition.x - rectWidth / 2, targetScreenPosition.y + rectHeight / 2, targetScreenPosition.z);
            Vector3 topRight = new Vector3(targetScreenPosition.x + rectWidth / 2, targetScreenPosition.y + rectHeight / 2, targetScreenPosition.z);
            Vector3 bottomLeft = new Vector3(targetScreenPosition.x - rectWidth / 2, targetScreenPosition.y - rectHeight / 2, targetScreenPosition.z);
            Vector3 bottomRight = new Vector3(targetScreenPosition.x + rectWidth / 2, targetScreenPosition.y - rectHeight / 2, targetScreenPosition.z);

            // Update the rectangle sides' positions
            UpdateRectangleSides(topLeft, topRight, bottomLeft, bottomRight);
        }

        private static void UpdateLine(Vector3 targetScreenPosition)
        {
            uiLineObject.SetActive(true);

            // Screen center (starting point of the line)
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            // Update UI Line Transform
            if (uiLineTransform != null)
            {
                // Set the position of the line to the midpoint between the two points
                Vector3 midpoint = (screenCenter + targetScreenPosition) / 2;
                uiLineTransform.position = midpoint;

                // Calculate the distance and angle between the two points
                float distance = Vector3.Distance(screenCenter, targetScreenPosition);
                float angle = Mathf.Atan2(targetScreenPosition.y - screenCenter.y, targetScreenPosition.x - screenCenter.x) * Mathf.Rad2Deg;

                // Adjust the line size and rotation
                uiLineTransform.sizeDelta = new Vector2(distance, 2f); // 2px width line
                uiLineTransform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private static GameObject CreateSide(string name)
        {
            GameObject side = new GameObject(name);
            side.AddComponent<Image>();
            Image image = side.GetComponent<Image>();
            image.color = Color.red; // Set the color of the side

            RectTransform rectTransform = side.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1, 5); // Default size, we will adjust this later

            return side;
        }

        private static void AdjustRectangleSides(GameObject topSide, GameObject bottomSide, GameObject leftSide, GameObject rightSide)
        {
            // Set default positions for the rectangle sides
            RectTransform topRect = topSide.GetComponent<RectTransform>();
            topRect.anchorMin = new Vector2(0, 1);
            topRect.anchorMax = new Vector2(0, 1);
            topRect.pivot = new Vector2(0, 1);

            RectTransform bottomRect = bottomSide.GetComponent<RectTransform>();
            bottomRect.anchorMin = new Vector2(0, 0);
            bottomRect.anchorMax = new Vector2(0, 0);
            bottomRect.pivot = new Vector2(0, 0);

            RectTransform leftRect = leftSide.GetComponent<RectTransform>();
            leftRect.anchorMin = new Vector2(0, 0);
            leftRect.anchorMax = new Vector2(0, 0);
            leftRect.pivot = new Vector2(0, 0);

            RectTransform rightRect = rightSide.GetComponent<RectTransform>();
            rightRect.anchorMin = new Vector2(1, 0);
            rightRect.anchorMax = new Vector2(1, 0);
            rightRect.pivot = new Vector2(1, 0);
        }

        private static void UpdateRectangleSides(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            // Correct the positions and sizes for the sides of the rectangle based on screen positions
            RectTransform topRect = rectangleObject.transform.Find("TopSide").GetComponent<RectTransform>();
            topRect.position = topLeft;
            topRect.sizeDelta = new Vector2(Vector3.Distance(topLeft, topRight), 5); // Width of the top side

            RectTransform bottomRect = rectangleObject.transform.Find("BottomSide").GetComponent<RectTransform>();
            bottomRect.position = bottomLeft;
            bottomRect.sizeDelta = new Vector2(Vector3.Distance(bottomLeft, bottomRight), 5); // Width of the bottom side

            RectTransform leftRect = rectangleObject.transform.Find("LeftSide").GetComponent<RectTransform>();
            leftRect.position = bottomLeft;
            leftRect.sizeDelta = new Vector2(5, Vector3.Distance(topLeft, bottomLeft)); // Height of the left side
            Plugin.Log.LogError(Vector3.Distance(topLeft, bottomLeft));
            RectTransform rightRect = rectangleObject.transform.Find("RightSide").GetComponent<RectTransform>();
            rightRect.position = bottomRight;
            rightRect.sizeDelta = new Vector2(5, Vector3.Distance(topRight, bottomRight)); // Height of the right side
        }

        // Get or create the Canvas
        private static Canvas GetOrCreateCanvas()
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                // Create a new Canvas if none exists
                GameObject canvasObject = new GameObject("UI Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Ensure it's screen space
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>(); // Add this to allow UI interactions (if necessary)
            }
            return canvas;
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
