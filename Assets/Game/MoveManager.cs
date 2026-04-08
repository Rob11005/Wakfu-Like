using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MoveManager : MonoBehaviour
{
    private GridManager grid;

    public void Start()
    {
        grid = GetComponent<GridManager>();
    }



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

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        Vector3Int[] directions =
        {
            Vector3Int.right, Vector3Int.down, Vector3Int.up, Vector3Int.left
        };

        foreach(var dir in directions)
        {
            Vector3Int neighborPos = node.position + dir;
            if (IsCellWalkable(neighborPos))
                neighbors.Add(new Node(neighborPos));
        }
        return neighbors;
    }
    private int CalculateHeuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public bool IsCellWalkable(Vector3Int cell)
    {
        if (!grid.groundTilemap.HasTile(cell))
            return false;

        if (grid.obstacleTilemap != null && grid.obstacleTilemap.HasTile(cell))
            return false;

        if(grid.occupiedCells != null && grid.occupiedCells.ContainsKey(cell))
            return false;

        return true;
    }

    // private List<Vector3Int> ReconstructPath(Node endNode)
    // {
        
    // }
}
