using UnityEngine;

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
