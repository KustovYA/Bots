using TMPro;
using UnityEngine;

public class CounterShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ResourceCounter _counter;

    private void OnEnable()
    {
        _counter.OnAmountRaised += ShowCounter;
    }

    private void OnDisable()
    {
        _counter.OnAmountRaised -= ShowCounter;
    }

    private void ShowCounter()
    {
        _text.text = "Ресусов собрано: " + _counter.ShowCount().ToString();
    }        
}
