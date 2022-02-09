using System;
using UnityEngine;
using UnityEditor;

namespace Outlive.Unit.Inspector
{
    
    [CustomEditor(typeof(UnitGeneric))]
    public class UnitGenericEditor : Editor {

        SerializedProperty _playersList;
        SerializedProperty _playerIndex;

        private void OnEnable() 
        {
            _playersList = serializedObject.FindProperty("_playersList");
            _playerIndex = serializedObject.FindProperty("_playerIndex");
        }
        public override void OnInspectorGUI() {

            SerializedProperty baseData = serializedObject.GetIterator();
            baseData.Next(true);
            
            int count = 0;
            do
            {
                if (count++ < 2)
                    continue;
                if (baseData.name == "_playerIndex")
                {
                    DrawPlayer();
                }
                else
                {
                    EditorGUILayout.PropertyField(baseData);
                }
            }while(baseData.NextVisible(false));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPlayer()
        {
            string[] list = new string[_playersList.arraySize];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = _playersList.GetArrayElementAtIndex(i).stringValue;
            }
            int newPlayerIndex = EditorGUILayout.Popup("Player", _playerIndex.intValue, list);
            if (newPlayerIndex != _playerIndex.intValue)
            {
                _playerIndex.intValue = newPlayerIndex;
                serializedObject.ApplyModifiedProperties();
                UnitGeneric unit = target as UnitGeneric;
                unit.ForceInjectorUpdate(newPlayerIndex);
            }

        }
    }
}