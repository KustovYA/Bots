using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollector : MonoBehaviour
{
    [SerializeField] private Scaner _scaner;
    [SerializeField] private List<Bot> _freeBot;
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private List<Bot> _busyBot = new List<Bot>();
    private Resource _currentResource = null;
    //private Resource _scannedResource = null;
    private Bot _currentBot = null;   
    private float _duration = 4f;
    //private List<Resource> _freeResources = new List<Resource>();
    

    private void OnEnable()
    {
        _scaner.OnResourceFounded += SendBot;        
    }

    private void OnDisable()
    {
        _scaner.OnResourceFounded -= SendBot;
    }     

    public void SendBot() 
    {      
        if (_freeBot.Count > 0)
        {
            _currentBot = _freeBot[0];            
            _currentResource = _scaner.GetResource();            

            _freeBot.Remove(_currentBot);
            _busyBot.Add(_currentBot);            
            _scaner.RemoveResourceFromList(_currentResource);

            StartCoroutine(PerformTask(_currentBot, _currentResource));                    
        }            
    }    

    private IEnumerator PerformTask(Bot _currentBot, Resource _currentResource)
    {
        var wait = new WaitForSeconds(_duration);
        _currentBot.CollectResource(_currentResource);
        
        yield return wait;

        _busyBot.Remove(_currentBot);
        _freeBot.Add(_currentBot);      
    }
}
