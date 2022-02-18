using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Outlive.GUI.Generic;

namespace Outlive.GUI.Inspector
{
    
    // [CustomEditor(typeof(GUIManager))]
    public class GUIManagerInspector : Editor {

        static GUIContent orderGUI = new GUIContent("Order");
        static GUIContent uiTransformGUI = new GUIContent("UI Root");
        public override void OnInspectorGUI() 
        {
            SerializedProperty order = serializedObject.FindProperty("_object_order");

            Object orderValue = EditorGUILayout.ObjectField(orderGUI, order.objectReferenceValue, typeof(Object), true);
            if (orderValue != order.objectReferenceValue)
            {
                if (orderValue is GameObject o)
                    order.objectReferenceValue = o.GetComponent(typeof(IGUILoaderOrder));
                else if (orderValue == null || orderValue is IGUILoaderOrder)
                    order.objectReferenceValue = orderValue;
            }

            SerializedProperty uiTransform = serializedObject.FindProperty("_uiTransform");
            EditorGUILayout.PropertyField(uiTransform, uiTransformGUI);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}