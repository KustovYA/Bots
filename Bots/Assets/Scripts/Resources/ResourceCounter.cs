using UnityEngine;
using System;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private Base _base;

    public int BotPrice => 3;
    public int BasePrice => 5;

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

        if (_base.IsResourceEnough())
        {
            ResourceAccumulated?.Invoke();            
        }

        AmountChanged?.Invoke();
    }

    public void RemoveCountForMakingNewBot()
    {
        Number -= BotPrice;
        AmountChanged?.Invoke();
    }

    public void RemoveCountForMakingNewBase()
    {
        Number -= BasePrice;
        AmountChanged?.Invoke();
    }
}