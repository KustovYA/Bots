using System.Collections;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter; 
    [SerializeField] private FlagCreator _flagCreator;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Base _base;

    private float _repeatRate = 0.01f;
    private WaitForSeconds _wait;
    private Coroutine CreateCourutine;   

    private void Start()
    {
        if (CreateCourutine != null)
            StopCoroutine(CreateCourutine);

        _wait = new WaitForSeconds(_repeatRate);
        CreateCourutine = StartCoroutine(Move(_wait));
    }   

    private IEnumerator Move(WaitForSeconds wait)
    {
    while (enabled)
    {
        CreateObject();
        yield return wait;
    }
    }

    private void CreateObject()
    {
        if (_flagCreator.IsFlagPut() && _resourceCounter.Number >= _resourceCounter.BasePrice)
        {
            CreateBase();
        }
        else if (_flagCreator.IsFlagPut() == false && _resourceCounter.Number >= _resourceCounter.BotPrice)
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
