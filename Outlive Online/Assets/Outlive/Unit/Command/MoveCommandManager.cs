using System.Collections.Generic;
using System;
using Outlive.Behaviours;
using UnityEngine;
using System.Linq;

namespace Outlive.Unit.Command
{
    public class MoveCommandManager : ICommandManager, IDisposable
    {

        private List<MoveCommand> _commands;
        public int Count {get; set;}
        public MoveCommandManager(Vector2Int target, Func<Vector2Int, bool> contain, Func<Vector2Int, Vector3> project)
        {
            Target = target;
            Contain = contain;
            Project = project;
            _commands = new List<MoveCommand>();
        }

        public void EnterCommand(MoveCommand command)
        {
            _commands.Add(command);
            command.OnSkip += Skip;
        }

        public void Calcule()
        {
            IsStarted = true;
            HashSet<Vector2Int> positions = OutliveUtilites.CalculePointsAroundGrid(Target, _commands.Count, Contain);
            foreach (var item in positions.Zip(_commands, ValueTuple.Create))
            {
                item.Item2.Target = Project(item.Item1);
            }
        }

        public bool CanUseCommand(Func<string, bool> canExecuteCommand, out MoveCommand command)
        {
            if (!canExecuteCommand(BehaviourPreset.Behaviour_Move))
            {
                command = null;
                return false;
            }

            command = new MoveCommand();
            command.OnSkip += Skip;
            return true;
        }
        public Vector2Int Target {get; private set;}
        public Func<Vector2Int, Vector3> Project {get; private set;}
        public Func<Vector2Int, bool> Contain { get; private set; }
        public bool IsStarted { get; private set; }

        private void Skip(object sender, ICommand command)
        {
            if (disposed)
                return;
            _commands.Remove((MoveCommand) command);
            command.OnSkip -= Skip;
        }
        bool disposed;
        ~MoveCommandManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    
                }
                _commands.Clear();
                _commands = null;
                Project = null;
                Contain = null;
                disposed = true;
            }
        }
    }
}