using UnityEngine;

public class MusicoTiles : MonoBehaviour
{
    public MusicoMapManager mapManager;
    private bool startup = true;

    private void OnEnable()
    {
        if (startup)
        {
            startup = false;
        }
        else
        {
            mapManager.ReloadTile();
        }
    }
}
