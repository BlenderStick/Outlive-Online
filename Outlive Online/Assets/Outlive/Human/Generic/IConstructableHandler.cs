using UnityEngine;

namespace Outlive.Human.Generic
{
    public interface IConstructableHandler
    {

        ///<summary>
        ///Sinaliz que um construtor pretende construir essa construção futuramente.
        ///</summary>
        void AddFutureConstructor();
        ///<summary>
        ///Sinaliz que um construtor que iria construir futuramente desistiu de construir.
        ///</summary>
        void SubtractFutureConstructor();
        ///<summary>
        ///Conta quantos construtores tentarão construir futuramente.
        ///</summary>
        int futureConstructors {get;}
        ///<summary>
        ///Verifica se esse construtor pode construir e configura a coordenada que o construtor tem que chegar para começar a construir.
        ///</summary>
        bool ConstructorTryToBuild(IConstructorHandler constructor);
        void ConstructorNotTryToBuild(IConstructorHandler constructor);

        ///<summary>
        ///Verifica se o constructor ocupa uma das posições necessárias para construir
        ///</summary>
        bool VerifyConstructor(IConstructorHandler constructor);
    }
}