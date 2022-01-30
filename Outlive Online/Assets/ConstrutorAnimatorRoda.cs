using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrutorAnimatorRoda : StateMachineBehaviour
{
    private ConstrutorAnimState reference;
    [SerializeField] bool _isMarchaRe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (reference == null)
            reference = animator.gameObject.GetComponent<ConstrutorAnimState>();
        if (_isMarchaRe)
            animator.Play(0, layerIndex, reference.rodaDianteiraReNormalizedTime);
        else
            animator.Play(0, layerIndex, reference.rodaDianteiraNormalizedTime);
        // count = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1f)
            animator.Play(0, layerIndex, 0f);

        // if (rodaState != _lastState)
        // {
        //     if (rodaState)
        //         animator.StartPlayback();
        //     else
        //         animator.StopPlayback();
        //     _lastState = rodaState;
        // }
        // animator.play
        // stateInfo.spee
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.GetParameter()
        if (_isMarchaRe)
        {
            reference.rodaDianteiraNormalizedTime = 1f - stateInfo.normalizedTime;
            reference.rodaDianteiraReNormalizedTime = stateInfo.normalizedTime;
        }
        else
        {
            reference.rodaDianteiraNormalizedTime = stateInfo.normalizedTime;
            reference.rodaDianteiraReNormalizedTime = 1f - stateInfo.normalizedTime;
        }
        // animator.SetFloat(_paramName, _normalizedTime);
        // animator.
        // time = animator.
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
