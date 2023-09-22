using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Outlive.Manager;

[CustomEditor(typeof(PlayerEditorSet), true)]
[CanEditMultipleObjects]
public class PlayerEditorSetEditor : Editor
{
    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        SerializedProperty baseData = serializedObject.GetIterator();
        baseData.Next(true);
        bool fireManager = false;

        while(baseData.NextVisible(false))
        {
            bool isPlayerSet = baseData.name == "_playerSelect";


            if (isPlayerSet)
                EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(baseData);

            if (isPlayerSet)
            {
                EditorGUI.EndChangeCheck();
                fireManager = true;
            }

        }

        serializedObject.ApplyModifiedProperties();

        if (fireManager)
        {
            GameManager gameManager = Object.FindAnyObjectByType<GameManager>();
            if (gameManager == null)
                return;
            
            gameManager.FirePlayerListChange();
        }
    }
}
