using System.Collections;
using System.Collections.Generic;
using Outlive.Controller.Generic;
using Outlive.Manager;
using UnityEditor;
using UnityEngine;

namespace Outlive.Controller.Inspector
{

    // [CustomEditor(typeof(PlayerEventListener))]
    public class PlayerEventListenerInspector : Editor {

        private static GUIContent playerControllerGUI = new GUIContent("PlayerController");
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            SerializedProperty playerController = serializedObject.FindProperty("_playerControllerRef");

            Object obj = EditorGUILayout.ObjectField(playerControllerGUI, playerController.objectReferenceValue, typeof(Object), true);
            if (obj != playerController.objectReferenceValue)
            {
                if (obj is IPlayerController)
                    playerController.objectReferenceValue = obj;
                else if (obj is GameObject o)
                {
                    Component comp = o.GetComponent(typeof(IPlayerController));
                    if (comp != null)
                        playerController.objectReferenceValue = comp;    
                }
                else if (obj == null)
                    playerController.objectReferenceValue = null;
            }

            serializedObject.ApplyModifiedProperties();


        }
    }
}
