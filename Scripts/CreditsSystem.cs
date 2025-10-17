using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Generalisk.Credits
{
    public static class CreditsSystem
    {
        internal const string PACKAGE_ID = "com.generalisk.credits";

        public static void Play()
        {
            // Generate & Open Credits Scene
            var currentScene = SceneManager.GetActiveScene();
            var creditsScene = GenerateScene();
            SceneManager.SetActiveScene(creditsScene);
            SceneManager.UnloadSceneAsync(currentScene);

            // Finish up
            GameObject.FindFirstObjectByType<AudioSource>().Play();
        }

        private static Scene GenerateScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            var settings = CreditsSettings.Instance;

            // Create Scene
            var scene = SceneManager.CreateScene("Credits");
            SceneManager.SetActiveScene(scene);

            // Create Camera
            // this is to fix the screen not refreshing properly
            var cameraObj = new GameObject("Main Camera");
            var camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;

            // Create Music Player
            var musicObj = new GameObject("Music");
            var music = musicObj.AddComponent<AudioSource>();
#if USE_AUDIO_RESOURCE
            music.resource = settings.music;
#else
            music.clip = settings.music;
#endif
            // make sure the player can actually hear the music
            cameraObj.AddComponent<AudioListener>();

            // Create Manager Object
            var obj = new GameObject("Manager");
            var player = obj.AddComponent<CreditsPlayer>();
            player.music = music;

            // Create Canvas
            var canvasObj = new GameObject("Canvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(960, 640);
            canvasScaler.matchWidthOrHeight = 1;

            // Generate Credits
            var credits = GenerateCredits(canvas.GetComponent<RectTransform>());
            player.credits = credits;

            // Finish up
            SceneManager.SetActiveScene(currentScene);

            return scene;
        }

        private const float CREDITS_WIDTH = 960;

        private static RectTransform GenerateCredits(RectTransform parent)
        {
            var settings = CreditsSettings.Instance;

            // Create Credits Object
            var obj = new GameObject("Credits");
            obj.transform.SetParent(parent);
            var rect = obj.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(CREDITS_WIDTH, 0);
            rect.pivot = new Vector2(0.5f, 1);

            var layoutGroup = obj.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childScaleWidth = true;
            layoutGroup.childScaleHeight = true;
            layoutGroup.spacing = 42;

            var contentSizeFitter = obj.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Generate Contents
            foreach (var item in settings.items)
            { GenerateCreditsItem(item, rect); }

            // Finish up
            return rect;
        }

        private static void GenerateCreditsItem(CreditsItem item, RectTransform creditsObject)
        {
            var title = item.text;
            var obj = creditsObject;

            if (string.IsNullOrWhiteSpace(item.text))
            { title = "Untitled"; }

            // If item contains sub items
            if (item.subItems.Length > 0)
            {
                var x = new GameObject(title);
                x.transform.SetParent(obj);
                var xLayout = x.AddComponent<VerticalLayoutGroup>();
                xLayout.childAlignment = TextAnchor.UpperCenter;
                xLayout.childForceExpandHeight = false;
                xLayout.childScaleWidth = true;
                xLayout.childScaleHeight = true;
                xLayout.spacing = 12;

                obj = x.GetComponent<RectTransform>();
                title = "Title";
            }

            var i = new GameObject(title);
            i.transform.SetParent(obj);

            // Text
            if (!string.IsNullOrWhiteSpace(item.text))
            {
                var iText = i.AddComponent<TextMeshProUGUI>();
                iText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                if (CreditsSettings.Instance.font != null)
                { iText.font = CreditsSettings.Instance.font; }
                iText.fontSize = item.fontSize;
                iText.text = item.text;
            }

            // Image
            if (item.image != null)
            {
                var iLayout = i.AddComponent<VerticalLayoutGroup>();
                iLayout.childAlignment = TextAnchor.UpperCenter;
                iLayout.childForceExpandHeight = true;
                iLayout.childControlHeight = false;

                var iChild = new GameObject("Image");
                iChild.transform.SetParent(i.transform);

                var iImage = iChild.AddComponent<Image>();
                float height = (1000f / item.image.bounds.size.x) * item.image.bounds.size.y;
                iImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, height);
                iImage.sprite = item.image;
            }

            // Generate sub-items
            foreach (var sub in item.subItems)
            { GenerateCreditsItem(sub, obj); }
        }
    }
}
