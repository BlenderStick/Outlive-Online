using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

public interface IBehaviour
{
    ///<summary>
    ///Verifica se este comando é executavel por este behaviour
    ///</summary>
    ///<param name="command"> Comando que será executado </param>
    bool Condition(ICommand command);
    ///<summary>
    ///Executa o comportamento do comando, quando o comando for satisfeito, retorna false.
    ///</summary>
    ///<param name="obj"> Objeto que recebe o comportamento </param>
    ///<param name="command"> O commando </param> 
    bool UpdateBehaviour(GameObject obj, ICommand command);
    ///<summary>
    ///Chamado quando a execução do behaviour é interrompida.
    ///</summary>
    void Cancel(GameObject obj, ICommand command);
    ///<summary>
    ///Configura o behaviour com o objeto que o está utilizando
    ///</summary>
    void Setup(GameObject obj);
    ///<summary>
    ///Reseta o behaviour para as configurações iniciais.
    ///</summary>
    void Reset();
}
