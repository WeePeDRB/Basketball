using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBallParticle : MonoBehaviour
{
    // Perfect hit
    [SerializeField] private ParticleSystem perfectScoreParticle;

    // On fire
    [SerializeField] private ParticleSystem basketFireParticle;

    //
    private void OnPerfectScore()
    {
        perfectScoreParticle.Play();
    }


    //
    private void Start()
    {
        ScoreCalculate.instance.perfectScore += OnPerfectScore;
    }
}
