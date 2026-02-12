using System;
using UnityEngine;

public class BaseStockTrigger : MonoBehaviour
{
    public event Action<Unit> UnitEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
            UnitEnterTrigger?.Invoke(unit);
    }
}
