using UnityEngine.Pool;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 50;       

    private ObjectPool<Resource> _pool;
    private float _positionX = 30f;
    private float _positionZ = 15f;
    private float _positionY = 9f;    
    private WaitForSeconds _wait;
    private Vector3 _offset;
    
    public event Action<Resource> ResourceSpawned;

    private void Awake()
    {
        _wait = new WaitForSeconds(_repeatRate);
        _pool = new ObjectPool<Resource>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, _poolCapacity, _poolMaxSize);        
    }   

    private void Start()
    {
        StartCoroutine(GetCubeRepeating(_wait));
    }      

    private void GetResource()
    {
        _pool.Get();
    }

    public void ReturnCubeToPool(Resource instance)
    {
        _pool.Release(instance);
    }
    
    private Resource CreatePooledItem() 
    {
        Resource instance = Instantiate(_resource);       
        instance.gameObject.SetActive(false);

        return instance;
    }

    private void OnTakeFromPool(Resource instance) 
    {        
        Vector3 randomPosition = new Vector3(Random.Range(-_positionX, _positionX), _positionY, Random.Range(-_positionZ, _positionZ));
        instance.transform.position = randomPosition + _offset;            
        instance.gameObject.SetActive(true);
        ResourceSpawned?.Invoke(instance);
    }

    private void OnReturnToPool(Resource instance) 
    {
        instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Resource instance) 
    {
        Destroy(instance.gameObject);
    }   

    private IEnumerator GetCubeRepeating(WaitForSeconds wait) 
    {       
        while (enabled)
        {
            GetResource();
            yield return wait;
        }       
    }
}