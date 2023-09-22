using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Manager.Generic
{
    public interface IGameManager
    {
        
        bool CreatePlayers(params IPlayer[] player);
        ///<summary> Instancia um grupo de unidades a partir de preFabs </summary>
        ///<param name="preFabs"> Prefabs a serem instanciadas </param>
        ///<param name="coords"> Coordenada de cada unidade </param>
        ///<param name="players"> Jogador correspondente de cada unidade </param>
        bool CreateUnits(IEnumerable<GameObject> preFabs, IEnumerable<Vector3> coords, IEnumerable<IPlayer> players);
        ///<summary> Avisa ao GameManager que uma unidade foi instanciada por fora </summary>
        bool UnitNotify(GameObject obj, IPlayer player);
        bool CreateUnit(GameObject preFab, Vector3 coord, IPlayer player);
        bool DestroyUnit(GameObject obj);
        IPlayer GetPlayer(string name);
        IPlayer GetPlayer(int index);
        ///<summary>
        ///Executa um foreach interno para todos os objetos do jogador
        ///<para> Todo objeto dessa lista possui um <see cref="Outlive.Unit.Generic.ICommandableUnit"/> Component </para>
        ///</summary>
        ///<param name="function"> Função com um argumento GameObject e retorno bool, se retornar False, para o foreach </param>
        // void ForEachObjects(System.Func<GameObject, bool> function, IPlayer player);
        ///<summary>
        ///Executa um foreach interno para todos os objetos de todos os jogadores
        ///<para> Todo objeto dessa lista possui um <see cref="Outlive.Unit.Generic.ICommandableUnit"/> Component </para>
        ///</summary>
        ///<param name="function"> Função com um argumento GameObject e retorno bool, se retornar False, para o foreach </param>
        // void ForEachObjects(System.Func<GameObject, bool> function);
        ///<summary>
        ///Todos os GameObjects listados pelos players
        ///<summary>
        IEnumerable<GameObject> AllObjects();
        ///<summary>
        ///Todos os GameObjects pertencentes ao players
        ///<summary>
        IEnumerable<GameObject> PlayerObjects(IPlayer player);
        ///<summary> Verifica se o jogo foi iniciado </summary>
        bool isGameStarted{get;}
        /// <summary>O Player que define a equipe das unidades que não tem uma equipe</summary>
        IPlayer UndefinedPlayer { get; }
    }
}