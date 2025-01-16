using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Base : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceDataBase _resourceData;    
    [SerializeField] private List<Bot> _freeBots = new List<Bot>();
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private FlagCreator _flagCreator;

    private Resource _currentResource = null;
    private Bot _currentBot = null;         
    private float _repeatSendRate = 0.1f;
    private WaitForSeconds _wait;
    private int _resourcesForBuy = 3;    
               
    public event Action CounterAdded;
        
    private void OnEnable()
    {        
        foreach (Bot bot in _freeBots)
        {            
            bot.BotReleased += FreeBot;
        }
    }

    private void OnDisable()
    {      
        foreach (Bot bot in _freeBots)
        {            
            bot.BotReleased -= FreeBot;
        }
    }

    private void Start()
    {
        _wait = new WaitForSeconds(_repeatSendRate);
        StartCoroutine(SendForResource(_wait));
    }    

    public void RemoveFlag()
    {
        _flagCreator.RemoveFlag();
    }

    public void FreeBot(Bot currentBot)
    {
        _freeBots.Add(currentBot);
    }

    public void RemoveBot(Bot currentBot)
    {
        _freeBots.Remove(currentBot);
    }    

    public bool IsResourceEnough()
    {
        return _resourceCounter.Number >= _resourcesForBuy;
    }

    public void ReturnResource(Resource resource)
    {
        _resourceSpawner.ReturnCubeToPool(resource);
        _resourceCounter.AddCount();
        CounterAdded?.Invoke();
    }      

    public bool IsFreeBot()
    {
        return _freeBots.Count > 0;
    }

    public Bot GetBotBuilder()
    {
        return _freeBots[0];
    }

    private IEnumerator SendForResource(WaitForSeconds wait)
    {
        while (enabled)
        {
            if (_resourceData.IsResourceFree())
            {
                SendBot();
            }

            yield return wait;
        }
    }

    private void SendBot()
    {
        if (_freeBots.Count > 0)
        {
            _currentBot = _freeBots[0];
            _currentResource = _resourceData.GetResource();            

            if (_currentResource != null)
            {
                _freeBots.Remove(_currentBot);
                _resourceData.RemoveResourceFromList(_currentResource);
                _currentBot.SendForResource(_currentResource);                
            }
        }
    }    
}
