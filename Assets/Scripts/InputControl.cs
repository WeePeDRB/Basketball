using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InputControl : MonoBehaviour
{
    //
    private int fingerID;
    private bool lockInput;
    private LayerMask ballLayerMask;
    private Transform ball;

    private Queue<Vector2> positionTrackingQueue;
    private int maxFrames;

    [SerializeField] Transform endPos;

    //
    private void IsTouchingBall()
    {
        // Check if there any finger touch the screen
        if (Input.touchCount > 0 && !lockInput)
        {
            Touch touch = Input.GetTouch(0);
            // Check if player just touch the screen
            if (touch.phase == TouchPhase.Began)
            {
                // If yes check if player touch the ball to lock input
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 20f, ballLayerMask))
                {
                    if (hit.collider.tag == "ballTag")
                    {
                        // Get the finger id and block input (Ensure only one ball can be touched at a time.)
                        fingerID = touch.fingerId;
                        lockInput = true;
                        ball = hit.transform;
                    }
                }
            }
        }
    }

    //
    private void IsTouchRealeased()
    {
        if (lockInput)
        {
            // Check if the finger holding the ball has been released.
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                // Realease the block input if the touch end
                if (touch.fingerId == fingerID && touch.phase == TouchPhase.Ended)
                {
                    // Get ball direction
                    Vector2 xyDirection = CalculateDirection(touch.position);
                    Vector3 direction = new Vector3(xyDirection.x, xyDirection.y, 1).normalized;

                    // Calculate end postion
                    float newX = ball.position.x + direction.x * 5;
                    Vector3 jumpTarget = new Vector3(newX, endPos.position.y, endPos.position.z);

                    // Lock input
                    lockInput = false;
                    fingerID = -1;

                    //
                    ball.DOJump(jumpTarget, 2f, 1, .5f);

                    //
                    ball = null;

                    //
                    positionTrackingQueue.Clear();
                }
            }
        }
    }

    //
    private void UpdateTouchPos()
    {
        if (lockInput)
        {
            // Calculate distance between picked ball and camera
            float distanceToBall = ball.position.z - Camera.main.transform.position.z;

            // Get world position
            Vector3 inputPosition = new Vector3(Input.GetTouch(fingerID).position.x, Input.GetTouch(fingerID).position.y, distanceToBall);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

            // Update ball position
            ball.transform.position = worldPosition;
        }
    }

    // Calculate the ball's throw direction
    private void TrackingFingerPosition()
    {
        if (fingerID != -1)
        {
            // Get the finger position on screen and convert it to XY axes
            Vector3 inputPosition = Input.GetTouch(fingerID).position;
            Vector2 inputDirection = new Vector2(inputPosition.x, inputPosition.y);

            // Enqueue the value
            positionTrackingQueue.Enqueue(inputDirection);

            // Dequeue when reach max value
            if (positionTrackingQueue.Count > maxFrames) positionTrackingQueue.Dequeue();
        }
    }
    private Vector2 CalculateDirection(Vector3 phaseEndPostion)
    {
        // Get the start and end postion
        Vector2 endPostion = new Vector2(phaseEndPostion.x, phaseEndPostion.y);
        Vector2 startPostion = positionTrackingQueue.Dequeue();

        // Calculate the direction
        Vector2 direction = (endPostion - startPostion).normalized;

        return direction;
    }

    //
    private void Awake()
    {
        ballLayerMask = LayerMask.GetMask("Ball");
        fingerID = -1;
        maxFrames = 4;
        positionTrackingQueue = new Queue<Vector2>();
    }

    private void Update()
    {
        // Checking if player touching the ball
        IsTouchingBall();

            // Update the ball position due to player input
            UpdateTouchPos();
            // Tracking the finger position
            TrackingFingerPosition();

        // Checking if player release the ball 
        IsTouchRealeased();
    }
}
