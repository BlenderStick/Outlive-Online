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

            if (list.hasMultipleDifferentValues)
            {
                EditorGUI.LabelField(position, "Player?");
                EditorGUILayout.HelpBox("Os jogadores não estão corretamente configurados, procure o GameObject que possue o PlayerInjector component ou crie um GameObject e adicione o PlayerInjector component nele e clique em Find All", MessageType.Error);
                return;
            }

            string[] strList = new string[list.arraySize];
            for (int i = 0; i < strList.Length; i++)
            {
                strList[i] = list.GetArrayElementAtIndex(i).stringValue;
            }

            int indexValue = index.intValue;
            if(index.hasMultipleDifferentValues)
                indexValue = 0;

            int newIndex = EditorGUI.Popup(position, "Player", indexValue, strList);
            if (newIndex != indexValue)
                index.intValue = newIndex;
        }
    }
}