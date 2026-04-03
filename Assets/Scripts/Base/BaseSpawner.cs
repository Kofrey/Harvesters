using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Transform _baseStockPoint;

    private bool _isSpawning = true;

    public bool IsSpawning => _isSpawning;

    public Unit GetUnit()
    { 
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Count - 1);
        Unit unit = Instantiate(_unitPrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
        unit.SetBase(_baseStockPoint);
        return unit;
    }

    public void SpawningOnOff()
    {
        _isSpawning = !_isSpawning;
    }
}
