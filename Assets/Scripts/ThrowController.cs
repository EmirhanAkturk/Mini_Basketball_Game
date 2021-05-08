﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ThrowController : MonoBehaviour
{

    [SerializeField] // todo assign with unity event
    Rigidbody ballRb;

    [SerializeField]
    float swipeLimitDistance;

    [Header("Throw Forces")]
    [SerializeField]
    float throwForceInXandY;

    [SerializeField]
    float throwForceInZ;

    Vector2 startTouchPosition, endTouchPosition, swipeDirection; //touch start posisiton, end position and swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // To calculate the throw force on the Z axis.

    float forceX, forceY, forceZ;

    // Start is called before the first frame update
    void Start()
    {
        // todo make with unity event
        ballRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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

        forceX = -swipeDirection.x * throwForceInXandY / 2;
        forceY = -swipeDirection.y * throwForceInXandY;
        forceZ = throwForceInZ / timeInterval;

        Debug.Log(forceX + ", " + forceY + ", " + forceZ);

        if(ballRb != null) 
        { 
            ballRb.isKinematic = false;
            ballRb.AddForce(forceX, forceY, forceZ);
        }

        ballRb = null;
        
        // todo add to ball pool
        // Destroy ball in 3 second
        Destroy(gameObject, 3f);
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
}
