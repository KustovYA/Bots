using UnityEngine;


public class ResourceCollector: MonoBehaviour
{
    [SerializeField] protected Resource _resource;
    [SerializeField] private float _repeatScanRate = 2f;
    [SerializeField] protected float _scanRadius = 400f;

    private float _currentTime;
    private Resource _currentResource;
    private BotMover _currentBot;

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if ( _currentTime > _repeatScanRate)
        {            
            Scan();
            _currentTime = 0;
            _currentResource = null;
            _currentBot = null;
        }
    }

    private void Scan()
    {
        Collider[] scannedObjects = Physics.OverlapSphere(transform.position, _scanRadius);

        foreach (Collider scannedObject in scannedObjects) 
        {        
            if (scannedObject.TryGetComponent<Resource>(out Resource resource))
            {
                CheckFreeResource(resource);                   
            }

            if (scannedObject.TryGetComponent<BotMover>(out BotMover bot))
            {                
                CheckFreeBots(bot);                              
            }
        }

        if(_currentResource != null && _currentBot != null)
        {
            _currentBot.CollectResource(_currentResource, transform.position);
            _currentBot = null;
        }
    }

    private void CheckFreeResource(Resource resource)
    {
        if (resource.ResourceIsFree())
        {
            _currentResource = resource;
            Debug.Log("Свободный ресурс найден");
            resource.MakeResourceBusy();
        }
    }

    private void CheckFreeBots(BotMover bot)
    {
        if (bot.IsFree())
        {
            _currentBot = bot;
            Debug.Log("Свободный бот найден"); 
        }
    }   
}
