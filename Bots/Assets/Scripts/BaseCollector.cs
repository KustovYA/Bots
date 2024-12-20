using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private float _repeatSendRate = 0.1f;
    private WaitForSeconds _wait;
    private int _resourcesForBuy = 3;    

    public event Action ResourceIsInDataBase;        
    public event Action CounterAdded;
        
    private void OnEnable()
    {               
        _resourceCounter.ResourceAccumulated += CreateObject;
        _allocationPrefab.SetActive(false);        

        foreach (Bot bot in _freeBots)
        {            
            bot.BotReleased += FreeBot;
        }
    }

    private void OnDisable()
    {        
        _resourceCounter.ResourceAccumulated -= CreateObject;

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

    private void Update()
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

    public void RemoveFlag()
    {
        _isFlagPut = false;

        if (_flag != null)
            Destroy(_flag.gameObject);
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

    private bool IsFlagReady()
    {        
        return _isFlagReady;
    }

    private bool IsFlagPut()
    {
        return _isFlagPut;
    }

    private Flag CreateFlag(Vector3 clickPosition)
    {
        _flag = Instantiate(_flagPrefab, clickPosition, Quaternion.identity);
        _isFlagPut = true;        
        return _flag;
    }     
       
    
    private void CreateObject()
    {
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
        bot.AssignBaseCollector(this);           
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
            botBuilder.SentToNewBasePosition(_flag);
            _resourceCounter.RemoveCountForCreateBase();            
        }
    }    
}
