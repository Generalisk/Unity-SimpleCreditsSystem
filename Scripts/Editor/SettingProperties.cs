using UnityEditor;
using UnityEngine;

namespace Generalisk.Credits.Editor
{
    internal static class SettingProperties
    {
        public static void Draw(SerializedObject obj)
        {
            // Music
            DrawProperty("music", SettingStyles.music, obj);

            // Font
            DrawProperty("font", SettingStyles.font, obj);

            // Contents
            EditorGUILayout.Space(15);
            DrawProperty("items", SettingStyles.contents, obj);

            // Save Modified Properties
            obj.ApplyModifiedProperties();
        }

        private static void DrawProperty(string property, GUIContent content, SerializedObject obj)
            => DrawProperty(obj.FindProperty(property), content);

        private static void DrawProperty(SerializedProperty property, GUIContent content)
            => EditorGUILayout.PropertyField(property, content);
    }
}
