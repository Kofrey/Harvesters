using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesDataBase : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _generator;

    private List<Resource> _unemployedResources;
    private List<Resource> _employedResources;

    public void Start()
    {
        _unemployedResources = new List<Resource>();
        _employedResources = new List<Resource>();
    }

    public void OperateFoundResources(List<Resource> comingResources)
    {
        foreach(Resource comingResource in comingResources)
        {
            if(!_unemployedResources.Contains(comingResource) && !_employedResources.Contains(comingResource))
                _unemployedResources.Add(comingResource);
        }
    }

    public bool TryGetResource(out Resource resource)
    {
        if (_unemployedResources.Count > 0)
        {
            resource = _unemployedResources[0];
            _unemployedResources.Remove(resource);
            _employedResources.Add(resource);
            return true;
        }
        else 
        {
            resource = null;
            return false;
        }
    }

    public void EraseResourceData(Resource resource)
    {
        _employedResources.Remove(resource);
    }
}
