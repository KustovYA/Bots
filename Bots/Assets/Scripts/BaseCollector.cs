using System.Collections.Generic;
using UnityEngine;

public class BaseCollector : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private List<Bot> _freeBots;
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceDataBase _resourceData;

    private Resource _currentResource = null;
    private Bot _currentBot = null;    

    private void OnEnable()
    {
        _resourceData.ResourceFounded += SendBot;

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded += _resourceCounter.AddCount;
            bot.BotReleased += FreeBot;
        }
    }

    private void OnDisable()
    {
        _resourceData.ResourceFounded -= SendBot;

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded -= _resourceCounter.AddCount;
            bot.BotReleased -= FreeBot;
        }
    }   

    public void SendBot()
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

    private void FreeBot(Bot currentBot) 
    {
        _freeBots.Add(currentBot);
    }
}
