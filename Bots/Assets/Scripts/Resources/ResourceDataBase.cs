using System.Collections.Generic;
using UnityEngine;

public class ResourceDataBase : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private List<Resource> _freeResources = new List<Resource>();
    private List<Resource> _busyResources = new List<Resource>();   

    private void OnEnable()
    {
        _resourceSpawner.ResourceSpawned += ReturnToFreeResourcesList;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceSpawned -= ReturnToFreeResourcesList;
    }       

    public Resource GetResource()
    {
        if (_freeResources.Count > 0)
            return _freeResources[0];
        else
            return null;
    }

    public void RemoveResourceFromList(Resource currentResource)
    {
        _freeResources.Remove(currentResource);
        _busyResources.Add(currentResource);
    }

    public void ReturnToFreeResourcesList(Resource currentResource)
    {
        if (_busyResources.Contains(currentResource))
        {
            _busyResources.Remove(currentResource);
            _freeResources.Add(currentResource);
        }        
    }

    public void AddResource(Resource resource)
    {
        if (!_freeResources.Contains(resource) && !_busyResources.Contains(resource))
        {
            _freeResources.Add(resource);
        }
    }

    public bool IsResourceFree()
    {
        return _freeResources.Count > 0;
    }   
}
