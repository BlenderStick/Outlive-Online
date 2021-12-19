using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Generic
{
    public interface ICommandableUnit
    {
        void PutCommand(ICommand command, bool enfilerate);
        void PutInteract(GameObject target, bool enfilerate);
        void UpdateCommand();

        Player player{get;}
    }
}

