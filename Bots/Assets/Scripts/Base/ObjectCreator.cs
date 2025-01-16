using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter; 
    [SerializeField] private FlagCreator _flagCreator;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Base _base;

    private void OnEnable()
    {
        _resourceCounter.ResourceAccumulated += CreateObject;       
    }

    private void OnDisable()
    {
        _resourceCounter.ResourceAccumulated -= CreateObject;
    }

    private void CreateObject()
    {
        if (_flagCreator.IsFlagPut() && _resourceCounter.Number >= 5)
        {
            CreateBase();
        }
        else if (_flagCreator.IsFlagPut() == false && _resourceCounter.Number >= 3)
        {
            CreateBot();
        }
    }

    private void CreateBot()
    {
        Bot bot = Instantiate(_botPrefab, transform.position, UnityEngine.Quaternion.identity);
        bot.AssignBaseCollector(_base);
        bot.BotReleased += _base.FreeBot;
        _base.FreeBot(bot);       
        _resourceCounter.RemoveCountForMakingNewBot();
    }

    private void CreateBase()
    {
        if (_base.IsFreeBot())
        {
            Bot botBuilder = _base.GetBotBuilder();
            _base.RemoveBot(botBuilder);            
            botBuilder.SentToNewBasePosition(_flagCreator.CurrentFlag());
            _resourceCounter.RemoveCountForMakingNewBase();
        }
    }
}
