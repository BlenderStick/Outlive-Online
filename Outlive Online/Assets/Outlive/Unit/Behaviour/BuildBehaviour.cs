using System;
using Outlive.Human.Generic;
using Outlive.Unit.Command;
using UnityEngine;
using UnityEngine.AI;

namespace Outlive.Unit.Behaviour
{
    public class BuildBehaviour : IBehaviour
    {

        private NavMeshAgent agent;
        private bool _chegou;
        private float _timeStartCount;
        private float _timeStopCount;
        private float _timeStepCount;
        private bool _isStarted;
        private bool _isStoped;
        private bool _isBuildPause = false;
        private MoveBehaviour _moveBehaviour;
        private MoveCommand _moveCommand;
        private ToBuild _toBuild;

        ///<summary>Tempo em segundos</summary>
        public float BuildTimeStep {get; set;}
        ///<summary>Tempo em segundos</summary>
        public float StartBuildTime {get; set;}
        ///<summary>Tempo em segundos</summary>
        public float StopBuildTime {get; set;}
        public bool IsBuildPause
        {
            get => _isBuildPause;
            set
            {
                if (_isBuildPause == value)
                    return;

                _isBuildPause = value;
                _toBuild.ProgressPause = value;
                if (OnStateChange == null)
                    return;
                if (value)
                    OnStateChange(new CallbackContext(this, BuildBehaviourState.Pause));
                else
                    OnStateChange(new CallbackContext(this, BuildBehaviourState.Unpause));
            }
        }
        public int ProgressValue {get; set;}

        public event BuildEvent OnStateChange;
        public delegate void BuildEvent (CallbackContext ctx);

        public class CallbackContext
        {
            public CallbackContext(BuildBehaviour behaviour, BuildBehaviourState state)
            {
                Behaviour = behaviour;
                State = state;
            }

            public BuildBehaviour Behaviour {get; private set;}
            public BuildBehaviourState State {get; private set;}
        }

        public BuildBehaviour()
        {
            BuildTimeStep = 1f;
            StartBuildTime = 1f;
            StopBuildTime = 1f;
            ProgressValue = 1;
        }

        public void Dispose()
        {
            agent = null;
        }

        public void ForceCancel(GameObject obj, ICommand command)
        {
            throw new NotImplementedException();
        }

        public void Setup(GameObject obj, ICommand command)
        {
            if (!obj.TryGetComponent(out agent))
                throw new MissingComponentException("NavMeshAgent necessario");

            _moveBehaviour = new MoveBehaviour();
            _moveCommand = new MoveCommand();
            _toBuild = new ToBuild(StartBuildTime, BuildTimeStep, StopBuildTime);

            command.Start();

            _moveBehaviour.Setup(obj, _moveCommand);
            _toBuild.OnProgress += () => 
            {
                if (!((BuildCommand) command).AddProgress(agent.nextPosition, 1))
                    _targetChanged = true;
            };
        }

