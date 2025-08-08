using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DunkBanner : MonoBehaviour
{
    // Animator
    private Animator animator;

    //
    private void OnPerfectScore()
    {
        animator.SetTrigger("perfectScore");
    }
    private void OnFire(int perfectStreak)
    {
        if (perfectStreak >= 3) animator.SetBool("onFire", true);
        else animator.SetBool("onFire", false);
    }

    //
    private void Start()
    {
        animator = GetComponent<Animator>();

        ScoreCalculate.instance.perfectScore += OnPerfectScore;
        ScoreCalculate.instance.onFire += OnFire;
    }
}
