using TMPro;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private ResourceCollector _collector;
    [SerializeField] private TextMeshProUGUI _text;

    private int _counter = 0;

    private void Update()
    {
        _text.text = "Ресусов собрано: " + _counter.ToString();
    }

    public void AddCount()
    {
        _counter++;
    }
}