        private bool _isMoveStart = true;
        private bool _building = false;
        private bool _moveCompleted = false;
        private bool _targetChanged = false;
        private bool _completed = false;
        public bool UpdateBehaviour(GameObject obj, ICommand command, bool cancel = false)
        {
            BuildCommand buildCommand = command as BuildCommand;
            BuildState state = buildCommand.CheckState(agent.nextPosition, _building);

            if (state == BuildState.Completed)   
                _completed = true;

            if (state == BuildState.TargetChanged)
            {
                _moveCommand.Target = buildCommand.Target;
                _targetChanged = true;
            }
                
            
            if (!_building)
            {
                if (!_moveBehaviour.UpdateBehaviour(obj, _moveCommand, cancel || _completed))
                {
                    if (_completed || cancel)
                        return false;

                    _targetChanged = false;
                    if (state == BuildState.CanBuild)
                        _building = true;
                }
            }
            else
            {
                if (_toBuild.Tick(cancel || _targetChanged || _completed))
                {
                    if (_completed || cancel)
                        return false;
                    
                    if (_targetChanged)
                        _building = false;
                }
            }
            return true;

            // if (cancel)
            //     _chegou = true;
            
            // if (_isMoveStart)
            // {
            //     _isMoveStart = false;
            //     buildCommand.FireStart();
            // }
                
            // if (!_chegou)
            // {
            //     if (agent.destination != buildCommand.Target)
            //     {
            //         agent.destination = buildCommand.Target;
            //         agent.isStopped = false;
            //     }
            //     _chegou = buildCommand.CheckChegou(obj.transform.position);
            //     if (_chegou)
            //     {
            //         agent.avoidancePriority -= 1;
            //         agent.isStopped = true;
            //     }
            //     return !_isStoped;
            // }

            // if (!buildCommand.Done && !cancel)
            // {
            //     if (_timeStartCount < StartBuildTime)
            //         _timeStartCount += Time.deltaTime;
            //     else
            //     {
            //         if (!_isStarted)
            //         {
            //             OnStateChange?.Invoke(new CallbackContext(this, BuildBehaviourState.Start)); ///Start event
            //             _isStarted = true;
            //         }
            //         if (!IsBuildPause)
            //         {
            //             if (_timeStepCount < BuildTimeStep)
            //             {
            //                 _timeStepCount += Time.deltaTime;
            //             }
            //             else
            //             {
            //                 _timeStepCount = 0;
            //                 buildCommand.AddProgress(obj.transform.position.To2D(), ProgressValue);
            //             }
            //         }
            //     }
            // }
            // else
            // {
            //     if (_timeStopCount < StopBuildTime)
            //     {
            //         _timeStopCount += Time.deltaTime;
            //     }
            //     else
            //     {
            //         _isStoped = true;
            //         OnStateChange?.Invoke(new CallbackContext(this, BuildBehaviourState.Stop)); ////Event stop
            //         agent.avoidancePriority += 1;
            //     }
            // }


            // return !_isStoped;
        }

        private class ToBuild
        {
            private Contador _initialize;
            private Contador _progress;
            private Contador _finalize;
            public bool ProgressPause {get; set;}
            private int state;
            public event Action OnProgress;

            public ToBuild(float _initializeTime, float _progressTime, float _finalizeTime)
            {
                state = 0;
                _initialize = new Contador(_initializeTime);
                _progress = new Contador(_progressTime, true);
                _finalize = new Contador(_finalizeTime);
            }

            ///<summary> Quando o processo terminar, returna True </summary>
            public bool Tick(bool cancel = false)
            {
                switch (state)
                {
                    case 0:
                        if (_initialize.Tick())
                        {
                            _initialize.Reset();
                            if (cancel)
                                state = 2;
                            else
                                state = 1;
                        }
                        break;
                    case 1:
                        if (!ProgressPause)
                        {
                            if (_progress.Tick(cancel))
                            {
                                _progress.Reset();
                                if (cancel)
                                    state = 2;
                                else
                                    OnProgress?.Invoke();
                            }
                        }
                        else
                        {
                            if (cancel)
                            {
                                state = 2;
                                _progress.Reset();
                            }
                                
                            else
                                OnProgress?.Invoke();
                        }
                        break;
                    case 2:
                        if (_finalize.Tick())
                        {
                            _finalize.Reset();
                            return true;
                        }
                        break;
                }
                return false;
            }
        }

        private class Contador 
        {
            private float _current;
            private float _max;
            private bool _aways;

            public event Action<Contador> OnTick;

            public Contador(float max, bool aways = false)
            {
                _max = max;
                _aways = aways;
            }

            public bool Tick(bool cancel = false)
            {
                if (_current >= _max || (cancel && _aways))
                    return true;
                _current += Time.deltaTime;

                return false;
            }
            public void Reset()
            {
                _current = 0;
            }
        }
    }

    public enum BuildBehaviourState
    {
        Start,
        Stop,
        Pause,
        Unpause
    }
}