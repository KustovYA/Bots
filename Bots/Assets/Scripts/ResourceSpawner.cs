using UnityEngine.Pool;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] protected Resource _resource;
    [SerializeField] private float _repeatRate = 3f;
    [SerializeField] protected int _poolCapacity = 10;
    [SerializeField] protected int _poolMaxSize = 50;

    private ObjectPool<Resource> _pool;
    private float _positionX = 34f;
    private float _positionZ = 17f;
    private float _positionY = 9f;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(CreatePooledCube, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, _poolCapacity, _poolMaxSize); // создаем новый пул и добавляем в скрипт методы описывающие его
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
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
        instance.transform.position = new Vector3(Random.Range(-_positionX, _positionX), _positionY, Random.Range(-_positionZ, _positionZ));        
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
}
