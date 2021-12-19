using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Interacts
{
    public abstract class IUnitInteract : ScriptableObject
    {
        ///<summary>
        ///Verifica se o objeto source pode interagir com o objeto target, se sim, retorna true e devolve o behaviour compatível com a ação desejada.
        ///</summary>
        ///<param name="source"> Objeto que irá executar a interação </param>
        ///<param name="target"> Alvo da interação </param>
        ///<param name="behaviour"> Comportamento que o objeto <paramref name="source"/> deve executar </param>
        public abstract bool Interact(GameObject source, GameObject target, out IBehaviour behaviour);
        ///<summary>
        ///Verifica se o objeto source pode interagir com o objeto target, se sim, retorna true e devolve o command correspondente à interação desejada.
        ///</summary>
        ///<param name="source"> Objeto que irá executar a interação </param>
        ///<param name="target"> Alvo da interação </param>
        ///<param name="command"> Comando que o objeto <paramref name="source"/> deve executar </param>
        public abstract bool Command(GameObject source, GameObject target, out ICommand command);
    }
}