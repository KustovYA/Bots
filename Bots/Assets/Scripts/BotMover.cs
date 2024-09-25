using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _runAnimation;
    [SerializeField] private AnimationClip _idleAnimation;
    [SerializeField] private ResourceCounter _resourceCounter;

    private float _duration = 3f;    
    private float minDistance = 0.5f;
    private bool _isFree = true;    
    private Vector3 _lookDirection;   

    public bool IsFree()
    {
        return _isFree;
    }

    public void CollectResource(Resource resource, Vector3 basePosition)
    {
        _isFree = false;        
        _lookDirection = resource.transform.position;
        transform.LookAt(_lookDirection);
        transform.DOMove(resource.transform.position, _duration);             
        _animator.Play(_runAnimation.name);
        StartCoroutine(Collect(resource, _duration, basePosition));         
    }

    private IEnumerator Collect(Resource resource, float _duration, Vector3 basePosition)
    {        
        var wait = new WaitForSeconds(_duration);
        yield return wait;
        
        Vector3 distance = resource.transform.position - transform.position;
        float sqrX = distance.x * distance.x;
        float sqrZ = distance.z * distance.z;
        float sqrtDistance = Mathf.Sqrt(sqrX + sqrZ);        

        if (sqrtDistance<minDistance)
        {
            resource.transform.SetParent(transform);                    
        }

        _lookDirection = basePosition;
        transform.LookAt(_lookDirection);
        transform.DOMove(basePosition, _duration);
        yield return wait;
        resource.transform.SetParent(null);
        _resourceCounter.AddCount();
        _isFree = true;
        _animator.Play(_idleAnimation.name);
    }
}
