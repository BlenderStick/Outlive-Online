using UnityEngine;
using UnityEditor;

namespace Outlive.Manager.Inspector
{
    
    [CustomEditor(typeof(PlayerInjector))]
    public class PlayerInjectorEditor : UnityEditor.Editor {

        private SerializedProperty _objectsToInjectPlayer;
        private SerializedProperty _manager;

        private void OnEnable() 
        {
            _objectsToInjectPlayer = serializedObject.FindProperty("_objectsToInjectPlayer");
            _manager = serializedObject.FindProperty("_manager");
        }
        public override void OnInspectorGUI() {

            EditorGUILayout.PropertyField(_manager);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_objectsToInjectPlayer);
            // Rect _buttonRect = EditorGUI.RectField(Rect.zero, Rect.zero);

            // EditorGUILayout.

            if (GUILayout.Button("Find All"))
            {
                (target as PlayerInjector).FindAll();
            }

            serializedObject.ApplyModifiedProperties();
            
        }

        // protected override void OnHeaderGUI()
        // {

        // }
    }
}