using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private BaseCollector _base;

    private Resource _resource = null;    

    public event Action<Resource> ResourceDelivered;
    public event Action CounterAdded;
    public event Action<Bot> BotReleased;
    
    public void SendForResource(Resource resource)
    {
        _botMover.Move(resource.transform.position);
        _resource = resource;           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() == _resource && _resource != null)
        {
            _resource?.transform.SetParent(transform);
            _botMover.Move(_base.transform.position);
        }

        if (other.GetComponent<BaseCollector>())
        {
            if (_resource.transform.IsChildOf(transform))
            {
                ResourceDelivered?.Invoke(_resource);
                _resource?.transform.SetParent(null);
                _resource = null;
                CounterAdded?.Invoke();
                BotReleased?.Invoke(this);
            }
        }
    }    
}