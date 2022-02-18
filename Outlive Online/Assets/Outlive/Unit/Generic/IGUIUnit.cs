using System.Security.Cryptography;
namespace Outlive.Unit.Generic
{
    public interface IGUIUnit
    {
        ///<summary>
        ///O nome do tipo de unidade que ajudará o GUIManager a escolher a interface de usuário certa.<para>
        ///O GUIManager utiliza a função Outlive.GUILoad.GetGUI( UnitName ) para localizar a interface certa.</para>
        ///</summary>
        string UnitName {get;}
    }
}