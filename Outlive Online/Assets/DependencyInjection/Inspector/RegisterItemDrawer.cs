using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DependencyInjection.Inspector
{
    
    [CustomPropertyDrawer(typeof(RegisterItem))]
    public class RegisterItemDrawer: PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            
            SerializedProperty _register = property.FindPropertyRelative("_register");

            Object _registerObject = EditorGUILayout.ObjectField(_register.objectReferenceValue, typeof(Component), true);
            if (_registerObject is DependencyRegister)
                _register.objectReferenceValue = _registerObject;
            else if (_registerObject == null)
                _register.objectReferenceValue = null;
        }
    }
}