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
    private int perfectStreak; // Store the player perfect streak

    // Events
    public event Action perfectScore;
    public event Action<int> score;
    public event Action<int> onFire;

    // Subscribe to ball's events
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
        // It's a perfect score when not hit the ring 
        if (!scoreGoalEventArgs.hitRing)
        {
            // Trigger the perfect score event
            PerfectScore();

            // Increase the streak and invoke a special effect event 
            perfectStreak++;
            OnFire();
        }
        else
        {
            perfectStreak = 0;
        }

        // Calcualate player score
        playerScore++;
        Score();
    }

    // Handle when player miss a goal
    private void MissGoal()
    {
        perfectStreak = 0;
        OnFire();
    }

    // Invoke Events
    private void PerfectScore()
    {
        // Triggered when player hit a perfect goal
        perfectScore?.Invoke();
    }
    private void Score()
    {
        // Trigger when player score a goal
        score?.Invoke(playerScore);
    }
    private void OnFire()
    {
        // Trigger when player hit a perfect goal 
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
