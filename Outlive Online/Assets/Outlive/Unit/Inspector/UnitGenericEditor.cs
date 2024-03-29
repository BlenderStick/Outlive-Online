using System;
using UnityEngine;
using UnityEditor;

namespace Outlive.Unit.Inspector
{
    
    // [CustomEditor(typeof(UnitGeneric), true)]
    [CanEditMultipleObjects]
    public class UnitGenericEditor : Editor {
        public override void OnInspectorGUI() {

            SerializedProperty baseData = serializedObject.GetIterator();
            baseData.Next(true);

            bool firePlayerChange = false;
            
            int count = 0;
            do
            {
                if (count++ < 2)
                    continue;

                bool isPlayerOrInjector = baseData.name == "_player";

                if (isPlayerOrInjector)
                    EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(baseData);
                
                if (isPlayerOrInjector)
                    firePlayerChange = EditorGUI.EndChangeCheck();
                
            }while(baseData.NextVisible(false));


            serializedObject.ApplyModifiedProperties();
            
        }
    }
}