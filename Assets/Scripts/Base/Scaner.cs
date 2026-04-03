using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Scaner : MonoBehaviour
{
    [SerializeField] private ResourcesDataBase _dataBase;
    [SerializeField] private float _scanTimer = 3f;
    [SerializeField] private float _scanRadius = 45f;
    
    private void Start()
    {
        StartCoroutine(ScanTick());
    }

    private void ScanLevel()
    {
        List<Resource> allFoundResources = new List<Resource>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.TryGetComponent(out Resource foundResource))
                allFoundResources.Add(foundResource);
        }

        _dataBase.OperateFoundResources(allFoundResources);
    }

    private IEnumerator ScanTick()
    {
        var wait = new WaitForSeconds(_scanTimer);

        while(enabled)
        {
            ScanLevel();

            yield return wait;
        }
    }
}
