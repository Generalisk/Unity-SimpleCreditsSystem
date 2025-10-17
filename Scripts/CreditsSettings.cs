using System.Linq;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if USE_AUDIO_RESOURCE
using UnityEngine.Audio;
#endif

namespace Generalisk.Credits
{
    public class CreditsSettings : ScriptableObject
    {
#if USE_AUDIO_RESOURCE
        public AudioResource music = null;
#else
        public AudioClip music = null;
#endif
        public TMP_FontAsset font = null;
        public CreditsItem[] items = { };

        public static CreditsSettings Instance { get; private set; } = null;

#if UNITY_EDITOR
        internal const string ID = CreditsSystem.PACKAGE_ID;
        internal const string DEFAULT_PATH = "Packages/" + CreditsSystem.PACKAGE_ID + "/Settings.asset";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            CreditsSettings dictionary = null;
            if (!EditorBuildSettings.TryGetConfigObject(ID, out dictionary))
            {
                dictionary = CreateInstance<CreditsSettings>();

                AssetDatabase.CreateAsset(dictionary, DEFAULT_PATH);
                EditorBuildSettings.AddConfigObject(ID, dictionary, true);
            }

            var preload = PlayerSettings.GetPreloadedAssets().ToList();
            if (!preload.Contains(dictionary))
            {
                preload.Add(dictionary);
                PlayerSettings.SetPreloadedAssets(preload.ToArray());
            }
        }
#endif

        void OnEnable()
        {
            if (Instance == null)
            { Instance = this; }
        }
    }
}
