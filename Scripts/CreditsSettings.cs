using System.IO;
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

        /// <summary>
        /// (Runtime only! Use the Get() function for editor scripts)
        /// </summary>
        public static CreditsSettings Instance { get; private set; } = null;

#if UNITY_EDITOR
        internal const string ID = CreditsSystem.PACKAGE_ID;
        internal const string DEFAULT_PATH = "Assets/Settings/Credits.asset";

        /// <summary>
        /// Gets the settings instance, or generates a new one if it could not be found
        /// 
        /// (Editor only! Use the Instance property for runtime scripts)
        /// </summary>
        /// <returns>The settings instance</returns>
        [InitializeOnLoadMethod]
        public static CreditsSettings Get()
        {
            CreditsSettings settings = null;
            if (!EditorBuildSettings.TryGetConfigObject(ID, out settings))
            {
                if (AssetDatabase.AssetPathExists(DEFAULT_PATH) &&
                    AssetDatabase.GetMainAssetTypeAtPath(DEFAULT_PATH) == typeof(CreditsSettings))
                {
                    settings = AssetDatabase.LoadAssetAtPath<CreditsSettings>(DEFAULT_PATH);
                }
                else
                {
                    settings = CreateInstance<CreditsSettings>();

                    if (!Directory.Exists(DEFAULT_PATH + "/../"))
                    { Directory.CreateDirectory(DEFAULT_PATH + "/../"); }

                    AssetDatabase.CreateAsset(settings, DEFAULT_PATH);
                }
                EditorBuildSettings.AddConfigObject(ID, settings, true);
            }

            var preload = PlayerSettings.GetPreloadedAssets().ToList();
            if (!preload.Contains(settings))
            {
                preload.Add(settings);
                PlayerSettings.SetPreloadedAssets(preload.ToArray());
            }

            return settings;
        }
#endif

        void OnEnable()
        {
            if (Instance == null)
            { Instance = this; }
        }
    }
}
