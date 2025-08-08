using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimator : MonoBehaviour
{
    // Animator
    private Animator animator;

    // Animation
    private void OnHitBall(HitBallEventArgs hitBallEventArgs)
    {
        if (hitBallEventArgs.hitBall == transform)
        {
            animator.SetBool("isSelected", true);
        }
        else
        {
            animator.SetBool("isSelected", false);
        }
    }

    //
    private void Start()
    {
        //
        animator = GetComponent<Animator>();

        BallSelection.instance.onHitBall += OnHitBall;
    }
}
