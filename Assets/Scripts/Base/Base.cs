using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private ResourcesDataBase _resourcesData;
    [SerializeField] private BaseSpawner _spawner;
    [SerializeField] private float _gatheringTimer = 1f;
    [SerializeField] private List<Unit> _harvesters;

    private Coroutine _coroutine;
    private int _stockResources = 0;
    private int _resourcesRequireForSpawn = 3;
    private int _resourcesRequireForBuild = 5;
        
    private void Start()
    {
        StartCoroutine(GatherTick());  
    }

    private void OnEnable()
    {
        foreach(Unit unit in _harvesters)
            unit.ReturnToBase += OnUnitReturn;
    }

    private void OnDisable()
    {
        foreach(Unit unit in _harvesters)
            unit.ReturnToBase -= OnUnitReturn;
    }

    private void GatherResource()
    {
        while (TryFindInactiveHarvester(out Unit harvester) && _resourcesData.TryGetResource(out Resource resource))
        {
            harvester.SetGoal(resource.transform, Unit.State.Harvesting);
        }
    }

    private void StoreResource(Unit unit)
    {
        Resource resource = unit.GetResource();
        _resourcesData.EraseResourceData(resource);
        resource.ProcessConsumption();
        _stockResources++;
        OnResourceAmountChanged();
    }

    private void OnUnitReturn(Unit unit)
    {
        if (unit.HaveResource)
            StoreResource(unit);

        unit.SetGoal(null, Unit.State.Idle);
        //unit.Refresh();
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

    private void OnResourceAmountChanged()
    {
        if (_spawner.IsSpawning && _stockResources >= _resourcesRequireForSpawn)
        {
            _stockResources -= _resourcesRequireForSpawn;
            Unit newHarvester = _spawner.GetUnit();
            newHarvester.ReturnToBase += OnUnitReturn;
            _harvesters.Add(newHarvester);
        }
    }
}
