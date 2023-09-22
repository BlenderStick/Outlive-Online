using System.Collections;
using System.Collections.Generic;
using Outlive.Controller;
using Outlive.Manager;
using Outlive.Manager.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerEditorSet : MonoBehaviour, IPlayerInjectable
{

    [SerializeField] private PlayerSelect _playerSelect;
    [SerializeField] private UnityEngine.Events.UnityEvent<Outlive.Manager.Player> _onPlayerChange;
    [SerializeField] private UnityEngine.Events.UnityEvent<Color> _onColor;

    private void Awake() {
        if (Application.isPlaying)
            return;

        PlayerInjector injector = FindAnyObjectByType<PlayerInjector>();
        if (injector != null)
            injector.AddInjectable(this);
    }

    private void OnDestroy() {
        if (Application.isPlaying)
            return;

        PlayerInjector injector = FindAnyObjectByType<PlayerInjector>();
        if (injector != null)
            injector.RemoveInjectable(this);
    }
    public void OnLoadPlayerListChange(IGameManager manager, Outlive.Manager.Player[] players)
    {
        _playerSelect.SetPlayerList(players);
        if(_playerSelect.isPlayerUndefined)
        {
            _onPlayerChange.Invoke((Outlive.Manager.Player) manager.UndefinedPlayer);
            _onColor.Invoke(manager.UndefinedPlayer.color);
            return;
        }

        string playerName = _playerSelect.PlayerName;
        Color color = players[_playerSelect.PlayerIndex].color;
        _onPlayerChange.Invoke(players[_playerSelect.PlayerIndex]);
        _onColor.Invoke(color);
    }

    public class PlayerSetCallback
    {

        public PlayerSetCallback(string displayName, Color color)
        {
            this.displayName = displayName;
            this.color = color;
        }

        public string displayName {get; private set;}
        public Color color {get; private set;}
    }
}
