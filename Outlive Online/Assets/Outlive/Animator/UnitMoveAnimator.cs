using System.Collections;
using System.Collections.Generic;
using Outlive.Unit;
using Outlive.Unit.Behaviour;
using UnityEditor.Animations;
using UnityEngine;
using Outlive.Behaviours;
using UnityEngine.AI;

namespace Outlive.Animator
{
    public class UnitMoveAnimator : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator _animator;
        private float velocity;
        private bool _updateBodyVelocity;
        private bool _isMoving;

        private void Update() 
        {
            velocity = GetComponent<NavMeshAgent>().velocity.magnitude;

            _animator.SetFloat("Velocity", Mathf.Max(velocity, 1));
            // _animator.SetBool("IsBodyMoving", _isMoving);
            // _animator.SetBool("isMoving", velocity > 1f);
            // _animator.SetBool("IsBodyMoving", velocity > 1f? _updateBodyVelocity: false);

            // if (_updateBodyVelocity)
            // {
            //     _animator.SetFloat("BodyVelocity", velocity);
            // }
            // Debug.Log(velocity);

        }



        public void OnMoveState(MoveCallback ctx)
        {
            _isMoving = ctx.State == MoveBehaviourState.Moving;
            // Debug.Log(ctx.State);
            // bool state = ctx.State == MoveBehaviourState.Moving;

            // _updateBodyVelocity = state;
            _animator.SetBool("IsBodyMoving", _isMoving);
            _animator.SetBool("Roda", _isMoving);

            // if (!state)
            //     _animator.SetFloat("BodyVelocity", 3.5f);

            // _animator.SetBool("isMoving", state);
        }
        public void OnBuildState(BuildCallback ctx)
        {
            Debug.Log("OnBuildState");
            if (ctx.State == BuildBehaviourState.Start)
                _animator.SetBool("isBuilding", true);

            if (ctx.State == BuildBehaviourState.Stop)
                _animator.SetBool("isBuilding", false);
        }

        public void OnMoveStop()
        {

        }
    }
}