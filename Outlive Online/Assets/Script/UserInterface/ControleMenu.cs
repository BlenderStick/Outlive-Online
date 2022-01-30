using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControleMenu : MonoBehaviourPunCallbacks
{

    [SerializeField] private MenuEntrada _menuEntrada;
    [SerializeField] private MenuLobby _menuLobby;

    // Start is called before the first frame update
    void Start()
    {
        _menuEntrada.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        _menuEntrada.gameObject.SetActive(true);
    }

    public void MudaMenu(GameObject menu)
    {
        _menuEntrada.gameObject.SetActive(false);
        _menuLobby.gameObject.SetActive(false);

        menu.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        MudaMenu(_menuLobby.gameObject);
        _menuLobby.photonView.RPC("AtualizaLista", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        _menuLobby.AtualizaLista();
    }

    
    public void SairDoLobby()
    {
        MultiplayerManager.Instance.SairDoLobby();
        MudaMenu(_menuEntrada.gameObject);
    }

    public void ComecaJogo(string nomeCena)
    {
        MultiplayerManager.Instance.photonView.RPC("ComecaJogo", RpcTarget.All, nomeCena);
    }

}
