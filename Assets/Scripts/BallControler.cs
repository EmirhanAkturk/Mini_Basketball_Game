﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BallControler : MonoBehaviour
{

    private WaitForSeconds delay;
    private Rigidbody rb;

    private float delayTime = 1.7f;
    private bool isThrowing;

    public bool IsThrowing{ get => isThrowing; set => isThrowing = value; }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        delay = new WaitForSeconds(delayTime);
        StartCoroutine(EnableIsKinematic());
    }

    private IEnumerator EnableIsKinematic()
    {
        yield return delay;

        if (!isThrowing && !rb.isKinematic)
            rb.isKinematic = true;
    }

}
