using UnityEngine;
using System.Collections;

public class BotMover : MonoBehaviour
{
    private float _speed = 50f;
    private Vector3 _target;
    private float _repeatRate = 0.01f;
    private WaitForSeconds _wait;
    private Coroutine MoveCourutine;

    private void Start()
    {
        _target = transform.position;
    }
    
    public void MoveToTarget(Vector3 resource)
    {
        if (MoveCourutine != null) 
            StopCoroutine(MoveCourutine); 

        _target = resource;
        _wait = new WaitForSeconds(_repeatRate);
        MoveCourutine = StartCoroutine(Move(_wait));
    }

    private IEnumerator Move(WaitForSeconds wait)
    {
        while (enabled)
        {
            transform.LookAt(_target);
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            yield return wait;
        }
    }
}