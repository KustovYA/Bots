using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCollector : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private List<Bot> _freeBots;    
    [SerializeField] private ResourceCounter _resourceCounter;
       
    private Resource _currentResource = null;    
    private Bot _currentBot = null;   
    private float _duration = 4f;
    private WaitForSeconds _wait;

    public event UnityAction<Resource> ResourceDelivered;

    private void Awake()
    {
        _wait = new WaitForSeconds(_duration);
    }

    private void OnEnable()
    {
        _scaner.ResourceFounded += SendBot;
        

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded += _resourceCounter.AddCount;
        }
    }

    private void OnDisable()
    {
        _scaner.ResourceFounded -= SendBot;

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded -= _resourceCounter.AddCount;           
        }
    }        

    public void SendBot()
    {        
        if (_freeBots.Count > 0)
        {            
            _currentBot = _freeBots[0];
            _currentResource = _scaner.GetResource();
            print("Бот отправлен " + _currentBot);

            if (_currentResource != null)
            {
                _freeBots.Remove(_currentBot);                
                _scaner.RemoveResourceFromList(_currentResource);                
                StartCoroutine(PerformTask(_currentBot, _currentResource, _wait));
            }
        }
    }        

    private IEnumerator PerformTask(Bot currentBot, Resource currentResource, WaitForSeconds wait)
    {
        currentBot.SendForResource(currentResource);

        yield return wait;
        _freeBots.Add(currentBot);
        ResourceDelivered?.Invoke(currentResource);        
    }
}