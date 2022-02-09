using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using UnityEditor;

namespace DependencyInjection.Inspector
{
    
    [CustomEditor(typeof(DependencyInjection))]
    public class DependencyInjectionEditor : Editor {

        SerializedProperty _registers;

        private void OnEnable() 
        {
            _registers = serializedObject.FindProperty("_object_registers");
        }
        public override void OnInspectorGUI() 
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_registers);

            if (EditorGUI.EndChangeCheck() || GUILayout.Button("Force dependencies", EditorStyles.miniButtonLeft, new GUILayoutOption[] {GUILayout.Width(150f)}))
            {
                DependencyInjection register = target as DependencyInjection;
                register.ForceRegisterAllInEdit(true);

            }
        }
    }
}