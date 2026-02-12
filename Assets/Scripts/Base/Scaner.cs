using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Scaner : MonoBehaviour
{
    [SerializeField] private Transform _resourceParent;
    [SerializeField] private float _scanTimer = 3f;

    private const bool IsAssigned = true;

    private Dictionary<Resource, bool> _pendingResource = new Dictionary<Resource, bool>();

    private void Start()
    {
        StartCoroutine(ScanTick());
    }

    public bool TryGetUnassignedResource(out Resource resource)
    {
        foreach(KeyValuePair<Resource, bool> pair in _pendingResource)
        {
            if (pair.Value == false)
            {
                resource = pair.Key;
                _pendingResource[resource] = IsAssigned;
                return true;
            }
        }

        resource = null;
        return false;
    }

    public void RemoveResource(Resource resource)
    {
        _pendingResource.Remove(resource);
    }

    private void ScanLevel()
    {
        Resource[] allResources = _resourceParent.GetComponentsInChildren<Resource>();

        foreach(Resource resource in allResources)
        {
            try
            {
                _pendingResource.Add(resource, !IsAssigned);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("An element with Key already exists.");
            }
        }
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
