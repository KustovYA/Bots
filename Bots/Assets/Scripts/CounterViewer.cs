using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CounterViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ResourceCounter _counter;    

    private Vector3 _offset = new Vector3(4, 0, 2.5f);

    private void OnEnable()
    {
        _counter.AmountChanged += Display;
        RectTransform rectTransform = _text.rectTransform;
        rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_counter.transform.position + _offset);
    }

    private void OnDisable()
    {
        _counter.AmountChanged -= Display;
    }

    private void Display()
    {
        _text.text = "Ресурсов: " + _counter.Number.ToString();        
    }        
}