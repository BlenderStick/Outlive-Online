using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller
{
    [Serializable]
    public class PlayerSelect
    {
        [SerializeField, HideInInspector] private string[] _playerList = new string[] {"Undefined"};
        [SerializeField, HideInInspector] private int _playerIndex = 0;
        public void SetPlayerList(params string[] list)
        {
            _playerList = new string[list.Length + 1];
            _playerList[0] = "Undefined";
            for (int i = 0; i < list.Length; i++)
            {
                _playerList[i + 1] = list[i];
            }
        }

        public void UpdateName(string lastName, string newName)
        {
            for (int i = 1; i < _playerList.Length; i++)
            {
                if (_playerList[i] == lastName)
                {
                    _playerList[i] = newName;
                    return;
                }
            }
        }

        public bool isPlayerUndefined => _playerIndex == 0;

        public string PlayerName
        {
            set
            {
                _playerIndex = Array.IndexOf(_playerList, value);
            }
            get
            {
                if (_playerIndex == 0)
                    return null;
                return _playerList[_playerIndex];
            }
        }
    }
}