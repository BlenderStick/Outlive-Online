using System;
using UnityEngine;
using UnityEditor;

namespace Outlive.Unit.Inspector
{
    
    [CustomEditor(typeof(UnitGeneric), true)]
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

                if (baseData.name == "_player")
                    EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(baseData);
                if (baseData.name == "_player")
                    firePlayerChange = EditorGUI.EndChangeCheck();
                
            }while(baseData.NextVisible(false));
            

            serializedObject.ApplyModifiedProperties();
            
            if (firePlayerChange)
            {
                (target as UnitGeneric).ForceInjectorUpdate();
            }
        }
    }
}