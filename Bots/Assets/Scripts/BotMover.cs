using DG.Tweening;
using UnityEngine;

public class BotMover : MonoBehaviour
{    
    private float _duration = 2f;      
    
    public void Move(Vector3 target)
    {        
        transform.LookAt(target);
        transform.DOMove(target, _duration).SetEase(Ease.Linear);        
    }
}