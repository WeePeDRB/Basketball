using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSelection : MonoBehaviour
{
    // Instance
    public static BallSelection instance;

    // Select ball
    private Transform selectedBall;

    // Events
    public event Action onOpenMenu;
    public event Action<HitBallEventArgs> onHitBall;
    public event Action<SelectBallEventArgs> onSelectBall;
    public event Action onCloseMenu;


    // Invoke events
    public void OpenMenu()
    {
        onOpenMenu?.Invoke();
    }
    public void CloseMenu()
    {
        onCloseMenu?.Invoke();
    }
    public void SelectBall()
    {
        if (selectedBall != null)
        {
            MeshRenderer meshRenderer = selectedBall.GetComponentInChildren<MeshRenderer>();
            onSelectBall?.Invoke(new SelectBallEventArgs {meshRenderer = meshRenderer });
            CloseMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    // Hit ball
    public void HitBall(Transform ball)
    {
        Debug.Log("Hit the ball ");
        selectedBall = ball;
        onHitBall?.Invoke(new HitBallEventArgs { hitBall = ball });
    }

    private void Awake()
    {
        //
        if (instance == null) instance = this;
        else Destroy(this);
    }
}

public class HitBallEventArgs : EventArgs
{
    public Transform hitBall;
}

public class SelectBallEventArgs : EventArgs
{
    public MeshRenderer meshRenderer;
}