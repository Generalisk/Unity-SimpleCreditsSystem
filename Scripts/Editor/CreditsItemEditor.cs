using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Generalisk.Credits.Editor
{
    [CustomPropertyDrawer(typeof(CreditsItem), true)]
    class CreditsItemEditor : PropertyDrawer
    {
        private Rect rect;
        private SerializedProperty property;

        private static KeyValuePair<float, string>[] FontSizeFormats { get; } =
        {
            new KeyValuePair<float, string>(96, "Title"),
            new KeyValuePair<float, string>(72, "Heading"),
            new KeyValuePair<float, string>(64, "Sub-heading"),
            new KeyValuePair<float, string>(48, "Large Text"),
            new KeyValuePair<float, string>(36, "Medium Text"),
            new KeyValuePair<float, string>(28, "Small Text"),
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            this.property = property;

            EditorGUI.BeginProperty(position, label, property);

            // Text
            var text = property.FindPropertyRelative("text");
            DrawProperty(text, CreditsItemStyles.text);

            if (!string.IsNullOrWhiteSpace(text.stringValue))
            {
                // Font Size
                var fontSizeRect = rect;
                fontSizeRect.width = EditorGUIUtility.labelWidth;

                EditorGUI.LabelField(fontSizeRect, CreditsItemStyles.fontSize);

                fontSizeRect.x += fontSizeRect.width + 2;
                fontSizeRect.width = rect.width - (fontSizeRect.width + 2);

                var fontSize = property.FindPropertyRelative("fontSize");
                if (EditorGUI.DropdownButton(fontSizeRect, GetFontSizeContent(), FocusType.Keyboard))
                {
                    var menu = new GenericMenu();

                    foreach (var format in FontSizeFormats)
                    {
                        var key = format.Key;
                        var isSize = fontSize.floatValue == key;
                        var title = new GUIContent(format.Value);
                        var output = new object[] { property, key };

                        menu.AddItem(title, isSize, SetFontSize, output);
                    }

                    menu.ShowAsContext();
                }

                rect.y += rect.height + 2;
            }

            // Image
            DrawProperty("image", CreditsItemStyles.image);

            // Sub-items
            DrawProperty("subItems", CreditsItemStyles.subItems);

            // End
            EditorGUI.EndProperty();
        }

        private void DrawProperty(string name, GUIContent content)
            => DrawProperty(property.FindPropertyRelative(name), content);

        private void DrawProperty(SerializedProperty property, GUIContent content)
        {
            // Calculate height & Y pos
            var height = EditorGUI.GetPropertyHeight(property);
            var rekt = rect;
            rekt.height = height;
            rect.y += height + 2;

            // Draw Property
            EditorGUI.PropertyField(rekt, property, content);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 42;

            // Text
            var text = property.FindPropertyRelative("text");
            if (!string.IsNullOrWhiteSpace(text.stringValue))
            { height += 20; }

            // Sub-items
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("subItems"));

            // Return final height
            return height;
        }

        private void SetFontSize(object value)
        {
            var array = (object[])value;
            var property = (SerializedProperty)array[0];
            var key = (float)array[1];

            property.FindPropertyRelative("fontSize").floatValue = key;
        }

        private GUIContent GetFontSizeContent()
        {
            var fontSize = property.FindPropertyRelative("fontSize");

            foreach (var format in FontSizeFormats)
            {
                if (fontSize.floatValue == format.Key)
                { return new GUIContent(format.Value); }
            }

            return new GUIContent("Custom");
        }
    }
}
