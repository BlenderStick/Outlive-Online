using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Outlive.Human
{
    public class Build : MonoBehaviour, Generic.IConstructable
    {
        [SerializeField] private int _maxProgress;
        [SerializeField] private int _currentProgress;
        [SerializeField] private Vector2Int[] _positionsToBuild;

        [SerializeField] private UnityEvent<CallbackContext> _onBuildProgress;

        public class CallbackContext
        {
            public CallbackContext(Generic.IConstructable build, int progress)
            {
                Build = build;
                Progress = progress;
            }

            public int Progress {get; private set;}
            public Generic.IConstructable Build {get; private set;}
        }

        private void Awake() 
        {
            PositionsToBuild = new HashSet<Vector2Int>(Array.ConvertAll(_positionsToBuild, i => i + Vector2Int.RoundToInt(transform.position.To2D())));
        }

        public bool AddBuildProgress(Vector2 builderPosition, int progress)
        {
            float dist = 100f;
            foreach (var item in PositionsToBuild)
            {
                dist = Mathf.Min((item - builderPosition).sqrMagnitude, dist);
                if ((item - builderPosition).sqrMagnitude < 0.1f)
                {
                    if (_currentProgress < _maxProgress)
                        _currentProgress += progress;
                    _onBuildProgress.Invoke(new CallbackContext(this, progress));
                    return true;
                }
            }
                Debug.Log(builderPosition);
            Debug.Log(dist);
            return false;
        }
        public bool NeedBuild {get => MissingProgress > 0;}
        public int MissingProgress {get => _maxProgress - _currentProgress;}
        public HashSet<Vector2Int> PositionsToBuild{get; private set;}
    }
}
