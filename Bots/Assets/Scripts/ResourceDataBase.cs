using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ResourceDataBase : MonoBehaviour
{
    private List<Resource> _freeResources = new List<Resource>();
    private List<Resource> _busyResources = new List<Resource>();
    private WaitForSeconds _wait;
    private float _repeatSendRate = 0.1f;

    public event Action ResourceFounded;

    private void Awake()
    {
        _wait = new WaitForSeconds(_repeatSendRate);
    }

    private void Start()
    {
        StartCoroutine(SendForResource(_wait));
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

    public void AddResource(Resource resource)
    {
        if (!_freeResources.Contains(resource) && !_busyResources.Contains(resource))
        {
            _freeResources.Add(resource);
        }
    }

    private IEnumerator SendForResource(WaitForSeconds wait)
    {
        while (true)
        {
            if (_freeResources.Count > 0)
            {
                ResourceFounded?.Invoke();
            }

            yield return wait;
        }
    }

}
