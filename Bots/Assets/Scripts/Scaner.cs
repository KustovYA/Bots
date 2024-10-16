using System.Collections;
using UnityEngine;

public class Scaner: MonoBehaviour
{    
    [SerializeField] private float _repeatScanRate = 0.1f;
    [SerializeField] private float _scanRadius = 400f;
    [SerializeField] private ResourceDataBase _resourceData;
      
    private WaitForSeconds _wait;   

    private void Awake()
    {
        _wait = new WaitForSeconds(_repeatScanRate);
    }

    private void Start()
    {        
        StartCoroutine(PerformScan(_wait));
    }   

    private void Scan()
    {
        Collider[] scannedObjects = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider scannedObject in scannedObjects) 
        {        
            if (scannedObject.TryGetComponent(out Resource resource))
            {
                _resourceData.AddResource(resource);                             
            }           
        }         
    }    
    
    private IEnumerator PerformScan(WaitForSeconds wait)
    {
        while (true)
        {            
            Scan();
            yield return wait;  
        }              
    }
}