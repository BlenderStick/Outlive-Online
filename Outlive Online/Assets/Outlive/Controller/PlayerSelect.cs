using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager;
using UnityEngine;

namespace Outlive.Controller
{
    [Serializable]
    public class PlayerSelect
    {
        [SerializeField, HideInInspector] private string[] _playerList;
        [SerializeField, HideInInspector] private int _playerIndex = -1;

        public void SetPlayerList(Outlive.Manager.Player[] newPlayerList) => _playerList = Array.ConvertAll(newPlayerList, (param) =>{return param.displayName;});

        public bool isPlayerUndefined => _playerIndex == -1;

        public int PlayerIndex => _playerIndex;

        public string PlayerName
        {
            set
            {
                _playerIndex = Array.IndexOf(_playerList, value);
            }
            get
            {
                if (_playerIndex == -1)
                    return null;
                return _playerList[_playerIndex];
            }
        }
    }
}