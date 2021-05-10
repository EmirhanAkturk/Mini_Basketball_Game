using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BallController : MonoBehaviour, IPooledBall
{
    private WaitForSeconds enableIsKinematicDelay;
    private Rigidbody rb;

    private float delayTime = 1.3f;
    private bool isThrowing;

    public bool IsThrowing{ get => isThrowing; set => isThrowing = value; }

    // Start is called before the first frame update
    public void OnBallSpawn()
    {
        rb = GetComponent<Rigidbody>();

        //To prevent rotation while assigning velocity
        rb.freezeRotation = true;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = false;

        isThrowing = false;
        
        enableIsKinematicDelay = new WaitForSeconds(delayTime);
        StartCoroutine(EnableIsKinematic());
    }

    public void SetBallProperties(Ball ball)
    {
        if(rb == null)
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

        gameObject.SetActive(false);
    }

}
