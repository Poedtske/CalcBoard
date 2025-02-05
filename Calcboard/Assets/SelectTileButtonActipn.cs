using UnityEngine;
using UnityEngine.UI;
public class SelectTileButtonActipn : MonoBehaviour
{
    private Button btn;
    private int id;
    private void Awake()
    {
        btn = GetComponent<Button>();
        id= GetComponent<ElektroTile>().TileId;
    }
    private void Start()
    {
        btn.onClick.AddListener(AddTileId);
    }
    public void AddTileId()
    {
        PlayerPrefs.SetInt("TileId",id);
    }
}
