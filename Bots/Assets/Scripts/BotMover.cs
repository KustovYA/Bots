using DG.Tweening;
using UnityEngine;

public class BotMover : MonoBehaviour
{    
    public float Duration { get; private set; } = 2f;      
    
    public void Move(Vector3 target)
    {        
        transform.LookAt(target);
        transform.DOMove(target, Duration).SetEase(Ease.Linear);        
    }
}