﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Outlive.Map;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Outlive.Manager.Inspector;

namespace Outlive.Map.Inspector
{
    [CustomPropertyDrawer(typeof(MapPlayerSettings))]
    public class IMapSettingsInspector : PlayerInspector
    {
        
    }
    // public class IMapSettingsInspector: PropertyDrawer 
    // {

    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         EditorGUI.BeginProperty(position, label, property);

    //         position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    //         float midWidth = position.width / 2f;

    //         Rect nameLabelRect = new Rect(position.x, position.y, midWidth * 0.3f, position.height);
    //         Rect nameFieldRect = new Rect(position.x + midWidth * 0.3f, position.y, midWidth * 0.7f, position.height);
    //         Rect colorRect = new Rect(position.x + midWidth, position.y, midWidth, position.height);
    //         EditorGUI.LabelField(nameLabelRect, "Nome");
    //         EditorGUI.PropertyField(nameFieldRect, property.FindPropertyRelative("_displayName"), GUIContent.none);
    //         EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("_color"), GUIContent.none);


    //         EditorGUI.EndProperty();
    //     }
    // }
}
