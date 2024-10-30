using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private BaseCollector _base;
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private Resource _resource = null;
    private bool _isBuilder = false;
        
    public event Action CounterAdded;
    public event Action<Bot> BotReleased;    

    public void GetBaseCollector(BaseCollector baseCollector)
    {        
        _base = baseCollector;
    }

    public void GetResourceSpawner(ResourceSpawner resourceSpawner)
    {
        _resourceSpawner = resourceSpawner;
    }

    public void SendForResource(Resource resource)
    {
        _botMover.Move(resource.transform.position);
        _resource = resource;           
    }

    public void BuildBase(Flag flag)
    {
        _botMover.Move(flag.transform.position);
        _isBuilder = true;
    }

    public void ReturnResource(Resource resource)
    {
        _resourceSpawner.ReturnCubeToPool(resource);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() == _resource && _resource != null)
        {
            _resource?.transform.SetParent(transform);
            _botMover.Move(_base.transform.position);
        }

        if (other.GetComponent<BaseCollector>() == _base && _resource != null)
        {
            if (_resource.transform.IsChildOf(transform))
            {               
                _resource?.transform.SetParent(null);                
                ReturnResource(_resource);
                CounterAdded?.Invoke();
                BotReleased?.Invoke(this);
            }
        }

        if(other.GetComponent<Flag>() && _isBuilder)
        {
            _base.RemoveBot(this);
            BaseCollector newBase = Instantiate(_base, transform.position, UnityEngine.Quaternion.identity);              
            _base.RemoveFlag();
            GetBaseCollector(newBase);             
            _isBuilder = false;
            newBase.FreeBot(this);
            BotReleased?.Invoke(this);
        }
    }    
}