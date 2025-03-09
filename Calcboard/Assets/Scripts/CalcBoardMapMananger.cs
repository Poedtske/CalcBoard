using UnityEngine;

public abstract class CalcBoardMapMananger : MonoBehaviour
{
    public abstract void Save();
    public abstract void ReloadTile();
    public abstract void LoadTiles();
    public abstract void SetIdAction(int id);
}
