using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scaner: MonoBehaviour
{    
    [SerializeField] private float _repeatScanRate = 2f;
    [SerializeField] private float _scanRadius = 400f;

    private List<Resource> _freeResources = new List<Resource>();
    private List<Resource> _busyResources = new List<Resource>();      

    public event UnityAction ResourceFounded;

    private void Update()
    {        
        StartCoroutine(PerformScan());
    }

    public Resource GetResource()
    {
        if(_freeResources.Count > 0)
            return _freeResources[0];
        else
            return null;
    }

    public void RemoveResourceFromList(Resource currentResource)
    {
        _freeResources.Remove(currentResource);
        _busyResources.Add(currentResource);
    }

    private void Scan()
    {
        Collider[] scannedObjects = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider scannedObject in scannedObjects) 
        {        
            if (scannedObject.TryGetComponent<Resource>(out Resource resource))
            {
                if(!_freeResources.Contains(resource) && !_busyResources.Contains(resource))
                {
                    _freeResources.Add(resource);                                              
                }                
            }           
        }

        if (_freeResources.Count > 0)
            ResourceFounded?.Invoke();
    }    
    
    private IEnumerator PerformScan()
    {
        var wait = new WaitForSeconds(_repeatScanRate);
        Scan();
        yield return wait;        
    }
}