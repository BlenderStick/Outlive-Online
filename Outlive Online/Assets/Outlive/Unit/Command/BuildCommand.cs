using System.Data.Common;
using System;
using Outlive.Human.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class BuildCommand : ICommand, IDisposable
    {
        private Func<Vector2, int, bool> _addProgress;
        public BuildCommand(Func<Vector2, int, bool> addProgressCommand)
        {
            if (addProgressCommand == null)
                throw new ArgumentNullException();

            _addProgress = addProgressCommand;
        }
        
        private bool _targetChanged;
        private Vector3 _target;
        public Vector3 Target
        {
            get => _target;
            internal set
            {
                _target = value;
                _targetChanged = true;
            }
        }
        public void FireStart() => OnStart?.Invoke(this, this);
        public object alvo => Target;

        public event EventHandler<ICommand> OnStart;
        public event EventHandler<ICommand> OnSkip;

        public bool CheckChegou(Vector3 position)
        {
            return (position - Target).sqrMagnitude < 0.1f;
        }

        public BuildState CheckState(Vector3 position, bool building)
        {
            if (_done)
                return BuildState.Completed;
            else if (_targetChanged)
            {
                _targetChanged = false;
                return BuildState.TargetChanged;
            }
            else if (CheckChegou(position))
            {
                if (!building)
                    return BuildState.CanBuild;
            }
            return BuildState.None;
        }

        public bool AddProgress(Vector3 position, int value) => _addProgress(position.To2D(), value);

        public void Skip() => OnSkip?.Invoke(this, this);
        
        public void Start() => OnStart?.Invoke(this, this);
        
        bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                OnStart = null;
                OnSkip = null;
                // Release unmanaged resources.
                // Set large fields to null.                
                disposed = true;
            }
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Done()
        {
            _done = true;
        }


        ~BuildCommand() // the finalizer
        {
            Dispose(false);
        }
        private bool _done = false;
        public bool IsDone => _done;
    }

    public enum BuildState
    {
        None,
        TargetChanged,
        CanBuild,
        Completed
    }
}