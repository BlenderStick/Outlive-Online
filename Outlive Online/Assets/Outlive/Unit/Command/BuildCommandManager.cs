using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Outlive.Behaviours;
using Outlive.Unit.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    class BuildCommandManager: ICommandManager
    {
        private IList<BuildCommand> _buildCommands;
        private IList<MoveCommand> _onlyMove;
        public bool IsCalculated {get; private set;}

        public int Count {get; set;}

        public event EventHandler<ProgressEventArgs> OnProgress;

        public class ProgressEventArgs: EventArgs
        {
            public ProgressEventArgs(BuildCommandManager manager, Vector2 constructorPosition, int value)
            {
                Manager = manager;
                ConstructorPosition = constructorPosition;
                Value = value;
                Use = true;
            }
            public BuildCommandManager Manager {get; private set;}
            public Vector2 ConstructorPosition {get; private set;}
            public int Value{get; private set;}
            ///<summary> Se estiver em false, o evento foi cancelado e o construtor terá que se reposicionar </summary>
            public bool Use {get; private set;}
            ///<summary> Cancelar faz com que o construtor se reposicione </summary>
            public void Cancel() => Use = false;
        }

        public Func<Vector2, bool> BuildPositionsContains {get; private set;}
        public Func<Vector2Int, bool> MaskContains {get; private set;}
        public Func<Vector2Int, Vector3> Project {get; set;}
        public Vector2Int Target;


        public BuildCommandManager(Func<Vector2, bool> buildPositionsContains, Func<Vector2Int, bool> maskContain)
        {
            BuildPositionsContains = buildPositionsContains;
            MaskContains = maskContain;
            Project = v => new Vector3(v.x, 0, v.y);
            _buildCommands = new List<BuildCommand>();
            _onlyMove = new List<MoveCommand>();
        }

        public void EnterCommand(ICommand command)
        {
            if (command is MoveCommand moveCommand)
            {
                _onlyMove.Add(moveCommand);
                command.OnSkip += (o, c) =>
                {
                    _onlyMove.Remove(moveCommand);
                };
            }
            else if (command is BuildCommand buildCommand)
            {
                _buildCommands.Add(buildCommand);
                command.OnSkip += (o, c) =>
                {
                    _buildCommands.Remove(buildCommand);
                };
                // buildCommand.
            }
        }

        public bool CanUseCommand(Func<string, bool> canExecuteCommand, out ICommand command)
        {
            if (canExecuteCommand(BehaviourPreset.Behaviour_Build))
            {
                command = new BuildCommand(AddProgress);
                return true;
            }
            if (canExecuteCommand(BehaviourPreset.Behaviour_Move))
            {
                command = new MoveCommand();
                return true;
            }
            command = null;
            return false;
        }

        public bool IsCanceled {get; private set;}
        public void Cancel()
        {
            IsCanceled = true;
            foreach (var item in _buildCommands)
            {
                item.Done();
            }
            foreach (var item in _onlyMove)
            {
                item.Done();
            }
        }
        public void Calcule()
        {
            HashSet<Vector2Int> positions = OutliveUtilites.CalculePointsAroundGrid(Target, _buildCommands.Count + _onlyMove.Count, MaskContains);
            IEnumerator<Vector2Int> positionsEnumerator = positions.GetEnumerator();
            IEnumerator<BuildCommand> buildEnumerator = _buildCommands.GetEnumerator();
            IEnumerator<MoveCommand> moveEnumerator = _onlyMove.GetEnumerator();

            while (positionsEnumerator.MoveNext() && buildEnumerator.MoveNext())
            {
                buildEnumerator.Current.Target = Project(positionsEnumerator.Current);
            }
            while (positionsEnumerator.MoveNext() && moveEnumerator.MoveNext())
            {
                moveEnumerator.Current.Target = Project(positionsEnumerator.Current);
            }
        }

        private bool FireProgress(Vector2 position, int value)
        {
            Debug.Log("Fire " + position);
            if (!BuildPositionsContains.Invoke(position))
                return false;
                
            ProgressEventArgs evt = new ProgressEventArgs(this, position, value);
            OnProgress?.Invoke(this, evt);
            return evt.Use;
        }

        public void Calcule(HashSet<Vector2Int> buildPositions, Func<Vector2Int, bool> contain, Func<Vector2Int, Vector3> project)
        {
            IsCalculated = true;
            int max = (_buildCommands.Count > buildPositions.Count? _buildCommands.Count - buildPositions.Count: 0) + _onlyMove.Count;
            Vector2Int midPoint = Vector2Int.zero;
            foreach (var item in buildPositions)
            {
                midPoint += item;
            }
            midPoint /= buildPositions.Count;

            HashSet<Vector2Int> moves = OutliveUtilites.CalculePointsAroundGrid(midPoint, max, contain);
            int i = 0;
            foreach (var item in buildPositions)
            {
                if (i < _buildCommands.Count)
                    _buildCommands[i++].Target = project(item);
            }
            int index = 0;
            foreach (var item in moves)
            {
                if (i < _buildCommands.Count)
                {
                    _buildCommands[i++].Target = project(item);
                }
                else
                {
                    _onlyMove[index++].Target = project(item);
                }
            }
            
        }

        public bool AddProgress(Vector2 position, int value) => FireProgress(position, value);

        public void Skip(object sender, ICommand command)
        {

        }

    }
}