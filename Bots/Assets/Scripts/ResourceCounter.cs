using UnityEngine;
using System;

public class ResourceCounter : MonoBehaviour
{         
    public event Action AmountRaised;

    public int Number { get; private set; } = 0;
    
    public void AddCount()
    {
        Number++;
        AmountRaised?.Invoke();
    }
}