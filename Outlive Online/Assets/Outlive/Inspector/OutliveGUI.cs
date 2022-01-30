using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Outlive;
using UnityEngine;
using UnityEditor;

public class OutliveGUI
{



    private static GUIStyle _removeItemButtonStyle;

    public static GUIStyle RemoveItemButtonStyle
    {
        get
        {
            if (_removeItemButtonStyle == null)
            {
                _removeItemButtonStyle = new GUIStyle(GUI.skin.button);
                _removeItemButtonStyle.fixedHeight = 22;
                _removeItemButtonStyle.fixedWidth = 34;
                _removeItemButtonStyle.fontStyle = FontStyle.Bold;
                _removeItemButtonStyle.normal.textColor = Color.black;
                _removeItemButtonStyle.active.textColor = Color.white;
            }

            return _removeItemButtonStyle;
        }
    }
    public static bool RemoveItemButton()
    {
        Rect controlRect = EditorGUILayout.GetControlRect(false, RemoveItemButtonStyle.fixedHeight);

        Rect removeButton = 
            new Rect(
                controlRect.xMax - RemoveItemButtonStyle.fixedWidth,
                controlRect.yMin,
                RemoveItemButtonStyle.fixedWidth,
                RemoveItemButtonStyle.fixedHeight
            );

        return GUI.Button(removeButton, "-", RemoveItemButtonStyle);
    }

    private static string[] imagesEncoded = 
    {
        "iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAIAAABLbSncAAAAMElEQVQIHWNgwAEYgeL///9Hk2VkZGSBCDU2NsLl6uvrgWwmOB+NQboETsvRTEZwASNNCQ0mcvo8AAAAAElFTkSuQmCC"
    };

    private static Vector2Int[] imagesEncodedSize = 
    {
        new Vector2Int(8, 8)
    };

    public static Texture2D GetImage (int index)
    {
        Vector2Int size = imagesEncodedSize[Math.Max(0, index)];
        Texture2D newTexture = new Texture2D(size.x, size.y);
        newTexture.LoadImage(Convert.FromBase64String(imagesEncoded[Math.Max(0, index)]));
        newTexture.filterMode = FilterMode.Point;
        // newTexture.

        return newTexture;
    }

    /// <summary>
    /// Draw texture using <see cref="GUIStyle"/> to workaround bug in Unity where
    /// <see cref="GUI.DrawTexture"/> flickers when embedded inside a property drawer.
    /// </summary>
    /// <param name="position">Position of which to draw texture in space of GUI.</param>
    /// <param name="texture">Texture.</param>
    public static void DrawTexture( Rect position, Texture2D texture )
    {
        if( Event.current.type != EventType.Repaint )
            return;

        GUIStyle s_TempStyle = new GUIStyle();

        s_TempStyle.normal.background = texture;

        s_TempStyle.Draw( position, GUIContent.none, false, false, false, false );
    }

    // public static bool PlayerProperty (SerializedProperty playerList, bool editable, bool foldout, Action<int> removeItem)
    // {
    //     GUILayoutOption[] options = { GUILayout.MinWidth(10.0f) };

    //     if (playerList == null)
    //     {
    //         return false;
    //     }

    //     float containerElementHeight = 22;
    //     float containerHeight = playerList.arraySize * containerElementHeight;



    //     EditorGUI.BeginDisabledGroup(!editable);
    //     Rect containerRect = Rect.zero;
    //     // Rect controlRect = PhotonGUI.ContainerBody(containerHeight);

    //     bool isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Jogadores", null, value => containerRect = value, null);

    //     if (!isOpen)
    //         containerHeight = 0;

    //     // GUI.DrawTexture
    //     // GUIStyle background = new GUIStyle();
    //     // background.normal.background = OutliveGUI.GetImage(0);
    //     // OutliveGUI.DrawTexture(originalRect, OutliveGUI.GetImage(0));
    //     // EditorGUILayout.
    //     if (isOpen)
    //         containerRect = PhotonGUI.ContainerBody(containerHeight);

    //     if(isOpen)
    //         for (int i = 0; i < playerList.arraySize; i++)
    //         {
    //             SerializedProperty itemProperty = playerList.GetArrayElementAtIndex(i);

    //             float collum = (containerRect.width - 32) / 5;
    //             Rect itemRect = new Rect(containerRect.xMin, containerRect.yMin + i * containerElementHeight, collum, containerElementHeight);

    //             if (itemProperty.objectReferenceValue is Outlive.Manager.Player player)
    //             {
    //                 EditorGUILayout.BeginHorizontal();

    //                 SerializedObject playerSerialized = new SerializedObject(player);

    //                 // Edição do nome do jogador
    //                 Rect nameItemRect = new Rect(containerRect.xMin + collum * 0.25f, containerRect.yMin + containerElementHeight * i, collum, containerElementHeight);
    //                 EditorGUI.LabelField(nameItemRect, "Nome: ");

    //                 nameItemRect.xMin += collum;
    //                 nameItemRect.xMax += collum;

    //                 string newName = EditorGUI.TextField(nameItemRect, player.displayName, GUI.skin.textField);
    //                 if (newName != player.displayName)
    //                 {
    //                     playerSerialized.FindProperty("_displayName").stringValue = newName;
    //                     playerSerialized.ApplyModifiedProperties();
    //                 }

    //                 // Edição da cor do jogador

    //                 Rect colorItemRect = new Rect(collum * 2.5f + containerRect.xMin, containerRect.yMin + containerElementHeight * i, collum, containerElementHeight);

    //                 EditorGUI.LabelField(colorItemRect, "Cor: ");


    //                 colorItemRect.xMin += collum;
    //                 colorItemRect.xMax += collum;

    //                 Color newColor = EditorGUI.ColorField(colorItemRect, player.color);
    //                 if (newColor != player.color)
    //                 {
    //                     playerSerialized.FindProperty("_color").colorValue = newColor;
    //                     playerSerialized.ApplyModifiedProperties();
    //                 }

    //                 Rect removeItemRect = new Rect(containerRect.xMin + collum * 5f, itemRect.yMin + 2f, 32, containerElementHeight - 4f);

    //                 if (GUI.Button(removeItemRect, "-", GUI.skin.button))
    //                 {
    //                     removeItem.Invoke(i);
    //                 }

    //                 EditorGUILayout.EndHorizontal();
    //             }
    //             else
    //             {
    //                 itemProperty.objectReferenceValue = new Outlive.Manager.Player(gameManager, "index " + i, Color.black);
    //             }
                
    //         }
                    

        
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
                
    //             // Debug.Log(listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).objectReferenceValue);
    //         }


            
    //     // this.serializedObject.ApplyModifiedProperties();
    //     EditorGUI.EndDisabledGroup();

    //     return isOpen;
    // }
}
