using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Manager.Generic
{
    public interface IPlayerInjectable
    {
        void OnInjectablePlayerListChange(IGameManager manager, string[] players);

        void OnInjectablePlayerChange(IGameManager manager, string lastName, string currentName, Color lastColor, Color currentColor);
        void OnGameManagerStart(IGameManager manager);

        void OnInjectorSet(PlayerInjector injector);
    }
}