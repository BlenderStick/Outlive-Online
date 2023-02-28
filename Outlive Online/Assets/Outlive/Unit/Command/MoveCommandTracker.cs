using System.Collections.Generic;
using System;
using Outlive.Behaviours;
using UnityEngine;
using System.Linq;

namespace Outlive.Unit.Command
{
    public class MoveCommandTracker : ICommandTracker, IDisposable
    {

        private List<MoveCommand> _commands;
        private HashSet<Vector2Int> _positions;
        private int _maxComands;
        private int _skipedComands;
        public int Count {get; set;}
        public MoveCommandTracker(Vector2Int target, int maxComands, Func<Vector2Int, bool> contain, Func<Vector2Int, Vector3> project)
        {
            Target = target;
            _maxComands = maxComands;
            Contain = contain;
            Project = project;
            _commands = new List<MoveCommand>(maxComands);
        }

        public void Calcule()
        {
            IsStarted = true;
            _positions = Outlive.OutliveUtilites.CalculePointsAroundGrid(Target, _maxComands, Contain);

            SetComandsTarget();
            // HashSet<Vector2Int> positions = OutliveUtilites.CalculePointsAroundGrid(Target, _commands.Count, Contain);
            // foreach (var item in positions.Zip(_commands, ValueTuple.Create))
            // {
            //     item.Item2.Target = Project(item.Item1);
            // }
        }

        private void SetComandsTarget()
        {
            foreach (var item in _positions.Zip(_commands, ValueTuple.Create))
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
            command.OnStart += Start;
            command.OnSkip += Skip;
            return true;
        }
        public Vector2Int Target {get; private set;}
        public Func<Vector2Int, Vector3> Project {get; private set;}
        public Func<Vector2Int, bool> Contain { get; private set; }
        public bool IsStarted { get; private set; }

        private void Start(object sender, ICommand command)
        {
            if (disposed)
                return;
            _commands.Add((MoveCommand) command);
            command.OnStart -= Start;
            SetComandsTarget();
        }

        private void Skip(object sender, ICommand command)
        {
            if (disposed)
                return;
            
            _skipedComands ++;
            _commands.Remove((MoveCommand) command);
            command.OnSkip -= Skip;

            if (_skipedComands == _maxComands)
                Dispose();
            else
                SetComandsTarget();

        }
        bool disposed;
        ~MoveCommandTracker()
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