using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Outlive.Manager.Generic;
using Outlive.Unit;
using UnityEngine.Events;
using System.Security;

namespace Outlive.Manager.Editor
{
    [CustomEditor(typeof(Outlive.Manager.GameManager))]
    public class GameManagerInspector : UnityEditor.Editor {


        static GUIContent _guiAutoStart = new GUIContent("Auto Start Game", "Inicia os players junto com o Awake.\nDefina como false para cenas que precisam ser pre configuradas em tempo de execução.");
        static GUIContent _guiManualOrRef = new GUIContent("Modo de criação de jogadores");
        static GUIContent _guiPlayers = new GUIContent("Players");

        private GameManager gameManager;
        ///<summary> Usado para acionar os eventos de UnitStarter </summary>
        private bool isChanged;
        private SerializedProperty mode, autoStart, defaultPlayer, players;

        private bool _events_foldout;
        private SerializedProperty[] _events;
        private SerializedProperty playerIndefinido;

        public void OnEnable() {
            gameManager = (GameManager) this.target;
            mode = serializedObject.FindProperty("_playerMode");
            autoStart = serializedObject.FindProperty("_autoStartGame");
            defaultPlayer = serializedObject.FindProperty("_createDefaultPlayers");

            if (gameManager._playerMode == GameManager.PlayerMode.Reference)
                players = serializedObject.FindProperty("_Object_Player");
            else
                players = serializedObject.FindProperty("_players");

            playerIndefinido = serializedObject.FindProperty("_playerIndefinido");

            _events = new SerializedProperty[]
            {
                serializedObject.FindProperty("_onPlayerListChange"),
                serializedObject.FindProperty("_onGameManagerStart"),
            };

            // SerializedProperty m_CreatedPlayers = serializedObject.FindProperty("CreatedPlayers");
            // for (int i = 0; i < m_CreatedPlayers.arraySize; i++)
            // {
            //     _tempCreatedPlayers.Add((IPlayer) m_CreatedPlayers.GetArrayElementAtIndex(i).objectReferenceValue);
            // }
        }
        public override void OnInspectorGUI() {

            bool fireEvents = false;

            EditorGUILayout.PropertyField(autoStart, _guiAutoStart);
            EditorGUILayout.PropertyField(defaultPlayer);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(mode, _guiManualOrRef);
            EditorGUILayout.PropertyField(players, _guiPlayers);
            EditorGUILayout.PropertyField(playerIndefinido);

            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < players.arraySize; i++)
                {
                    players.GetArrayElementAtIndex(i).FindPropertyRelative("_inspectorGameManager").objectReferenceValue = gameManager;
                }
                fireEvents = true;
            }


            _events_foldout = EditorGUILayout.BeginFoldoutHeaderGroup(_events_foldout, "Events");

            if (_events_foldout)
                foreach (var item in _events)
                    EditorGUILayout.PropertyField(item);

            EditorGUILayout.EndFoldoutHeaderGroup();
                
            // if (gameManager._playerMode == GameManager.PlayerMode.Reference)
            //     if (players != gameManager._Object_Player)
            //         isChanged = true;
            


            serializedObject.ApplyModifiedProperties();
            if (fireEvents)
                gameManager.FirePlayerListChange();
            // if (GUI.Button(new Rect()))
        }
    }
}

