using UnityEngine;
using System;

public class SpawnedObject : MonoBehaviour
{
    public event Action<SpawnedObject> Consumed;

    public void ProcessConsumption()
    {
        Consumed?.Invoke(this);
    }
}
