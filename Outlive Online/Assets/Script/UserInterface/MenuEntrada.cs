using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEntrada : MonoBehaviour
{

    [SerializeField] private Text _nomeDoJogador;
    [SerializeField] private Text _nomeDaSala;

    public void CriaSala()
    {
        MultiplayerManager.Instance.MudaNick(_nomeDoJogador.text);
        MultiplayerManager.Instance.CriaSala(_nomeDaSala.text);
    }

    public void EntraSala() 
    {
        MultiplayerManager.Instance.MudaNick(_nomeDoJogador.text);
        MultiplayerManager.Instance.EntraSala(_nomeDaSala.text);
    }
    
}
