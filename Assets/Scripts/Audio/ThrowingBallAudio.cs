using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBallAudio : MonoBehaviour
{
    // Reference
    [SerializeField] private List<BallControl> ballControlList;

    // Audio sources
    [SerializeField] private AudioSource ballCollide;
    [SerializeField] private AudioSource basketCollide;
    [SerializeField] private AudioSource basketPass;
    [SerializeField] private AudioSource basketPerfect;
    [SerializeField] private AudioSource cheer;

    // Ball reference control
    private void GetBallEvents()
    {
        for (int i = 0; i < ballControlList.Count; i++)
        {
            ballControlList[i].hitBarrier += BallCollide;
            ballControlList[i].hitBasketRing += BasketCollide;
        }
    }

    // Audios play
    private void BallCollide()
    {
        ballCollide.Play();
    }
    private void BasketCollide()
    {
        basketCollide.Play();
    }
    private void BasketPass(int score)
    {
        basketPass.Play();
    }
    private void BasketPerfect()
    {
        basketPerfect.Play();
    }
    private void Cheer()
    {
        cheer.Play();
    }

    //
    private void Start()
    {
        GetBallEvents();
        ScoreCalculate.instance.score += BasketPass;
        ScoreCalculate.instance.perfectScore += BasketPerfect;
        ScoreCalculate.instance.perfectScore += Cheer;
    }
}
