using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball", menuName = "Scriptable Objects/Ball")]
public class Ball : ScriptableObject
{
    [Header("Ball properties")]

    [SerializeField]
    float mass;
    
    [SerializeField]
    Color color;

    [SerializeField]
    Vector3 size;


    public float GetMass()
    {
        return mass;
    }

    public Color GetColor()
    {
        return color;
    }

    public Vector3 GetSize()
    {
        return size;
    }

}
