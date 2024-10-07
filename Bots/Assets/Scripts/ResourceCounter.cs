using UnityEngine;
using UnityEngine.Events;

public class ResourceCounter : MonoBehaviour
{           
    private int _counter = 0;

    public event UnityAction AmountRaised;

    public int GetCount()
    {
        return _counter;
    }      

    public void AddCount()
    {
        _counter++;
        AmountRaised?.Invoke();
    }
}