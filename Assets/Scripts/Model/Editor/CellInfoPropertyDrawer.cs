using UnityEditor;
using UnityEngine;

namespace Lesstergy.Chess2D {

    [CustomPropertyDrawer(typeof(CellInfo))]
    public class CellInfoPropertyDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            float fullWidth = contentPosition.width;

            contentPosition.width = fullWidth * 0.6f;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 40f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("coord"), new GUIContent("Cell"));

            contentPosition.x += contentPosition.width;
            contentPosition.width = fullWidth * 0.4f;
            EditorGUIUtility.labelWidth = 50f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("pieceType"), new GUIContent("Piece"));
            EditorGUI.EndProperty();
        }
    }

}
