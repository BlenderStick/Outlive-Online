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
        void Load(GUIManager.CallbackContext ctx);
        ///<summary>
        ///Atualiza a interface
        ///</summary>
        void Update(GUIManager.CallbackContext ctx);
        ///<summary>
        ///Remove a GUI da janela do jogador
        ///</summary>
        void Leave(GUIManager.CallbackContext ctx);
    }
}