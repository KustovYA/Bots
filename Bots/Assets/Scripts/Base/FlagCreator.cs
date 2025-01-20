using UnityEngine;

public class FlagCreator : MonoBehaviour
{
    [SerializeField] private GameObject _allocationPrefab;
    [SerializeField] private Flag _flagPrefab;

    private bool _isFlagReady = false;
    private bool _isFlagPut = false;
    private Flag _flag;

    private void OnEnable()
    {        
        _allocationPrefab.SetActive(false);        
    }       

    public void CreateFlagAtPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.GetComponent<Collider>() != null)
            {
                if (IsFlagReady() && !IsFlagPut())
                {
                    _flag = CreateFlag(hit.point);
                }
                else if (IsFlagReady() && IsFlagPut())
                {
                    RemoveFlag();
                    _flag = CreateFlag(hit.point);
                }
            }
        }
    }

    public void AllocateFlag()
    {
        if (_isFlagReady)
        {
            _isFlagReady = false;
        }
        else
        {
            _isFlagReady = true;
        }

        _allocationPrefab.SetActive(_isFlagReady);
    }

    public void RemoveFlag()
    {
        _isFlagPut = false;

        if (_flag != null)
            Destroy(_flag.gameObject);
    }

    public bool IsFlagPut()
    {
        return _isFlagPut;
    }

    public Flag CurrentFlag()
    {
        return _flag;
    }

    public bool IsFlagReady()
    {
        return _isFlagReady;
    }       

    private Flag CreateFlag(Vector3 clickPosition)
    {
        _flag = Instantiate(_flagPrefab, clickPosition, Quaternion.identity);
        _isFlagPut = true;
        return _flag;
    }
}
