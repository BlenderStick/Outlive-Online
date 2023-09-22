using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller.Inspector
{
    using UnityEngine;
    using UnityEditor;
    using Outlive.Manager;

    [CustomPropertyDrawer(typeof(PlayerSelect))]
    public class PlayerSelectDrawer: PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {

            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty list = property.FindPropertyRelative("_playerList");
            SerializedProperty index = property.FindPropertyRelative("_playerIndex");

            if (list.hasMultipleDifferentValues)
            {
                EditorGUI.LabelField(position, "Player?");
                EditorGUILayout.HelpBox("Os jogadores não estão corretamente configurados, procure o GameObject que possue o PlayerInjector component ou crie um GameObject e adicione o PlayerInjector component nele e clique em Find All", MessageType.Error);
                return;
            }

            string[] strList = new string[list.arraySize + 1];
            strList[0] = "Indefinido";
            for (int i = 1; i < strList.Length; i++)
            {
                strList[i] = list.GetArrayElementAtIndex(i - 1).stringValue;
            }

            int indexValue = index.intValue + 1;
            if(index.hasMultipleDifferentValues)
                indexValue = 0;

            int newIndex = EditorGUI.Popup(position, "Player", indexValue, strList);
            if (newIndex != indexValue)
                index.intValue = newIndex - 1;
                
            EditorGUI.EndProperty();
        }
    }
}