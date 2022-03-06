using System;

namespace Outlive.Unit.Command
{
    public interface ICommand
    {
        object alvo { get; }

        void Start();
        void Skip();
        event EventHandler<ICommand> OnStart;
        event EventHandler<ICommand> OnSkip;
    }
}