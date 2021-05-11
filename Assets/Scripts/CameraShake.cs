using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraShake : MonoBehaviour
{
    public delegate void ShakeAction();

    private void OnEnable()
    {
        BasketCheckher.ShakeListener += OnCameraShake;
    }

    private void OnDisable()
    {
        BasketCheckher.ShakeListener -= OnCameraShake;

    }

    private void OnCameraShake()
    {
        Camera.main.DOShakePosition(0.5f, 0.2f, fadeOut: true);
    }

}
