using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;
    private Player player;
    public Dictionary<Vector3Int, UnitStats> occupiedCells = new Dictionary<Vector3Int, UnitStats>();
    private static GridManager instance = null;
    public static GridManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        player = Player.Instance;
    }
}
