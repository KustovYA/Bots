using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class InputReader : MonoBehaviour
{
    [SerializeField] private FlagCreator _flagCreator; 
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _flagCreator.CreateFlagAtPoint();            
        }
    }

    private void OnMouseDown()
    {
        _flagCreator.AllocateFlag();
    }
}
