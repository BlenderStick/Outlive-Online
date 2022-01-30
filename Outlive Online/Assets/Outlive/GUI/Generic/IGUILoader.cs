using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.GUI.Generic
{
    public interface IGUILoader
    {
        ///<summary>
        ///Carrega a GUI na janela do jogador
        ///</summary>
        void load(IGUILoaderEvent evt);
        ///<summary>
        ///Remove a GUI da janela do jogador
        ///</summary>
        void leave(IGUILoaderEvent evt);
    }
}