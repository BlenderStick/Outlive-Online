using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Human.Generic
{
    public interface IConstructable
    {
        ///<summary> Verifica se o construtor atende as condições necessárias para começar a construir</summary>
        bool CanBuild(IConstructor constructor);
        ///<summary> 
        ///Registra o construtor na lista de construtores que estão construindo, 
        ///isso não depende do construtor atender as condições necessárias para construir
        ///</summary>
        void RegisterConstructor(IConstructor constructor);
        void UnregisterConstructor(IConstructor constructor);
        bool HaveConstructor(IConstructor constructor);
        ///<summary> Máximo de construtores que podem construir </summary>
        int MaxConstructors {get;}
        ///<summary> Quantidade de construtores registrados para construir </summary>
        int ConstructorsCount {get;}
        ///<summary> Quantidade de construtores registrados que atendem as condições para construir </summary>
        int ConstructorsBuilding {get;}
        Vector2Int[] PointsToBuild {get;}
        IEnumerable<IConstructor> Constructors {get;}
    }
}