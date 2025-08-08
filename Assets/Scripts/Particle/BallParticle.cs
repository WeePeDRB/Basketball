using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticle : MonoBehaviour
{
    // 
    private bool isOnFire;
    [SerializeField] private ParticleSystem ballFireParticle;

    // On fire particle effect
    private void OnFire(int perfectStreak)
    {
        if (perfectStreak < 3)
        {
            if (!isOnFire) return;
            ballFireParticle.Stop();
            isOnFire = false;
            return;
        }
        if (isOnFire) return;
        ballFireParticle.Play();
        isOnFire = true;
    }

    //
    private void Start()
    {
        //
        isOnFire = false;

        ScoreCalculate.instance.onFire += OnFire;
    }
}
