using UnityEditor;
using UnityEngine;

namespace Generalisk.Credits.Editor
{
    [CustomEditor(typeof(CreditsSettings))]
    class CreditsSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw Project Setting Button
            if (GUILayout.Button(new GUIContent("Open in Project Settings")))
            { SettingsService.OpenProjectSettings(ProjectSettings.PATH); }

            // Draw Project Settings
            EditorGUILayout.Space(15);
            SettingProperties.Draw(serializedObject);

            // Save Modified Properties
            serializedObject.ApplyModifiedProperties();
        }
    }
}
