using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculate : MonoBehaviour
{
    // Instance
    public static ScoreCalculate instance;

    // Reference
    [SerializeField] private List<BallControl> ballControlList;

    // Calculate score
    private int playerScore;
    private int perfectStreak;

    // Event
    public event Action perfectScore;
    public event Action<int> score;
    public event Action<int> onFire;

    // Ball reference control
    private void GetBallEvents()
    {
        for (int i = 0; i < ballControlList.Count; i++)
        {
            ballControlList[i].scoreGoal += ScoreGoal;
            ballControlList[i].missGoal += MissGoal;
        }
    }

    // Checking the ball state
    private void ScoreGoal(ScoreGoalEventArgs scoreGoalEventArgs)
    {
        if (!scoreGoalEventArgs.hitRing)
        {
            PerfectScore();
            perfectStreak++;
            OnFire();
        }
        else
        {
            perfectStreak = 0;
        }
        playerScore++;
        Score();
    }
    private void MissGoal()
    {
        Debug.Log("Miss goal");
        perfectStreak = 0;
    }

    // Invoke Events
    private void PerfectScore()
    {
        perfectScore?.Invoke();
    }
    private void Score()
    {
        score?.Invoke(playerScore);
    }
    private void OnFire()
    {
        onFire?.Invoke(perfectStreak);
    }

    //
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        //
        playerScore = 0;
        perfectStreak = 0;
        GetBallEvents();
    }
}
