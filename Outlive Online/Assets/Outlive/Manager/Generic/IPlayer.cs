using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Manager.Generic
{
    public interface IPlayer
    {
        ///<summary> Cor que as unidades desse jogador exibem no jogo </summary>
        Color color{get;}
        ///<summary> Nome do jogador </summary>
        string displayName{get;}

        ///<summary> 
        /// Permite iterar entre todas as unidades comandadas por esse jogador
        ///<para> É importante ter cuidado com o tratamento dos itens pois eles não são cópias </para>
        ///<para> Todo GameObject retornado possui um Component herdado de <see cref="Outlive.Unit.Generic.ICommandableUnit"/></para>
        ///</summary>
        ///<param name="function"> Função que recebe os objetos da interação, retorna true se deve continuar o ForEach e false se deve parar </param>
        // void InteractUnits(System.Func<GameObject, bool> function);

        ///<summary>
        /// Obtém todos os objetos pertencentes a esse player, permitindo interar entre todos eles
        ///</summary>
        IEnumerable<GameObject> units {get;}

        void Awake();
    }
}