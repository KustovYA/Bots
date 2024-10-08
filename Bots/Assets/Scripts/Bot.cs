using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotMover _botMover;
    [SerializeField] private BaseCollector _base;
       
    private Resource _resource = null;
    private float _duration = 2f;

    public event UnityAction CounterAdded;

    public void CollectResource(Resource resource)
    {
        _botMover.Move(resource.transform.position);
        _resource = resource;
        StartCoroutine(PerformTask());
    }      

    private IEnumerator PerformTask()
    {
        var wait = new WaitForSeconds(_duration);
        yield return wait;

        if (_resource != null)
        {
            _resource.transform.SetParent(transform);
            _botMover.Move(_base.transform.position);
        }        

        yield return wait;

        if (_resource.transform.IsChildOf(transform))
        {
            _resource.transform.SetParent(null);
            _resource = null;
            CounterAdded?.Invoke();
        }
    }
    }