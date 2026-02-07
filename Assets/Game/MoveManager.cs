using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MoveManager : MonoBehaviour
{
    #region Singleton
    private static MoveManager _instance = new MoveManager();

    public static MoveManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;

    public List<Vector3Int> GenerateManhattanPath(Vector3Int start, Vector3Int target)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int current = start;

        while (current.x != target.x || current.y != target.y)
        {
            int dx = target.x - current.x;
            int dy = target.y - current.y;

            Vector3Int next = current;


            if (Mathf.Abs(dx) >= Mathf.Abs(dy) && dx != 0)
                next.x += dx > 0 ? 1 : -1;
            else if (dy != 0)
                next.y += dy > 0 ? 1 : -1;

            next.z = 0;
                

            current = next;
            path.Add(current);
        }

        return path;
    }

    public bool IsCellWalkable(Vector3Int cell)
    {
        if (!groundTilemap.HasTile(cell))
            return false;

        if (obstacleTilemap != null && obstacleTilemap.HasTile(cell))
            return false;

        return true;
    }
}
