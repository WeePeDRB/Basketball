using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWheel : MonoBehaviour
{
    // Spinning wheel values
    [SerializeField] private GameObject ballWheel;
    private float rotateAccelerate;
    private float rotateDeccelerate;
    private bool isOpenMenu;
    private float currentRotate;

    // Select ball in wheel
    private Ray checkingRay;
    private float rayLength;
    private bool isHitBall;

    // Rotate the wheel
    private void RotateWheel()
    {
        if (isOpenMenu)
        {
            // Get direction
            Vector2 direction = InputControl.instance.Direction;

            // Calculate the rotate speed
            float rotateSpeed = direction.x * rotateAccelerate;

            // Ensures smooth rotation 
            currentRotate = Mathf.Lerp(currentRotate, rotateSpeed, rotateDeccelerate * Time.deltaTime);

            ballWheel.transform.Rotate(Vector3.up, -currentRotate * Time.deltaTime);
        }
    }

    // Select ball
    private void CheckingBall()
    {
        if (isOpenMenu)
        {
            // Checking if ray hit object
            if (Physics.Raycast(checkingRay, out RaycastHit hit, rayLength))
            {
                // If hit the ball invoke an event that animate the ball
                if (hit.transform.tag == "selectBall")
                {
                    if (!isHitBall)
                    {
                        isHitBall = true;
                        BallSelection.instance.HitBall(hit.transform);
                    }
                    else return;
                }
            }
            else
            {
                BallSelection.instance.HitBall(null);
                isHitBall = false;
            }
            Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
        }
    }

    // Functions that are triggered when an event occurs
    private void OnOpenMenu()
    {
        isOpenMenu = true;
    }
    private void OnCloseMenu()
    {
        isOpenMenu = false;

        isHitBall = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize datas
        rotateAccelerate = 150;
        rotateDeccelerate = 3f;

        checkingRay = new Ray(transform.position, transform.forward);
        rayLength = 6f;
        isHitBall = false;

        BallSelection.instance.onOpenMenu += OnOpenMenu;
        BallSelection.instance.onCloseMenu += OnCloseMenu;
    }

    // Update is called once per frame
    void Update()
    {
        RotateWheel();
        CheckingBall();
    }
}
