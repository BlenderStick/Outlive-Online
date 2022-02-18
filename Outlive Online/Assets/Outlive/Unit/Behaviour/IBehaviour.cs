using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

public interface IBehaviour: IDisposable
{
    ///<summary>
    ///Executa o comportamento do comando, quando o comando for satisfeito, retorna false.
    ///</summary>
    ///<param name="obj"> Objeto que recebe o comportamento </param>
    ///<param name="command"> O comando </param> 
    ///<param name = "cancel"> O comportamento precisa ser parado </param>
    bool UpdateBehaviour(GameObject obj, ICommand command, bool cancel = false);
    ///<summary>
    ///Chamado quando a execução do behaviour é interrompida.
    ///</summary>
    void ForceCancel(GameObject obj, ICommand command);
    ///<summary>
    ///Configura o behaviour com o objeto que o está utilizando
    ///</summary>
    void Setup(GameObject obj, ICommand command);
}
