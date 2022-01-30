using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller.Generic
{
    public interface IPlayerController
    {

        ///<summary> 
        /// Desliga todas as entradas do jogador, impedindo o funcionamento de atalhos e controle pelo mouse 
        ///<para> Essa funcionalidade é utilizada principalmente pela interface </para>
        ///</summary>
        ///<param name="obj"> Objeto que solicitou a desabilitação </param>
        void DisableInputs(System.Object obj);
        ///<summary> 
        /// Liga todas as entradas do jogador, permitindo o funcionamento de atalhos e controle pelo mouse 
        ///<para> Essa funcionalidade é utilizada principalmente pela interface </para>
        ///</summary>
        ///<param name="obj"> Objeto que solicitou a desabilitação </param>
        void EnableInputs(System.Object obj);
        

        #region rayCast
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out RaycastHit hit);
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out RaycastHit hit, int layerMask);
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out Collider collider, int layerMask);
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out Collider collider);
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out Vector3 worldPoint, int layerMask);
            ///<summary> Lança um raio a partir da tela do jogador e retorna o objeto atingido </summary>
            ///<returns> True se atingir um objeto e False se não atingir nada </returns>
            bool RayCast(Vector2 pointInScreen, out Vector3 worldPoint);
        #endregion
        
    }
}