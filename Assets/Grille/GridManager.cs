using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;

    private static GridManager _instance = new GridManager();

    public static GridManager Instance
    {
        get
        {
            return _instance;
        }
    }
}
