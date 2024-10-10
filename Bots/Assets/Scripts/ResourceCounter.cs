using UnityEngine;
using UnityEngine.Events;

public class ResourceCounter : MonoBehaviour
{         
    public event UnityAction AmountRaised;

    public int Number { get; private set; } = 0;
    
    public void AddCount()
    {
        Number++;
        AmountRaised?.Invoke();
    }
}