using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ThrowController : MonoBehaviour
{
    // Unity Events
    public delegate void ThrowContollerActions(Rigidbody ballRb);
    public static event LevelController.LevelControllerAcions ballThrowListener;


    [SerializeField]
    bool isMaxForce, isAutoThrow;

    [Header("Throw Forces")]
    [SerializeField]
    float throwForceInX;

    [SerializeField]
    float throwForceInY;

    [SerializeField]
    float throwForceInZ;

    [SerializeField]
    float maxForceX, maxForceY/*, maxForceZ*/;

    [SerializeField]
    float defaultForce;

    [SerializeField]
    float minForceY/*, minForceZ*/;

    [SerializeField]
    Rigidbody ballRb;

    private float swipeLimitDistance = 50;
    private Vector2 startTouchPosition, endTouchPosition, swipeDirection; //touch start posisiton, end position and swipe direction

    private float forceX, forceY, forceZ;

    private void OnEnable()
    {
        LevelController.ballCreateListener += OnBallCreateListener;
    }

    private void OnDisable()
    {
        LevelController.ballCreateListener -= OnBallCreateListener;
    }

    private void OnBallCreateListener(Rigidbody newBallRb)
    {
        ballRb = newBallRb;
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Instance.IsPlaying)
            TounchControl();
    }

    private void TounchControl()
    {
        // if screen was touched
        if (Input.GetMouseButtonDown(0))
        {
            if (ballRb != null)
            {
                ballRb.isKinematic = true;
                BallController controller = ballRb.gameObject.GetComponent<BallController>();
                controller.IsThrowing = true;
            }
            // Get touch start time and position information.
            startTouchPosition = Input.mousePosition;
        }
        // if touching the screen is over     //startTouchPosition condition is added for the moment the start button is pressed.
        else if (Input.GetMouseButtonUp(0) && startTouchPosition != Vector2.zero)
        {
            // Get touch end time and position information.
            endTouchPosition = Input.mousePosition;

            // Calculate swipe direction
            swipeDirection = startTouchPosition - endTouchPosition;

            swipeDirection.x = GetProportionSwipeDistance(false, swipeDirection.x);
            swipeDirection.y = GetProportionSwipeDistance(true, swipeDirection.y);

            if (CheckSwipeDistance())
            {
                ThrowBall();
            }

        }

    }

    private bool CheckSwipeDistance()
    {
        float horizontalDistance = Mathf.Abs(endTouchPosition.x - startTouchPosition.x);
        float verticleDistance = Mathf.Abs(endTouchPosition.y - startTouchPosition.y);

        // horizontal swipe
        if (horizontalDistance > verticleDistance)
        {
            if (horizontalDistance >= GetProportionSwipeDistance(false, swipeLimitDistance))
                return true;
            else
                return false;
        }
        else
        {
            if (verticleDistance >= GetProportionSwipeDistance(true, swipeLimitDistance))
                return true;
            else
                return false;
        }
    }

    private void ThrowBall()
    {
        // add force to balls rigidbody in 3D space depending on swipe time, direction, throw forces

        if (!isAutoThrow)
        {
            forceX = Mathf.Clamp(-swipeDirection.x * throwForceInX, -maxForceX, maxForceX);
            forceY = defaultForce + Mathf.Clamp(-swipeDirection.y * throwForceInY, minForceY, maxForceY);
            forceZ = defaultForce + throwForceInZ;
        }

        Debug.Log(forceX + ", " + forceY + ", " + forceZ);

        if (ballRb != null)
        {
            GameManager.Instance.IsThrowingBallExist = false;

            ballRb.isKinematic = false;
            ballRb.AddForce(forceX, forceY, forceZ);

            // todo add to ball pool
            // Destroy ball in 3 second
            Destroy(ballRb.gameObject, 5f);

            ballRb = null;

            ballThrowListener?.Invoke();
        }
        else
        {
            Debug.Log("ammar");
        }
    }

    private float GetProportionSwipeDistance(bool isVerticle, float distance)
    {
        float referanceWidth = 1080;
        float referanceHeight = 1920;

        float thisScreenWidth = Screen.width;
        float thisScreenHeight = Screen.height;

        float limitDistance;

        if (isVerticle)
            limitDistance = distance * referanceHeight / thisScreenHeight;

        else
            limitDistance = distance * referanceWidth / thisScreenWidth;

        return limitDistance;
    }

    public void AutoThrow()
    {
        if (isMaxForce)
            MaxForceThrow();
        else
            MinForceThrow();
    }

    public void MaxForceThrow()
    {
        forceY = maxForceY;
        //forceZ = maxForceZ;

        ThrowBall();
    }

    public void MinForceThrow()
    {
        forceY = minForceY;
        //forceZ = minForceZ;

        ThrowBall();
    }
}
