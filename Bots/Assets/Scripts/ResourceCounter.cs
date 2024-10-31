using UnityEngine;
using System;

public class ResourceCounter : MonoBehaviour
{
    public event Action AmountChanged;
    public event Action ResourceAccumulated;

    public int Number { get; private set; }

    public void Awake()
    {
        Number = 0;
        AmountChanged?.Invoke();
    }

    public void AddCount()
    {
        Number++;        

        if (Number >= 3)
        {
            ResourceAccumulated?.Invoke();
            print("Ресурсы собраны");
        }

        AmountChanged?.Invoke();
    }

    public void RemoveCountForCreateBot()
    {
        Number = Number - 3;
        AmountChanged?.Invoke();
    }

    public void RemoveCountForCreateBase()
    {
        Number = Number - 5;
        AmountChanged?.Invoke();
    }
}