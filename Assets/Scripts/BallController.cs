using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BallController : MonoBehaviour
{
    private WaitForSeconds delay;
    private Rigidbody rb;

    private float delayTime = 1.3f;
    private bool isThrowing;

    public bool IsThrowing{ get => isThrowing; set => isThrowing = value; }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        delay = new WaitForSeconds(delayTime);
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

    private IEnumerator EnableIsKinematic()
    {
        yield return delay;

        if (!isThrowing && !rb.isKinematic)
            rb.isKinematic = true;
    }

}
