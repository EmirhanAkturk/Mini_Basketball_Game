using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BallController : MonoBehaviour, IPooledBall
{
    public static event GameplayUIController.RemainingBallAction BallFallListener; // todo move ball controller

    private WaitForSeconds enableIsKinematicDelay;
    private Rigidbody rb;

    private float delayTime = 1.3f;
    private bool isThrowing;
    private bool isFall;

    public bool IsThrowing { get => isThrowing; set => isThrowing = value; }

    void OnCollisionEnter(Collision collision)
    {
        if(isThrowing && !isFall)
        {
            if (collision.gameObject.CompareTag("Plane") && isThrowing)
            {
                BallFallListener?.Invoke();
                Debug.Log("Ball falled down");
                isFall = true;
            }
        }
    }

    // Start is called before the first frame update
    public void OnBallSpawn()
    {
        rb = GetComponent<Rigidbody>();

        //To prevent rotation while assigning velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isThrowing = false;
        isFall = false;

        enableIsKinematicDelay = new WaitForSeconds(delayTime);
        StartCoroutine(EnableIsKinematic());
    }

    public void SetBallProperties(Ball ball)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.mass = ball.GetMass();
        GetComponent<Renderer>().material.color = ball.GetColor();
        transform.localScale = ball.GetSize();
    }

    public void AddBackToPool(float hideDelayTime)
    {
        WaitForSeconds hideDelay = new WaitForSeconds(hideDelayTime);

        StartCoroutine(HideGameObject(hideDelay));
    }

    private IEnumerator EnableIsKinematic()
    {
        yield return enableIsKinematicDelay;

        if (!isThrowing && !rb.isKinematic)
            rb.isKinematic = true;
    }

    public IEnumerator HideGameObject(WaitForSeconds hideDelay)
    {
        yield return hideDelay;

        if (!isFall) 
        {
            isFall = true;
            BallFallListener?.Invoke();
        }

        gameObject.SetActive(false);
    }

}
