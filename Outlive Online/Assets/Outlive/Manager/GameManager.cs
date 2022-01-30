using System.Threading;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Outlive.Manager
{ 
    [AddComponentMenu("Outlive/GameManager")]
    public class GameManager : MonoBehaviour, Generic.IGameManager
    {

        [FormerlySerializedAs("playerMode"), SerializeField] internal PlayerMode _playerMode;
        [FormerlySerializedAs("autoStartGame"), SerializeField] private bool _autoStartGame = true;

        [SerializeField] private UnityEngine.Object[] _Object_Player;
        [SerializeField] private Player[] _players;
        private IPlayer[] _iPlayers;

        protected IList<GameObject>[] _playerObjects;
        protected ReaderWriterLock[] _playersRWLock;
        public bool isGameStarted {get; private set;}


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
            foreach (IPlayer player in _iPlayers)
            {
                if (player.displayName.Equals(name))
                    return player;
            }
            return null;
        }
        public IPlayer GetPlayer(int index)
        {
            if (index < 0)
                return null;

            if (!Application.isPlaying)
            {
                if (_playerMode == PlayerMode.ManualCreate)
                {
                    if (index >= _players.Length)
                        return null;
                    return _players[index];
                }
                else
                {
                    if (index >= _Object_Player.Length)
                        return null;
                    UnityEngine.Object item = _Object_Player[index];
                    if (item == null)
                        return null;
                    return (IPlayer) item;
                }
            }
                
            
            if (index >= _iPlayers.Length)
                return null;

            return _iPlayers[index];
            // else
            // {
            //     if (index >= ReferencedPlayer.Count)
            //         return null;

            //     return ReferencedPlayer[index];
            // }
            
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

            if (!_autoStartGame)
                return;

            StartGame();
            // ;

            // _players = new IPlayer[CreatedPlayers.Count];
            // for (int i = 0; i < CreatedPlayers.Count; i++)
            // {
            //     _players[i] = CreatedPlayers[i];
            // }
                
        }

        ///<summary>
        ///Inicia os e configura os players.
        ///</summary>
        public void StartGame()
        {
            if (_playerMode == PlayerMode.ManualCreate)
                CreatePlayers(_players);
            else
                CreatePlayers(_Object_Player);

            foreach (var item in _iPlayers)
            {
                item.Awake();
            }
        }
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public enum PlayerMode
        {
            Reference,
            ManualCreate
        }
    }
}