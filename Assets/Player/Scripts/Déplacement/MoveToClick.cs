using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveToClick : MonoBehaviour
{
    [SerializeField] private GameObject redGhost;
    [SerializeField] private GameObject greenGhost;
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;
    public float moveSpeed = 5f;
    List<GameObject> pathGOs = new List<GameObject>();
    private Queue<Vector3> pathWorldPositions = new Queue<Vector3>();
    private Queue<Vector3Int> pathCells = new Queue<Vector3Int>();
    private Vector3 currentTarget;
    Vector3Int selectedCell;
    private bool isMoving;
    private Player player;
    [SerializeField] [Range(-1, 0)] private float ghostY;


    void Start()
    {
        player = GetComponent<Player>();   
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && selectedCell != hit.point && !isMoving)
        {
            for(int i = 0; i < pathGOs.Count; i++)
            {
                Destroy(pathGOs[i]);
            }
            selectedCell = groundTilemap.WorldToCell(hit.point);
            List<Vector3Int> path = GenerateManhattanPath(groundTilemap.WorldToCell(transform.position), selectedCell);
                if (path.Count == 0)
                    return;
                else if(path.Count > player.pmPlayer)
                    return;
                else
            {
                foreach(var cell in path)
                {
                    if(IsCellWalkable(cell) == false)
                    {
                        GameObject ghost = Instantiate(redGhost, new Vector3(groundTilemap.GetCellCenterWorld(cell).x,
                        groundTilemap.GetCellCenterWorld(cell).y + ghostY, 
                        groundTilemap.GetCellCenterWorld(cell).z), Quaternion.identity);
                        pathGOs.Add(ghost);
                    }
                    else if(IsCellWalkable(cell) == true)
                    {
                        GameObject ghost = Instantiate(greenGhost, new Vector3(groundTilemap.GetCellCenterWorld(cell).x,
                        groundTilemap.GetCellCenterWorld(cell).y + ghostY, 
                        groundTilemap.GetCellCenterWorld(cell).z), Quaternion.identity);
                        pathGOs.Add(ghost);
                    }
                }
            }
        }

        if (!isMoving)
        {
            HandleClick();
        }
        else
        {
            MovePlayer();
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3Int startCell = groundTilemap.WorldToCell(transform.position);
                Vector3Int targetCell = groundTilemap.WorldToCell(hit.point);

                // Forcer Z = 0 pour que ça n’interfère pas
                startCell.z = 0;
                targetCell.z = 0;

                

                if (!IsCellWalkable(targetCell))
                    return;

                List<Vector3Int> path = GenerateManhattanPath(startCell, targetCell);
                if (path.Count == 0)
                    return;
                if(path.Count > player.pmPlayer)
                    return;

                pathWorldPositions.Clear();

                pathCells.Clear();

                foreach (var cell in path)
                {
                    pathCells.Enqueue(cell);
                }

                TrySetNextTarget();
                            }
                        }
                    }

    void MovePlayer()
{
    transform.position = Vector3.MoveTowards(
        transform.position,
        currentTarget,
        moveSpeed * Time.deltaTime
    );

    if (Vector3.Distance(transform.position, currentTarget) < 0.05f)
    {
        player.pmPlayer--;
        TrySetNextTarget();
    }
}
void TrySetNextTarget()
{
    if (pathCells.Count == 0 || player.pmPlayer <= 0)
    {
        isMoving = false;
        return;
    }

    Vector3Int nextCell = pathCells.Peek();

    if (!IsCellWalkable(nextCell))
    {
        pathCells.Clear();
        isMoving = false;
        return;
    }

    pathCells.Dequeue();

    currentTarget = groundTilemap.GetCellCenterWorld(nextCell);
    currentTarget.y += 1f;

    isMoving = true;
}

    List<Vector3Int> GenerateManhattanPath(Vector3Int start, Vector3Int target)
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

    bool IsCellWalkable(Vector3Int cell)
    {
        if (!groundTilemap.HasTile(cell))
            return false;

        if (obstacleTilemap != null && obstacleTilemap.HasTile(cell))
            return false;

        return true;
    }

}
