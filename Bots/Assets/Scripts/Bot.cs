using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private BaseCollector _base;
       
    private Resource _resource = null;   

    public event UnityAction OnCounterAdded;       

    private void OnTriggerEnter(Collider other)
    {      
        if (other.GetComponent<Resource>() == _resource && _resource != null)       
        {
            _resource.transform.SetParent(transform);
            _botMover.Move(_base.transform.position);            
        }
            
        if (other.GetComponent<BaseCollector>())        
        {            
            if (_resource.transform.IsChildOf(transform))
            {
                print("—бросил ресурс");
                _resource.transform.SetParent(null);                
                _resource = null;
                OnCounterAdded?.Invoke();
            }                  
        }
    }

    public void CollectResource(Resource resource)
    {        
        _botMover.Move(resource.transform.position);  
        _resource = resource;              
    }   
    }
