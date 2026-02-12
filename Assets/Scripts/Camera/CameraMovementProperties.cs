using UnityEngine;
using System;

[Serializable]
public class CameraMovementProperties
{
    public Transform  Pivot;
    public float Speed = 1f;
    public float Smoothness = 0.2f;
}
