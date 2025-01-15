using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Base : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceDataBase _resourceData;
    [SerializeField] private Bot _botPrefab;   
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
        _resourceCounter.ResourceAccumulated += CreateBaseOrBot;               

        foreach (Bot bot in _freeBots)
        {            
            bot.BotReleased += FreeBot;
        }
    }

    private void OnDisable()
    {        
        _resourceCounter.ResourceAccumulated -= CreateBaseOrBot;

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

    private IEnumerator SendForResource(WaitForSeconds wait)
    {
        while (true)
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
    
    private void CreateBaseOrBot()
    {
        if (_flagCreator.IsFlagPut() && _resourceCounter.Number >= 5)
        {            
            CreateBase();
        }
        else if (!_flagCreator.IsFlagPut() && _resourceCounter.Number >= 3)
        {
            CreateBot();
        }        
    }

    private void CreateBot()
    {        
        Bot bot = Instantiate(_botPrefab, transform.position, UnityEngine.Quaternion.identity);
        bot.AssignBaseCollector(this);           
        bot.BotReleased += FreeBot;        
        _freeBots.Add(bot);        
        _resourceCounter.RemoveCountForMakingNewBot();
    }    

    private void CreateBase()
    {       
        if (_freeBots.Count > 0)
        {
            Bot botBuilder = _freeBots[0];
            _freeBots.Remove(botBuilder);
            botBuilder.SentToNewBasePosition(_flagCreator.CurrentFlag());
            _resourceCounter.RemoveCountForMakingNewBase();            
        }
    }    
}
