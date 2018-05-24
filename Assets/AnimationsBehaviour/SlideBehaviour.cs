﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : StateMachineBehaviour {

    private Vector2 slideSize = new Vector2(1.45f, 1.8f);
    private Vector2 offsetSize = new Vector2(0, -1.29f);

    private Vector2 size;
    private Vector2 offset;

    private BoxCollider2D boxCollider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PlayerController.Instance.Slide = true;

        if(boxCollider == null) {
            boxCollider = PlayerController.Instance.GetComponent<BoxCollider2D>();
            size = boxCollider.size;
            offset = boxCollider.offset;
        }

        boxCollider.size = slideSize;
        boxCollider.offset = offsetSize;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        PlayerController.Instance.Slide = false;
        animator.ResetTrigger("slide");
        boxCollider.size = size;
        boxCollider.offset = offset;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
