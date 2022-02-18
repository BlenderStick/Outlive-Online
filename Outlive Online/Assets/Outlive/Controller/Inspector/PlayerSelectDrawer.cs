using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller.Inspector
{
    using UnityEngine;
    using UnityEditor;
    
    [CustomPropertyDrawer(typeof(PlayerSelect))]
    public class PlayerSelectDrawer: PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            SerializedProperty list = property.FindPropertyRelative("_playerList");
            SerializedProperty index = property.FindPropertyRelative("_playerIndex");
            string[] strList = new string[list.arraySize];
            for (int i = 0; i < strList.Length; i++)
            {
                strList[i] = list.GetArrayElementAtIndex(i).stringValue;
            }

            int newIndex = EditorGUI.Popup(position, "Player", index.intValue, strList);
            if (newIndex != index.intValue)
                index.intValue = newIndex;
        }
    }
}