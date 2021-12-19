using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Human.Generic
{
    public interface IConstructorHandler
    {
        void ConnectConstructable(IConstructableHandler constructable);
        ///<summary>
        ///Verifica se este construtor está construindo esta construção
        ///</summary>
        bool IsBuilding(IConstructableHandler constructable);
        float DistancePathTo(Vector3 coord);
        float SqrDistancePathTo(Vector3 coord);
        ///<summary>
        ///Define a posição que o construtor tem que andar para começar a construir.
        ///<para>
        ///A operação só é válida se <paramref name="constructable"/> for a mesma construção que o construtor tenta construir.
        ///</para>
        ///</summary>
        void SetPositionToConstruct(Vector3 position, IConstructableHandler constructable);

        void DisconectConstructable(IConstructableHandler constructable);
        Transform transform{get;}
    }
}