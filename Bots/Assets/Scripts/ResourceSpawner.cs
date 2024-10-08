using UnityEngine.Pool;
using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 50;
    [SerializeField] private BaseCollector _baseCollector;

    private ObjectPool<Resource> _pool;
    private float _positionX = 30f;
    private float _positionZ = 15f;
    private float _positionY = 9f;
    private float _baseRadius = 5f;

    private void OnEnable()
    {
        _baseCollector.ResourceDelivered += ReturnCubeToPool;
    }

    private void OnDisable()
    {
        _baseCollector.ResourceDelivered -= ReturnCubeToPool;
    }

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(CreatePooledCube, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, _poolCapacity, _poolMaxSize); // создаем новый пул и добавляем в скрипт методы описывающие его
    }

    private void Start()
    {        
        StartCoroutine(GetCubeRepeating());
    }

    private void GetResource()
    {
        _pool.Get();
    }

    private void ReturnCubeToPool(Resource instance)
    {
        _pool.Release(instance);
    }

    private Resource CreatePooledCube() 
    {
        Resource instance = Instantiate(_resource);       
        instance.gameObject.SetActive(false);

        return instance;
    }

    private void OnTakeFromPool(Resource instance) 
    {
        Vector3 randomPosition;

        do
        {
            randomPosition = new Vector3(Random.Range(-_positionX, _positionX), _positionY, Random.Range(-_positionZ, _positionZ));
        }
        while (IsInCircle(randomPosition));

        instance.transform.position = randomPosition;            
        instance.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Resource instance) 
    {
        instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Resource instance) 
    {
        Destroy(instance.gameObject);
    }

    private bool IsInCircle(Vector3 position)
    {
        float centerX = 0f;
        float centerZ = 0f;

        float distance = Mathf.Sqrt(Mathf.Pow(position.x - centerX, 2) + Mathf.Pow(position.z - centerZ, 2));
        return distance < _baseRadius;
    }

    private IEnumerator GetCubeRepeating() 
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (true)
        {
            GetResource();
            yield return wait;
        }       
    }
}