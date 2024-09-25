using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isResourceFree = true;
   
    public bool ResourceIsFree()
    {
        return _isResourceFree;
    }

    public void MakeResourceBusy()
    {
        _isResourceFree = false;
    }
}
