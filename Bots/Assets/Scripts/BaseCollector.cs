using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCollector : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private List<Bot> _freeBot;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourceCounter _resourceCounter;

    private List<Bot> _busyBot = new List<Bot>();
    private Resource _currentResource = null;    
    private Bot _currentBot = null;   
    private float _duration = 4f;  
    
    public event UnityAction<Resource> ResourceDelivered;

    private void OnEnable()
    {
        _scaner.ResourceFounded += SendBot;

        foreach (Bot bot in _freeBot)
        {
            bot.CounterAdded += _resourceCounter.AddCount;
        }
    }

    private void OnDisable()
    {
        _scaner.ResourceFounded -= SendBot;

        foreach (Bot bot in _freeBot)
        {
            bot.CounterAdded -= _resourceCounter.AddCount;
        }
    }

    public void SendBot()
    {
        if (_freeBot.Count > 0)
        {
            _currentBot = _freeBot[0];
            _currentResource = _scaner.GetResource();

            if (_currentResource != null)
            {
                _freeBot.Remove(_currentBot);
                _busyBot.Add(_currentBot);
                _scaner.RemoveResourceFromList(_currentResource);

                StartCoroutine(PerformTask(_currentBot, _currentResource));
            }
        }
    }        

    private IEnumerator PerformTask(Bot _currentBot, Resource _currentResource)
    {
        var wait = new WaitForSeconds(_duration);
        _currentBot.CollectResource(_currentResource);
        
        yield return wait;

        _busyBot.Remove(_currentBot);
        _freeBot.Add(_currentBot);
        ResourceDelivered?.Invoke(_currentResource);
    }
}