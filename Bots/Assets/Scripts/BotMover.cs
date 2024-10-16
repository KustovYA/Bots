using UnityEngine;

public class BotMover : MonoBehaviour
{
    private float _speed = 10f;
    private Vector3 _target;
   
    private void Start()
    {
        _target = transform.position;
    }

    private void Update()
    {
        transform.LookAt(_target);
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }

    public void Move(Vector3 resource)
    {
        _target = resource;
    }    
}