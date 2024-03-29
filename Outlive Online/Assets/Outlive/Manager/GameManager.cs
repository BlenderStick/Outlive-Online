﻿using System.Threading;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;

namespace Outlive.Manager
{ 
    [AddComponentMenu("Outlive/GameManager")]
    public class GameManager : MonoBehaviour, Generic.IGameManager
    {

        [FormerlySerializedAs("playerMode"), SerializeField] internal PlayerMode _playerMode;
        [FormerlySerializedAs("autoStartGame"), SerializeField] private bool _autoStartGame = true;
        [SerializeField, Description("Iniciará os players definidos no Inspector")] private bool _createDefaultPlayers = true;

        [SerializeField] private Player _playerIndefinido;
        [SerializeField] private Player[] _players;
        [SerializeField, Header("Events")] private UnityEvent<PlayerListChangeCallback> _onPlayerListChange;
        [SerializeField, Header("Events")] private UnityEvent<IGameManager> _onGameManagerStart;
        private IPlayer[] _iPlayers;

        ///<summary> Armazena os objetos de todos os players </summary>
        protected IList<GameObject>[] _playerObjects;
        protected ReaderWriterLock[] _playersRWLock;
        public bool isGameStarted {get; private set;}
        public IPlayer UndefinedPlayer => _playerIndefinido;
        public UnityEvent<PlayerListChangeCallback> OnPlayerListChange { get => _onPlayerListChange;}
        
        public UnityEvent<IGameManager> OnGameManagerStart { get => _onGameManagerStart;}

        public bool CreatePlayers(params System.Object[] players)
        {
            if (_iPlayers != null)
                return false;

            if (isGameStarted)
                return false;

            _playerObjects = new List<GameObject>[players.Length];
            _iPlayers = new IPlayer[players.Length];
            _playersRWLock = new ReaderWriterLock[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] is IPlayer player)
                    _iPlayers[i] = player;
                else
                    throw new ArrayTypeMismatchException();
                _playersRWLock[i] = new ReaderWriterLock();
                _playerObjects[i] = new List<GameObject>();
            }
            isGameStarted = true;

