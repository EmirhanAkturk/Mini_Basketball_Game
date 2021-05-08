using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ThrowController : MonoBehaviour
{
    public delegate void ThrowContollerActions(Rigidbody ballRb);

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
    float maxForceX, maxForceY, maxForceZ;
    
    [SerializeField]
    float minForceY, minForceZ;

    private Rigidbody ballRb;

    private float swipeLimitDistance = 50;
    private Vector2 startTouchPosition, endTouchPosition, swipeDirection; //touch start posisiton, end position and swipe direction
    private float touchTimeStart, touchTimeFinish, timeInterval; // To calculate the throw force on the Z axis.

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
        TounchControl();
    }
    
    
    private void TounchControl()
    {
        #if UNITY_ANDROID
            #region Android touch controller

            // if screen was touched
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Get touch start time and position information.
                startTouchPosition = Input.GetTouch(0).position;
                touchTimeStart = Time.time;
            }
            // if touching the screen is over
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // Get touch end time and position information.
                endTouchPosition = Input.GetTouch(0).position;
                touchTimeFinish = Time.time;

                // Calculate swipe time interval
                timeInterval = touchTimeFinish - touchTimeStart;

                // Calculate swipe direction
                swipeDirection = startTouchPosition - endTouchPosition;

                if (CheckSwipeDistance())
                {
                    ThrowBall();
                }
            }

            #endregion
        #endif

        #if UNITY_EDITOR
            #region Unity editor touch controler

            // if screen was touched
            if (Input.GetMouseButtonDown(0))
            {
                if(ballRb != null)
                    ballRb.isKinematic = true;    

                // Get touch start time and position information.
                startTouchPosition = Input.mousePosition;
                touchTimeStart = Time.time;
            }
            // if touching the screen is over
            else if (Input.GetMouseButtonUp(0))
            {
                // Get touch end time and position information.
                endTouchPosition = Input.mousePosition;
                touchTimeFinish = Time.time;

                // Calculate swipe time interval
                timeInterval = touchTimeFinish - touchTimeStart;

                // Calculate swipe direction
                swipeDirection = startTouchPosition - endTouchPosition;

                if (CheckSwipeDistance())
                {
                    ThrowBall();
                }

            }

            #endregion
        #endif
    }

    private bool CheckSwipeDistance()
    {
        float horizontalDistance = Mathf.Abs(endTouchPosition.x - startTouchPosition.x);
        float verticleDistance = Mathf.Abs(endTouchPosition.y - startTouchPosition.y);

        // horizontal swipe
        if (horizontalDistance > verticleDistance)
        {
            if (horizontalDistance >= GetLimitSwipeDistance(false, swipeLimitDistance))
                return true;
            else
                return false;
        }
        else
        {
            if (verticleDistance >= GetLimitSwipeDistance(true, swipeLimitDistance))
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
            forceY = Mathf.Clamp(-swipeDirection.y * throwForceInY, minForceY, maxForceY);
            forceZ = Mathf.Clamp(throwForceInZ /*(3 * throwForceInZ) / (2 * timeInterval)*/, minForceZ, maxForceZ);
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
        }
    }

    private float GetLimitSwipeDistance(bool isVerticle, float distance)
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
        forceZ = maxForceZ;

        ThrowBall();
    }    
    
    public void MinForceThrow()
    {
        forceY = minForceY;
        forceZ = minForceZ;

        ThrowBall();
    }
}
