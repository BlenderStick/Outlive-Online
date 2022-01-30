using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Outlive.Manager.Generic;

namespace Outlive.Map.Generic
{
    public interface IMapSettings
    {
        Sprite sprite{get;}
        string mapName{get;}

        IEnumerable<IPlayer> playables{get;}
        IEnumerable<bool> changeColor{get;}
        IEnumerable<bool> changeName{get;}

        bool SetColor(int playerIndex, Color newColor);
        bool SetName(int playerIndex, string newName);

        void LoadMap();

    }
}