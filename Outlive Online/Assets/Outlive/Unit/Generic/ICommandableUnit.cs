using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Generic
{
    public interface ICommandableUnit
    {
        void PutCommand(ICommand command, bool enfilerate);
        void UpdateCommand();

        bool CanExecuteCommand(string commandName);

        IPlayer player{get; set;}
    }
}

