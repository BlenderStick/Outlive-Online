using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEditor;
using UnityEngine;

namespace Outlive.Unit.Inspector
{
    [CanEditMultipleObjects]
    [ObsoleteAttribute]
    [CustomEditor(typeof(UnitStarter))]
    public class UnitStarterInspector : Editor
    {
        private static GUIContent modeGUI = new GUIContent("Modo", "Define como o player será procurado");
        private static GUIContent player = new GUIContent("Player");
        private static GUIContent gameManagerGUI = new GUIContent("GameManager", "GameObject que contém um IGameManager");
        public override void OnInspectorGUI()
        {
            SerializedProperty gameManager = serializedObject.FindProperty("_gameManagerRef");
            IGameManager gameManagerValue = gameManager.objectReferenceValue != null? (IGameManager) gameManager.objectReferenceValue: null;

            bool isPropChanges = false;


            SerializedProperty mode = serializedObject.FindProperty("_mode");
            int modeValue = mode.intValue;
            EditorGUILayout.PropertyField(mode, modeGUI);

            if (modeValue != mode.enumValueIndex)
                isPropChanges = true;

            if ((mode.enumValueIndex == (int) UnitStarter.PlayerSelectionMode.INDEX))
            {
                SerializedProperty playerIndex = serializedObject.FindProperty("_playerIndex");
                int playerIndexValue = playerIndex.intValue;

                int value = EditorGUILayout.IntField(player, playerIndexValue);
                if (value < 0)
                    value = 0;

                if (value != playerIndexValue)
                {
                    playerIndex.intValue = value;
                    isPropChanges = true;
                }

            }
            else
            {
                SerializedProperty playerRef = serializedObject.FindProperty("_playerReference");
                UnityEngine.Object playerRefValue = playerRef.objectReferenceValue;

                UnityEngine.Object obj = EditorGUILayout.ObjectField(player, playerRef.objectReferenceValue, typeof(UnityEngine.Object), true);
                if (obj != playerRef.objectReferenceValue)
                {
                    if (obj is IPlayer)
                        playerRef.objectReferenceValue = obj;
                    else if (obj is GameObject o)
                    {
                        Component component = o.GetComponent(typeof(IPlayer));
                        if (component != null)
                            playerRef.objectReferenceValue = component;
                    }
                    else
                        Debug.LogError("Object precisa extender de IPlayer ou possuir um IPlayer Component");
                        
                    // if (obj is IPlayer)
                    isPropChanges = true;
                }
            }
            EditorGUILayout.Separator();

            UnityEngine.Object manager = EditorGUILayout.ObjectField(gameManagerGUI, gameManager.objectReferenceValue, typeof(UnityEngine.Object), true);

            if (manager != gameManager.objectReferenceValue)
            {
                if (manager is GameObject i)
                {
                    Component component = i.GetComponent(typeof(IGameManager));
                    if (component != null)
                    {
                        gameManager.objectReferenceValue = component;
                    }
                    isPropChanges = true;
                }
                else if (manager is IGameManager)
                {
                    gameManager.objectReferenceValue = manager;
                    isPropChanges = true;
                }
                else if (manager == null)
                    gameManager.objectReferenceValue = null;
                
            }

            EditorGUILayout.Separator();
            SerializedProperty onPlayerLoad = serializedObject.FindProperty("_onPlayerLoad");
            EditorGUILayout.PropertyField(onPlayerLoad);
            
            serializedObject.ApplyModifiedProperties();
            if (isPropChanges)
                changed(gameManagerValue);
        }

        private void changed(IGameManager gameManager)
        {
            if (gameManager == null)
                return;

            UnitStarter unitStarter = (UnitStarter) this.target;
            unitStarter.SetPlayerWithGameManager(gameManager);
        }

    }
}