using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private Transform _baseStockPoint;
    [SerializeField] private float _gatheringTimer = 1f;
    [SerializeField] private List<Unit> _harvesters;

    private BaseStockTrigger _trigger;
    private Coroutine _coroutine;
    private int _stockResources = 0;
        
    private void Start()
    {
        _trigger = _baseStockPoint.GetComponent<BaseStockTrigger>();
        _trigger.UnitEnterTrigger += OnUnitEnterTrigger;
        StartCoroutine(GatherTick());  
    }

    private void GatherResource()
    {
        while (TryFindInactiveHarvester(out Unit harvester) && _scaner.TryGetUnassignedResource(out Resource resource))
        {
            harvester.SetTarget(resource.transform);
        }
    }

    private void StoreResource(Unit unit)
    {
        Resource resource = unit.GetResource();
        _scaner.RemoveResource(resource);
        Destroy(resource.gameObject);
        _stockResources++;
    }

    private void OnUnitEnterTrigger(Unit unit)
    {
        if (unit.HaveResource)
            StoreResource(unit);

        if (unit.IsActive && unit.IsReturning)
            unit.Refresh();
    }

    private bool TryFindInactiveHarvester(out Unit inactiveHarvester)
    {
        foreach(Unit harvester in _harvesters)
        {
            if (harvester.IsActive == false)
            {
                inactiveHarvester = harvester;
                return true;
            }
        }  
            
        inactiveHarvester = null;
        return false;  
    }

    private IEnumerator GatherTick()
    {
        var wait = new WaitForSeconds(_gatheringTimer);

        while(enabled)
        {
            GatherResource();

            yield return wait;
        }
    }
}
