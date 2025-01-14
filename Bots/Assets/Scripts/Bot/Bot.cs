using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private Base _base;    

    private Resource _resource = null;
    private bool _isBuilder = false;
        
    public event Action<Bot> BotReleased;    

    public void AssignBaseCollector(Base baseCollector)
    {        
        _base = baseCollector;
    }   

    public void SendForResource(Resource resource)
    {
        _botMover.MoveToTarget(resource.transform.position);
        _resource = resource;           
    }

    public void SentToNewBasePosition(Flag flag)
    {
        _botMover.MoveToTarget(flag.transform.position);
        _isBuilder = true;
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() == _resource && _resource != null)
        {
            _resource?.transform.SetParent(transform);
            _botMover.MoveToTarget(_base.transform.position);
        }

        if (other.GetComponent<Base>() == _base && _resource != null)
        {
            _resource?.transform.SetParent(null); 
            _base.ReturnResource(_resource);                       
            BotReleased?.Invoke(this);           
        }

        if(other.GetComponent<Flag>() && _isBuilder)
        {
            _base.RemoveBot(this);
            Base newBase = Instantiate(_base, transform.position, UnityEngine.Quaternion.identity);              
            _base.RemoveFlag();
            AssignBaseCollector(newBase);             
            _isBuilder = false;
            newBase.FreeBot(this);
            BotReleased?.Invoke(this);
        }
    }    
}