using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketParticle : MonoBehaviour
{
    // Perfect hit
    [SerializeField] private ParticleSystem basketPerfectScoreParticle;

    // On fire
    private bool isOnFire;
    [SerializeField] private ParticleSystem basketFireParticle;

    // Trigger the perfect score particle
    private void OnPerfectScore()
    {
        basketPerfectScoreParticle.Play();
    }

    // Trigger the perfect streak particle
    private void OnFire(int perfectStreak)
    {
        if (perfectStreak < 3)
        {
            if (!isOnFire) return;
            basketFireParticle.Stop();
            isOnFire = false;
            return;
        }
        if (isOnFire) return;
        basketFireParticle.Play();
        isOnFire = true;
    }

    //
    private void Start()
    {
        // Initialize data
        isOnFire = false;

        // Event subscription
        ScoreCalculate.instance.perfectScore += OnPerfectScore;
        ScoreCalculate.instance.onFire += OnFire;
    }
}
