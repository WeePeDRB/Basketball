using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class InputControl : MonoBehaviour
{
    // Instance
    public static InputControl instance;

    // Tracking finger movement
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
    }

    // Touching ball values
    private int fingerID;
    private bool lockInput;
    private LayerMask ballLayerMask;
    private Transform ball;

    // Tracking touch position values
    private Queue<Vector2> positionTrackingQueue;
    private int maxFrames;
    

    // Select ball
    private bool isSelectBall;

    // Events
    public event Action<TouchingBallEventArgs> onClickBall;
    public event Action<Vector2> onReleaseBall;

    // Coroutine
    private Coroutine resetDirection;

    // Functions that help track the player's finger position
    private void IsTouchingScreen()
    {
        // Check if there any finger touch the screen
        if (Input.touchCount > 0 && !lockInput)
        {
            Touch touch = Input.GetTouch(0);
            // Check if player just touch the screen
            if (touch.phase == TouchPhase.Began)
            {
                // Lock player input
                lockInput = true;
                fingerID = touch.fingerId;

                if (!isSelectBall)
                {
                    // Check if player click the ball
                    IsTouchingBall(touch);
                }
            }
        }
    }
    private void IsTouchRealeased()
    {
        if (lockInput)
        {
            Touch touch = Input.GetTouch(fingerID);
            // Realease the block input if the touch end
            if (touch.fingerId == fingerID && touch.phase == TouchPhase.Ended)
            {
                if (ball != null)
                {
                    // Get ball direction
                    onReleaseBall?.Invoke(direction);

                    //
                    ball = null;
                }

                // Lock input
                lockInput = false;
                fingerID = -1;

                //
                if (resetDirection == null) resetDirection = StartCoroutine(ResetDirection());
            }
        }
    }
    private void TrackingFingerPosition()
    {
        if (lockInput)
        {
            // Get the finger position on screen and convert it to XY axes
            Vector2 inputPosition = Input.GetTouch(fingerID).position;

            // Enqueue the value
            positionTrackingQueue.Enqueue(inputPosition);

            // Dequeue when reach max value
            if (positionTrackingQueue.Count > maxFrames) positionTrackingQueue.Dequeue();
        }
    }
    private void CalculateDirection()
    {
        if (lockInput && positionTrackingQueue.Count >= 2)
        {
            // Get the start and end postion
            Vector2 endPostion = positionTrackingQueue.Last();
            Vector2 startPostion = positionTrackingQueue.Peek();

            // Calculate the direction
            direction = (endPostion - startPostion).normalized;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    // Functions that are triggered when an event occurs
    private void IsTouchingBall(Touch touch)
    {
        // Create a ray due to the touch position
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f, ballLayerMask))
        {
            if (hit.collider.tag == "ballTag")
            {
                ball = hit.transform;
                // Invoke envent with data
                onClickBall?.Invoke(new TouchingBallEventArgs { fingerID = fingerID, ball = ball });
            }
        }
    }
    private void OnOpenMenu()
    {
        isSelectBall = true;
    }
    private void OnCloseMenu()
    {
        isSelectBall = false;
    }

    // Coroutine
    private IEnumerator ResetDirection()
    {
        yield return new WaitForSeconds(0.5f);
        positionTrackingQueue.Clear();
    }

    //
    private void Awake()
    {
        //
        if (instance == null) instance = this;
        else Destroy(this);

        // Initialize datas
        ballLayerMask = LayerMask.GetMask("Ball");
        fingerID = -1;
        maxFrames = 4;
        positionTrackingQueue = new Queue<Vector2>();
        isSelectBall = false;
    }

    private void Start()
    {
        //
        BallSelection.instance.onOpenMenu += OnOpenMenu;
        BallSelection.instance.onCloseMenu += OnCloseMenu;
    }

    private void Update()
    {
        // Checking if player touching screen
        IsTouchingScreen();

        // Tracking the finger position
        TrackingFingerPosition();

        //
        CalculateDirection();

        // Checking if player release the ball 
        IsTouchRealeased();
    }
}

public class TouchingBallEventArgs : EventArgs
{
    public int fingerID;
    public Transform ball;
}