using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BallControl : MonoBehaviour
{
    // Ball throwing
    private int fingerID;
    private bool isClicked;
    [SerializeField] private float power;

    // Checking ball state
    private bool hitRing;
    private bool hitTop;
    private bool hitBottom;

    // Physics
    private Rigidbody rb;

    // Effects
    private TrailRenderer trailRenderer;

    // Events
    public event Action<ScoreGoalEventArgs> scoreGoal; // Invoke when player score a goal (used for logic / can be used for sound effect)
    public event Action missGoal; 
    public event Action hitBarrier; // Invoke when ball collide with barrier suround (used for sound effect)
    public event Action hitBasketRing; // Invoke when ball collide with the basket ring (used for sound effect)

    // Throwing ball
    private void OnClickBall(TouchingBallEventArgs touchingBallEventArgs)
    {
        if (touchingBallEventArgs.ball == transform)
        {
            fingerID = touchingBallEventArgs.fingerID;
            isClicked = true;
        }
    }
    private void OnReleaseBall(Vector2 direction)
    {
        if (isClicked)
        {
            // Using dotween to simulate the throw ball action

            // // Calculate ball landing position
            // float maxHorizontalValue = 5;
            // Vector3 movingDirection = new Vector3(direction.x, direction.y, 1).normalized;
            // float newX = transform.position.x + movingDirection.x * maxHorizontalValue;
            // Vector3 landingPosition = new Vector3(newX, 10f, -2.3f);
            // // Simulate ball throwing
            // transform.DOJump(landingPosition, 2f, 1, .5f);

            //
            trailRenderer.enabled = true;
            if (direction != Vector2.zero)
            {
                Vector3 forceVector = new Vector3(direction.x, direction.y * 0.78f, 0.57f) * power;
                rb.AddForce(forceVector, ForceMode.Impulse);
            }

            //
            isClicked = false;
            fingerID = -1;
        }
    }
    private void UpdatePosition()
    {
        if (isClicked && fingerID != -1)
        {
            //
            rb.velocity = Vector3.zero;

            // Get world position
            float distanceToBall = transform.position.z - Camera.main.transform.position.z;
            Vector3 inputPosition = new Vector3(Input.GetTouch(fingerID).position.x, Input.GetTouch(fingerID).position.y, distanceToBall);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

            //
            transform.position = worldPosition;
        }
    }

    // Checking goal state
    private void HitRing()
    {
        hitRing = true;
    }
    private void HitTop()
    {
        hitTop = true;
    }
    private void HitBottom()
    {
        hitBottom = true;
        if (hitTop)
        {
            scoreGoal?.Invoke(new ScoreGoalEventArgs { hitRing = hitRing, hitTop = hitTop, hitBottom = hitBottom });
        }
    }
    private void MissGoal()
    {
        Debug.Log("Invoke miss goal");
        missGoal?.Invoke();
    }

    private void ResetState()
    {
        hitRing = false;
        hitTop = false;
        hitBottom = false;
    }

    //
    private void Start()
    {
        // Initialize data
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        fingerID = -1;
        isClicked = false;

        InputControl.instance.onClickBall += OnClickBall;
        InputControl.instance.onReleaseBall += OnReleaseBall;
    }
    private void Update()
    {
        UpdatePosition();
    }

    //
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "barrierReset")
        {
            rb.velocity = Vector3.zero;
            trailRenderer.enabled = false;
        }
        if (collision.transform.tag == "basketRing")
        {
            HitRing();
            hitBasketRing?.Invoke();
        }
        if (collision.transform.tag == "barrierBack")
        {
            Debug.Log("Hit back");
            MissGoal();
            hitBarrier?.Invoke();
        }
        if (collision.transform.tag == "barrierBottom")
        {
            ResetState();
            hitBarrier?.Invoke();
        }
        if (collision.transform.tag == "barrier")
        {

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "dunkTop")
        {
            HitTop();
        }
        if (collider.transform.tag == "dunkBottom")
        {
            HitBottom();
        }
    }
}

public class ScoreGoalEventArgs : EventArgs
{
    public bool hitRing;
    public bool hitTop;
    public bool hitBottom;
}