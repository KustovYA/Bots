using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BaseCollector : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private ResourceDataBase _resourceData;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private GameObject _allocationPrefab;
    [SerializeField] private List<Bot> _freeBots = new List<Bot>();
    [SerializeField] private ResourceSpawner _resourceSpawner;    

    private Resource _currentResource = null;
    private Bot _currentBot = null;     
    private bool _isFlagReady = false;
    private bool _isFlagPut = false;
    private Flag _flag;        

    private void OnEnable()
    {        
        _resourceData.ResourceFounded += SendBot;
        _resourceCounter.ResourceAccumulated += CreateObject;
        _allocationPrefab.SetActive(false);        

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded += _resourceCounter.AddCount;
            bot.BotReleased += FreeBot;
        }
    }

    private void OnDisable()
    {
        _resourceData.ResourceFounded -= SendBot;
        _resourceCounter.ResourceAccumulated -= CreateObject;

        foreach (Bot bot in _freeBots)
        {
            bot.CounterAdded -= _resourceCounter.AddCount;
            bot.BotReleased -= FreeBot;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                       
            if (Physics.Raycast(ray, out hit))
            {                
                if (hit.collider != null && hit.collider.GetComponent<Collider>() != null)
                {    
                    if (IsFlagReady() && !IsFlagPut())
                    {
                        _flag = CreateFlag(hit.point);
                    }
                    else if (IsFlagReady() && IsFlagPut())
                    {
                        RemoveFlag();
                        _flag = CreateFlag(hit.point);
                    }
                }
            }
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

    public bool IsFlagReady()
    {        
        return _isFlagReady;
    }

    public bool IsFlagPut()
    {
        return _isFlagPut;
    }

    public Flag CreateFlag(Vector3 clickPosition)
    {
        _flag = Instantiate(_flagPrefab, clickPosition, Quaternion.identity);
        _isFlagPut = true;        
        return _flag;
    }

    public void RemoveFlag()
    {
        _isFlagPut = false; 

        if (_flag != null)
            Destroy(_flag.gameObject);        
    }

    private void OnMouseDown()
    {
        if (_isFlagReady)
        {
            _isFlagReady = false;            
        }
        else
        {
            _isFlagReady = true;
        }

        _allocationPrefab.SetActive(_isFlagReady);
    }
    
    private void CreateObject()
    {
        print(_isFlagPut);
        if (_isFlagPut && _resourceCounter.Number >= 5)
        {            
            CreateBase();
        }
        else if (!_isFlagPut && _resourceCounter.Number >= 3)
        {
            CreateBot();
        }
    }

    private void CreateBot()
    {        
        Bot bot = Instantiate(_botPrefab, transform.position, UnityEngine.Quaternion.identity);
        bot.GetBaseCollector(this);
        bot.GetResourceSpawner(_resourceSpawner);
        bot.CounterAdded += _resourceCounter.AddCount;
        bot.BotReleased += FreeBot;        
        _freeBots.Add(bot);        
        _resourceCounter.RemoveCountForCreateBot();
    }    

    private void CreateBase()
    {
        if (_freeBots.Count > 0)
        {
            Bot botBuilder = _freeBots[0];
            _freeBots.Remove(botBuilder);
            botBuilder.BuildBase(_flag);
            _resourceCounter.RemoveCountForCreateBase();
        }
    }

    public void FreeBot(Bot currentBot) 
    {
        _freeBots.Add(currentBot);
    }

    public void RemoveBot(Bot currentBot)
    {
        _freeBots.Remove(currentBot);
    }
}
