using System.Collections;
using BT;
using UnityEngine;

public class PlayAnimation : BTAction {
    private string _animationName;

    public PlayAnimation (string animationName, BTPrecondition precondition = null) : base (precondition) {
        _animationName = animationName;
    }

    protected override void Enter () {
        Animator animator = database.GetComponent<Animator> ();
        animator.Play (_animationName);
    }
}