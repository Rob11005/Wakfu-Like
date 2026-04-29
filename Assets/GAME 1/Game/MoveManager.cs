using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MoveManager : MonoBehaviour
{
    private GridManager grid;

    public void Start()
    {
        grid = GetComponent<GridManager>();
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        Node currentNode = new Node(start);

        currentNode.h = CalculateHeuristic(start, target);
        currentNode.f = currentNode.h;
        OpenList.Add(currentNode);

        while(OpenList.Count > 0)
        {
            currentNode = OpenList.OrderBy(n => n.f).ThenBy(n => n.h).First();
            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if(currentNode.position == target)
            {
                return ReconstructPath(currentNode);
            }

            foreach(Node node in GetNeighbors(currentNode))
            {  
                if(!ClosedList.Any(n => n.position == node.position))
                {
                    node.g = currentNode.g + 1;
                    node.h = CalculateHeuristic(node.position, target);
                    node.f = node.g + node.h;
                    node.parent = currentNode;
                    if(OpenList.Any(n => n.position == node.position))
                    {
                        Node n = OpenList.Find(n => n.position == node.position);
                        if(n.g > node.g)
                        {
                            n.g = node.g;
                            n.f = node.g + n.h;
                            n.parent = currentNode;
                        }
                    }
                    else
                        OpenList.Add(node);
                }
            }
        }
        // Debug.Log("Start : " + start + " | Target : " + target);
        // Debug.Log("Voisins du départ : " + GetNeighbors(new Node(start)).Count);
        // Debug.Log("groundTilemap has tile at start : " + grid.groundTilemap.HasTile(start));
        return new List<Vector3Int>();
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
            neighborPos.z = 0;
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
        if(cell == grid.groundTilemap.WorldToCell(Player.Instance.transform.position)) return true;
        if (!grid.groundTilemap.HasTile(cell))
            return false;

        if (grid.obstacleTilemap != null && grid.obstacleTilemap.HasTile(cell))
            return false;

        if(grid.occupiedCells != null && grid.occupiedCells.ContainsKey(cell))
        {
            return false;
        }
        return true;
    }

    private List<Vector3Int> ReconstructPath(Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node n = endNode;

        while(n != null)
        {
            path.Add(n.position);
            n = n.parent;
        }

        path.Reverse();
        path.RemoveAt(0);
        return path;
    }
}
