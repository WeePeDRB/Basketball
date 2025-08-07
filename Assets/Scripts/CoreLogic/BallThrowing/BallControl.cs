using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallControl : MonoBehaviour
{
    // Ball throwing
    private int fingerID;
    private bool isClicked;

    // Physics
    private Rigidbody rb;

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
            // Calculate ball landing position
            float maxHorizontalValue = 5;
            Vector3 movingDirection = new Vector3(direction.x, direction.y, 1).normalized;
            float newX = transform.position.x + movingDirection.x * maxHorizontalValue;

            Vector3 landingPosition = new Vector3(newX, 10f, -2.3f);

            // Simulate ball throwing
            transform.DOJump(landingPosition, 2f, 1, .5f);

            //
            isClicked = false;
            fingerID = -1;
        }
    }
    private void UpdatePosition()
    {
        if (isClicked && fingerID != -1)
        {
            // Get world position
            float distanceToBall = transform.position.z - Camera.main.transform.position.z;
            Vector3 inputPosition = new Vector3(Input.GetTouch(fingerID).position.x, Input.GetTouch(fingerID).position.y, distanceToBall);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

            //
            transform.position = worldPosition;
        }
    }

    private void Start()
    {
        // Initialize data
        rb = GetComponent<Rigidbody>();
        fingerID = -1;
        isClicked = false;

        InputControl.instance.onClickBall += OnClickBall;
        InputControl.instance.onReleaseBall += OnReleaseBall;
    }

    private void Update()
    {
        UpdatePosition();   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "barrier")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
