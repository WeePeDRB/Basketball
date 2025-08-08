using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreCalculate : MonoBehaviour
{
    // Text reference
    [SerializeField] private TextMesh score;

    //
    private void OnPlayerScore(int playerScore)
    {
        score.text = playerScore.ToString();
    }

    //
    private void Start()
    {
        ScoreCalculate.instance.score += OnPlayerScore;
    }
}
