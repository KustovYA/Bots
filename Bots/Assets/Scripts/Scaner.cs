using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scaner: MonoBehaviour
{
    [SerializeField] protected Resource _resource;
    [SerializeField] private float _repeatScanRate = 2f;
    [SerializeField] protected float _scanRadius = 400f;

    private List<Resource> _freeResources = new List<Resource>();
    private List<Resource> _busyResources = new List<Resource>();
    private float _currentTime;    

    public event UnityAction OnResourceFounded;

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if ( _currentTime > _repeatScanRate)
        {            
            Scan();
            _currentTime = 0;            
        }
    }

    public Resource GetResource()
    {
        return _freeResources[0];
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
                if(resource != null && !_freeResources.Contains(resource) && !_busyResources.Contains(resource))
                {
                    _freeResources.Add(resource);                    
                    OnResourceFounded?.Invoke();                  
                }
            }           
        }        
    }         
}
