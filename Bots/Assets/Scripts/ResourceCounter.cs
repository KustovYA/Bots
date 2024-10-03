using UnityEngine;
using UnityEngine.Events;

public class ResourceCounter : MonoBehaviour
{    
   [SerializeField] private Bot[] _bots;

    public event UnityAction OnAmountRaised;

    private int _counter = 0;

   private void OnEnable()
    {
        foreach(Bot bot in _bots)
        {
            bot.OnCounterAdded += AddCount;
        }        
    }

    private void OnDisable()
    {
        foreach (Bot bot in _bots)
        {
            bot.OnCounterAdded -= AddCount;
        }        
    }

    public int ShowCount()
    {
        return _counter;
    }      

    public void AddCount()
    {
        _counter++;
        OnAmountRaised?.Invoke();
    }
}
