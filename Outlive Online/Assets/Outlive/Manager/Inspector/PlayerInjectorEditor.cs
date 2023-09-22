using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

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

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear"))
            {
                (target as PlayerInjector).ClearInjectables();
            }
            
            if (GUILayout.Button("Find All"))
            {
                (target as PlayerInjector).FindAll();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
            
        }

        // protected override void OnHeaderGUI()
        // {

        // }
    }
}