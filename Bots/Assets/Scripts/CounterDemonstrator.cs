using TMPro;
using UnityEngine;

public class CounterDemonstrator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ResourceCounter _counter;

    private void OnEnable()
    {
        _counter.AmountRaised += Display;
    }

    private void OnDisable()
    {
        _counter.AmountRaised -= Display;
    }

    private void Display()
    {
        _text.text = "������� �������: " + _counter.Number.ToString();
    }        
}