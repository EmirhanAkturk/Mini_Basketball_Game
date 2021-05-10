using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ThrowController : MonoBehaviour
{
    // Unity Events
    public delegate void ThrowContollerAction(Rigidbody ballRb);
    public static event LevelController.CreateBallAction BallThrowListener1;
    public static event GameplayUIController.RemainingBallAction BallThrowListener2;

    float throwForceInX = 0.25f, throwForceInY = 0.5f, throwForceInZ = 150; 

    float maxForceX = 100, maxForceY = 650, minForceY = 300;

    [Header("Throw Forces")]
    [SerializeField]
    //It adds to the ejection force arising from the swiping.
    float defaultForce = 75;

    [SerializeField]
    Rigidbody ballRb;

    //It adds to the ejection force arising from the swiping.
    private Dictionary<int, int> defaultForces;
    
    //touch start posisiton, end position and swipe direction
    private Vector2 startTouchPosition, endTouchPosition, swipeDirection; 

    private float swipeLimitDistance = 50;
    private float forceX, forceY, forceZ;
    private float delayTime = 4;

    private void OnEnable()
    {
        LevelController.BallCreateListener += OnBallCreateListener;
    }

    private void OnDisable()
    {
        LevelController.BallCreateListener -= OnBallCreateListener;
    }

    private void OnBallCreateListener(Rigidbody newBallRb)
    {
        ballRb = newBallRb;
    }

    private void Start()
    {
        InitializeDictionary();

        int levelNumber = LevelController.Instance.GetLevelNumber();

        defaultForce = defaultForces[levelNumber];
    }

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Instance.IsPlaying)
            TounchControl();
    }

    private void InitializeDictionary()
    {
        //It adds to the ejection force arising from the swiping.
        defaultForces = new Dictionary<int, int>()
        {
            { 1, 0 }, { 2, 0 }, { 3, 90 }, { 4, 70  }, { 5, 110 },
            { 6, 110 }, { 7, 225 }, { 8, 170 }, { 9, 210 }, { 10, 210 }
        };
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

        forceX = Mathf.Clamp(-swipeDirection.x * throwForceInX, -maxForceX, maxForceX);
        forceY = defaultForce + Mathf.Clamp(-swipeDirection.y * throwForceInY, minForceY, maxForceY);
        forceZ = defaultForce + throwForceInZ;

        //Debug.Log(forceX + ", " + forceY + ", " + forceZ);

        if (ballRb != null)
        {
            GameManager.Instance.IsThrowingBallExist = false;

            ballRb.isKinematic = false;

            ballRb.AddForce(forceX, forceY, forceZ);
            ballRb.AddTorque(Vector3.right * forceY);


            BallController ballController = ballRb.gameObject.GetComponent<BallController>();

            ballController.AddBackToPool(delayTime);


            ballRb = null;

            BallThrowListener1?.Invoke();
            BallThrowListener2?.Invoke();
        }
    }

    public IEnumerator HideBall(GameObject ball, WaitForSeconds hideDelay)
    {
        yield return hideDelay;

        ball.SetActive(false);
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
}
