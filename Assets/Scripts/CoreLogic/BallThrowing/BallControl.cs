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

    // Interact with the ball based on player input
    private void OnClickBall(TouchingBallEventArgs touchingBallEventArgs)
    {
        // Check and lock onto the ball currently being interacted with by the player
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
            trailRenderer.enabled = true;
            // Checking and add fore to the ball
            if (direction != Vector2.zero)
            {
                float yOffset = 0.78f;
                float zOffset = 0.57f;
                Vector3 forceVector = new Vector3(direction.x, direction.y * yOffset, 1 * zOffset) * power;
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
            // Disable velocity to prevent strange glitches
            rb.velocity = Vector3.zero;

            // Get world position
            float distanceToBall = transform.position.z - Camera.main.transform.position.z;
            Vector3 inputPosition = new Vector3(Input.GetTouch(fingerID).position.x, Input.GetTouch(fingerID).position.y, distanceToBall);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

            // Update the ball position 
            transform.position = worldPosition;
        }
    }

    // Checking goal state based on the collision 
    private void HitRing()
    {
        // Trigger when hit the basket ring
        hitRing = true;
    }
    private void HitTop()
    {
        // Trigger when hit collider of the top dunk
        hitTop = true;
    }
    private void HitBottom()
    {
        // Trigger when hit collider of the bottom dunk
        hitBottom = true;

        // Checking for the top dunk before invoke the score event
        if (hitTop)
        {
            scoreGoal?.Invoke(new ScoreGoalEventArgs { hitRing = hitRing, hitTop = hitTop, hitBottom = hitBottom });
        }
    }
    private void CheckingMissGoal()
    {
        // Triggered every time the ball
        if (hitTop && hitBottom) return;
        missGoal?.Invoke();
    }
    private void ResetState()
    {
        // Trigger when the ball returns to the neutral zone (reset hit state flags)
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

        // Event subscription
        InputControl.instance.onClickBall += OnClickBall;
        InputControl.instance.onReleaseBall += OnReleaseBall;
    }
    private void Update()
    {
        UpdatePosition();
    }

    // Collision detect
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "barrierReset")
        {
            rb.velocity = Vector3.zero;
            trailRenderer.enabled = false;
            ResetState();
        }
        if (collision.transform.tag == "basketRing")
        {
            HitRing();
            hitBasketRing?.Invoke();
        }
        if (collision.transform.tag == "barrier")
        {
            hitBarrier?.Invoke();
        }
    }

    // Collider detect
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
        if (collider.transform.tag == "checkingGoal")
        {
            Debug.Log("Checking");
            CheckingMissGoal();
        }
    }
}

public class ScoreGoalEventArgs : EventArgs
{
    public bool hitRing;
    public bool hitTop;
    public bool hitBottom;
}