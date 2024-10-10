using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private BaseCollector _base;
       
    private Resource _resource = null;    
    private WaitForSeconds _wait;

    public event UnityAction CounterAdded;    

    private void Awake()
    {
        _wait = new WaitForSeconds(_botMover.Duration);
    }

    public void SendForResource(Resource resource)
    {
        _botMover.Move(resource.transform.position);
        _resource = resource;
        
        StartCoroutine(PerformTask(_wait));
    }      

    private IEnumerator PerformTask(WaitForSeconds wait)
    {        
        yield return wait;

        _resource.transform.SetParent(transform);
        _botMover.Move(_base.transform.position);        

        yield return wait;

        _resource.transform.SetParent(null);
        _resource = null;
        CounterAdded?.Invoke();               
    }
    }