using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{

    public static MultiplayerManager Instance {get; private set;}
    private void Awake() {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SairDoLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        // var converted = GetComponent<Image>();
        // byte[] bits = converted.sprite.texture.EncodeToPNG();
        // Debug.Log(Convert.ToBase64String(bits));

        // converted.sprite
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado");
    }

    public void CriaSala(string nome)
    {
        PhotonNetwork.CreateRoom(nome);
    }

    public void EntraSala(string nome)
    {
        PhotonNetwork.JoinRoom(nome);
    }

    internal static void ComecaJogo(string nomeCena)
    {
        PhotonNetwork.LoadLevel(nomeCena);
        // PhotonNetwork.room
    }

    public void MudaNick(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    public string ObterListaDeJogadores()
    {
        string lista = "";
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            lista += p.NickName + "\n";
        }
        return lista;
    }

    public bool donoDaSala
    {
        get => PhotonNetwork.IsMasterClient;
    }
}
