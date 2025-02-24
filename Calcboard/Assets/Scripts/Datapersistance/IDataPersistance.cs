using UnityEngine;

public interface IDataPersistance
{
    void LoadData(ElektroMapData data);
    void SaveData(ref ElektroMapData data);
}
