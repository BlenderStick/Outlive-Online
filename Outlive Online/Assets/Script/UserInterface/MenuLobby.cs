using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MenuLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _listaDeJogadores;
    [SerializeField] private Button _comecaJogo;

    [PunRPC]
    public void AtualizaLista()
    {
        _listaDeJogadores.text = MultiplayerManager.Instance.ObterListaDeJogadores();
        _comecaJogo.interactable = MultiplayerManager.Instance.donoDaSala;
    }

}
