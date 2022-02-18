using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEditor;

namespace Outlive.Controller.Inspector
{
    
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : Editor 
    {
        SerializedProperty[] _propertys;
        SerializedProperty[] _events;
        private bool _eventsFoldout;

        private void OnEnable() 
        {
            _propertys = new SerializedProperty[]
            {
                serializedObject.FindProperty("_input"),
                serializedObject.FindProperty("_mainCamera"),
                serializedObject.FindProperty("_player"),
                serializedObject.FindProperty("_layerSelectable"),
            };
            _events = new SerializedProperty[]
            {
                serializedObject.FindProperty("_onFocusChange"),
                serializedObject.FindProperty("_onSelect"),
                serializedObject.FindProperty("_onCancelSelect"),
                serializedObject.FindProperty("_onInteract"),
                serializedObject.FindProperty("_onMultiselectChange"),
                serializedObject.FindProperty("_onEnableInputsChange"),
                serializedObject.FindProperty("_onCameraMove"),
                serializedObject.FindProperty("_onCameraChange"),
                serializedObject.FindProperty("_onSelectionChange"),
            };
            
        }
        public override void OnInspectorGUI() 
        {
            foreach (var item in _propertys)
            {
                EditorGUILayout.PropertyField(item);
            }

            EditorGUILayout.Separator();

            if (_eventsFoldout = EditorGUILayout.Foldout(_eventsFoldout, "Eventos", EditorStyles.foldout))

            foreach (var item in _events)
            {
                EditorGUILayout.PropertyField(item);
            }
            
            // EditorGUILayout.EndFoldoutHeaderGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}