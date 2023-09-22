using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Manager.Generic
{
    public interface IPlayerInjectable
    {
        void OnLoadPlayerListChange(IGameManager manager, Player[] players);

    }
}