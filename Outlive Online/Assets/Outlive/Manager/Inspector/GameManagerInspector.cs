using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Outlive.Manager.Generic;
using Outlive.Unit;

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

        public void OnEnable() {
            gameManager = (GameManager) this.target;

            // SerializedProperty m_CreatedPlayers = serializedObject.FindProperty("CreatedPlayers");
            // for (int i = 0; i < m_CreatedPlayers.arraySize; i++)
            // {
            //     _tempCreatedPlayers.Add((IPlayer) m_CreatedPlayers.GetArrayElementAtIndex(i).objectReferenceValue);
            // }
        }
        public override void OnInspectorGUI() {


            SerializedProperty mode = serializedObject.FindProperty("_playerMode");
            SerializedProperty autoStart = serializedObject.FindProperty("_autoStartGame");
            

            SerializedProperty players;
            if (gameManager._playerMode == GameManager.PlayerMode.Reference)
            {
                players = serializedObject.FindProperty("_Object_Player");
                // GUIPlayerList("", false, false);
            }
            else
            {
                players = serializedObject.FindProperty("_players");
                // GUIPlayerList("CreatedPlayers", false, !Application.isPlaying);
            }

            EditorGUILayout.PropertyField(autoStart, _guiAutoStart);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(mode, _guiManualOrRef);
            EditorGUILayout.PropertyField(players, _guiPlayers);

            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < players.arraySize; i++)
                {
                    players.GetArrayElementAtIndex(i).FindPropertyRelative("_inspectorGameManager").objectReferenceValue = gameManager;
                }
                NotifyUnitStarter();
            }
                
            // if (gameManager._playerMode == GameManager.PlayerMode.Reference)
            //     if (players != gameManager._Object_Player)
            //         isChanged = true;
            


            serializedObject.ApplyModifiedProperties();
            // if (GUI.Button(new Rect()))
        }

        // public void GUIPlayerList(string pathList, bool reference, bool editable)
        // {
        //     GameManager gameManager = (GameManager) this.target;
        //     GUILayoutOption[] options = { GUILayout.MinWidth(10.0f) };

        //     SerializedProperty listProperty = this.serializedObject.FindProperty(pathList);
            

        //     if (listProperty == null)
        //     {
        //         Debug.Log("Nulo");
        //         return;
        //     }

        //     float containerElementHeight = 22;
        //     float containerHeight = listProperty.arraySize * containerElementHeight;



        //     EditorGUI.BeginDisabledGroup(!editable);
        //     Rect containerRect = Rect.zero;
        //     // Rect controlRect = PhotonGUI.ContainerBody(containerHeight);

        //     SerializedProperty isFoldout = this.serializedObject.FindProperty("isFoldout");
        //     bool isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(isFoldout.boolValue, "Jogadores", null, value => containerRect = value, null);

        //     if (!isOpen)
        //         containerHeight = 0;

        //     isFoldout.boolValue = isOpen;
        //     serializedObject.ApplyModifiedProperties();

        //     // GUI.DrawTexture
        //     // GUIStyle background = new GUIStyle();
        //     // background.normal.background = OutliveGUI.GetImage(0);
        //     // OutliveGUI.DrawTexture(originalRect, OutliveGUI.GetImage(0));
        //     // EditorGUILayout.
        //     if (isOpen)
        //         containerRect = PhotonGUI.ContainerBody(containerHeight);

        //     if (reference)
        //     {

        //     }
        //     else
        //     {
        //         serializedObject.Update ();
        //         if(isOpen)
        //             for (int i = 0; i < listProperty.arraySize; i++)
        //             {
        //                 SerializedProperty itemProperty = listProperty.GetArrayElementAtIndex(i);

        //                 float collum = (containerRect.width - 32) / 5;
        //                 Rect itemRect = new Rect(containerRect.xMin, containerRect.yMin + i * containerElementHeight, collum, containerElementHeight);

        //                 if (itemProperty.objectReferenceValue is Player player)
        //                 {
        //                     EditorGUILayout.BeginHorizontal();

        //                     SerializedObject playerSerialized = new SerializedObject(player);

        //                     // Edição do nome do jogador
        //                     Rect nameItemRect = new Rect(containerRect.xMin + collum * 0.25f, containerRect.yMin + containerElementHeight * i, collum, containerElementHeight);
        //                     EditorGUI.LabelField(nameItemRect, "Nome: ");

        //                     nameItemRect.xMin += collum;
        //                     nameItemRect.xMax += collum;

        //                     string newName = EditorGUI.TextField(nameItemRect, player.displayName, GUI.skin.textField);
        //                     if (newName != player.displayName)
        //                     {
        //                         Undo.RecordObject(gameManager, "Desfazer mudança no nome do jogador");
        //                         playerSerialized.FindProperty("_displayName").stringValue = newName;
        //                         playerSerialized.ApplyModifiedProperties();
        //                     }

        //                     // Edição da cor do jogador

        //                     Rect colorItemRect = new Rect(collum * 2.5f + containerRect.xMin, containerRect.yMin + containerElementHeight * i, collum, containerElementHeight);

        //                     EditorGUI.LabelField(colorItemRect, "Cor: ");


        //                     colorItemRect.xMin += collum;
        //                     colorItemRect.xMax += collum;

        //                     Color newColor = EditorGUI.ColorField(colorItemRect, player.color);
        //                     if (newColor != player.color)
        //                     {
        //                         Undo.RecordObject(gameManager, "Desfazer mudança na cor do jogador");
        //                         playerSerialized.FindProperty("_color").colorValue = newColor;
        //                         playerSerialized.ApplyModifiedProperties();
        //                         isChanged = true;
        //                     }

        //                     Rect removeItemRect = new Rect(containerRect.xMin + collum * 5f, itemRect.yMin + 2f, 32, containerElementHeight - 4f);

        //                     if (GUI.Button(removeItemRect, "-", GUI.skin.button))
        //                     {
        //                         gameManager.RemoveItem(i);
        //                         this.serializedObject.ApplyModifiedProperties();
        //                         isChanged = true;
        //                     }

        //                     EditorGUILayout.EndHorizontal();
        //                 }
        //                 else
        //                 {
        //                     itemProperty.objectReferenceValue = new Player(gameManager, "index " + i, Color.black);
        //                 }
                        
        //             }
                        

        //     }
        //     EditorGUILayout.EndFoldoutHeaderGroup();

        //     if (editable && isOpen)
        //         if (PhotonGUI.AddButton())
        //         {
        //             listProperty.InsertArrayElementAtIndex(Mathf.Max(0, listProperty.arraySize - 1));
        //             // this.serializedObject.ApplyModifiedProperties();


        //             listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).objectReferenceValue = CreateInstance<Player>();//= new Player(gameManager, "Novo Jogador", Color.blue);
        //             this.serializedObject.ApplyModifiedProperties();

        //             SerializedObject newPlayerSerialized = new SerializedObject(listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).objectReferenceValue);
        //             newPlayerSerialized.FindProperty("_inspectorGameManager").objectReferenceValue = gameManager;
        //             newPlayerSerialized.ApplyModifiedProperties();
        //             isChanged = true;
        //             // Debug.Log(listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).objectReferenceValue);
        //         }


                
        //     // this.serializedObject.ApplyModifiedProperties();
        //     EditorGUI.EndDisabledGroup();



        //     // EditorGUILayout.BeginVertical();
        //     // EditorGUILayout.BeginHorizontal();
        //     // EditorGUILayout.LabelField("Label");
        //     // EditorGUILayout.LabelField("Label");
        //     // // EditorGUILayout.BeginHorizontal();
        //     // EditorGUILayout.LabelField("Label");
        //     // EditorGUILayout.LabelField("Label");
        //     // // EditorGUILayout.EndHorizontal();
        //     // EditorGUILayout.EndHorizontal();
        //     // EditorGUILayout.EndVertical();
        // }

        private void NotifyUnitStarter()
        {
            UnityEngine.Object[] objetos = FindObjectsOfType(typeof(GameObject));
            foreach (var item in objetos)
            {
                GameObject obj = (GameObject) item;
                UnitStarter unitStarter;
                if (obj.TryGetComponent<UnitStarter>(out unitStarter))
                {
                    if ((System.Object) unitStarter.gameManager == target)
                        unitStarter.SetPlayerWithGameManager(unitStarter.gameManager);
                }
            }
        }
    }
}