            return true;
        }
        
        public bool CreatePlayers(params IPlayer[] player) => CreatePlayers((object[]) player);

        public bool CreateUnit(GameObject preFab, Vector3 coord, IPlayer player)
        {
            ICommandableUnit commandable;
            if (preFab.TryGetComponent<ICommandableUnit>(out commandable))
            {
                GameObject instance = GameObject.Instantiate<GameObject>(preFab, coord, Quaternion.Euler(0, 0, 0));
                instance.GetComponent<ICommandableUnit>().player = player;
                instance.SetActive(true);

                return UnitNotify(instance, player);
            }
            return false;
        }

        public bool CreateUnits(IEnumerable<GameObject> preFabs, IEnumerable<Vector3> coords, IEnumerable<IPlayer> players)
        {
            IEnumerator<GameObject> preFabsEnumerator = preFabs.GetEnumerator();
            IEnumerator<Vector3> coordsEnumerator = coords.GetEnumerator();
            IEnumerator<IPlayer> playerEnumerator = players.GetEnumerator();

            while(preFabsEnumerator.MoveNext() && coordsEnumerator.MoveNext() && playerEnumerator.MoveNext())
            {
                if (!CreateUnit(preFabsEnumerator.Current, coordsEnumerator.Current, playerEnumerator.Current))
                    return false;
            }
            return true;
        }

        public bool DestroyUnit(GameObject obj)
        {
            ICommandableUnit commandable;
            if (obj.TryGetComponent<ICommandableUnit>(out commandable))
            {
                if (Array.Exists<IPlayer>(_players, predicateValue => predicateValue == commandable.player))
                {
                    DestroyUnit(obj, Array.IndexOf<IPlayer>(_players, commandable.player));
                    return true;
                }
            }
            return false;
        }

        private void DestroyUnit(GameObject obj, int playerIndex)
        {
            _playersRWLock[playerIndex].AcquireWriterLock(500);

            _playerObjects[playerIndex].Remove(obj);
            
            _playersRWLock[playerIndex].ReleaseWriterLock();
        }

        public IEnumerable<GameObject> AllObjects()
        {
            foreach (IPlayer player in _iPlayers)
            {
                foreach (GameObject obj in PlayerObjects(player))
                {
                    yield return obj;
                }
            }
        }

        public IEnumerable<GameObject> PlayerObjects(IPlayer player)
        {
            int playerIndex = Array.IndexOf(_iPlayers, player);
            _playersRWLock[playerIndex].AcquireReaderLock(500);
            IEnumerable<GameObject> playerGameObjects = _playerObjects[playerIndex];
            foreach (GameObject obj in playerGameObjects)
            {
                yield return obj;
            }
            _playersRWLock[playerIndex].ReleaseReaderLock();
        }

        public IPlayer GetPlayer(string name)
        {
            foreach (IPlayer player in _players)
            {
                if (player.displayName.Equals(name))
                    return player;
            }
            return null;
        }
        public IPlayer GetPlayer(int index)
        {
            if (index < 0)
                return _playerIndefinido;

            if (!Application.isPlaying)
            {
                if (index >= _players.Length)
                    return null;
                return _players[index];
            }
                
            
            if (index >= _players.Length)
                return null;

            return _iPlayers[index];
        }

        public bool UnitNotify(GameObject obj, IPlayer player)
        {
            if (Array.Exists<IPlayer>(_iPlayers, delegateValue => delegateValue == player) 
             && obj.GetComponent<ICommandableUnit>() != null) //Verifica se o player está na lista de players e o objeto possui ICommandableComponent
            {
                return UnitNotify(obj, Array.IndexOf<IPlayer>(_iPlayers, player));
            }
            return false;
        }
        private bool UnitNotify(GameObject obj, int playerIndex)
        {
            IList<GameObject> playerObjects = _playerObjects[playerIndex];
            
            _playersRWLock[playerIndex].AcquireWriterLock(500);
            if (playerObjects.Contains(obj))
                return false;

            playerObjects.Add(obj);
        
            _playersRWLock[playerIndex].ReleaseWriterLock();
            return true;
        }

        private void Awake() 
        {
            if (_createDefaultPlayers)
                CreateDefaultPlayers();

            if (!_autoStartGame)
                return;



            StartGame();
        }

        public void StartGame()
        {
            FirePlayerListChange();
            _onGameManagerStart.Invoke(this);
        }

        ///<summary>
        ///Inicia e configura os players.
        ///</summary>
        public void CreateDefaultPlayers()
        {
            CreatePlayers(_players);

            foreach (var item in _iPlayers)
            {
                item.Awake();
            }
        }
        

        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in _playerObjects)
            {
                foreach (var item2 in item)
                {
                    Debug.Log(item2);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal Player GetPlayerInEditor(string name)
        {
            foreach (var item in _players)
            {
                if (item.displayName == name)
                    {return item;}
            }
            return null;
        }

        internal string checkExistPlayerName(string name)
        {
            foreach (var item in _players)
            {
                if (item.displayName == name)
                    return name + "_001";
            }
            return name;
        }

        public void FirePlayerListChange()
        {
            _onPlayerListChange.Invoke(new PlayerListChangeCallback(this, _players));
        }

        public class PlayerListChangeCallback
        {
            internal PlayerListChangeCallback(Generic.IGameManager manager, Player[] players)
            {
                this.manager = manager;
                this.players = players;
            }
            public Generic.IGameManager manager {get; private set;}
            public Player[] players {get; private set;}

        }

        public class PlayerChangeCallback
        {
            internal PlayerChangeCallback(Generic.IGameManager manager, String lastName, String currentName, Color lastColor, Color currentColor)
            {
                this.manager = manager;
                this.lastName = lastName;
                this.currentName = currentName;
                this.lastColor = lastColor;
                this.currentColor = currentColor;
            }

            public Generic.IGameManager manager {get; private set;}
            public string lastName {get; private set;}
            public string currentName {get; private set;}
            public Color lastColor {get; private set;}
            public Color currentColor {get; private set;}
        }

        public enum PlayerMode
        {
            Reference,
            ManualCreate
        }
    }
}