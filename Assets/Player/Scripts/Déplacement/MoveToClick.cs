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
    private MoveManager moveManager;
    private GridManager grid;
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
        grid = GridManager.Instance;
        moveManager = MoveManager.Instance; 
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
            selectedCell = GridManager.Instance.groundTilemap.WorldToCell(hit.point);
            List<Vector3Int> path = moveManager.GenerateManhattanPath(grid.groundTilemap.WorldToCell(transform.position), selectedCell);
                if (path.Count == 0)
                    return;
                else if(path.Count > player.pmPlayer)
                    return;
                else
            {
                foreach(var cell in path)
                {
                    if(moveManager.IsCellWalkable(cell) == false)
                    {
                        GameObject ghost = Instantiate(redGhost, new Vector3(grid.groundTilemap.GetCellCenterWorld(cell).x,
                        grid.groundTilemap.GetCellCenterWorld(cell).y + ghostY, 
                        grid.groundTilemap.GetCellCenterWorld(cell).z), Quaternion.identity);
                        pathGOs.Add(ghost);
                    }
                    else if(moveManager.IsCellWalkable(cell) == true)
                    {
                        GameObject ghost = Instantiate(greenGhost, new Vector3(grid.groundTilemap.GetCellCenterWorld(cell).x,
                        grid.groundTilemap.GetCellCenterWorld(cell).y + ghostY, 
                        grid.groundTilemap.GetCellCenterWorld(cell).z), Quaternion.identity);
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
                Vector3Int startCell = grid.groundTilemap.WorldToCell(transform.position);
                Vector3Int targetCell = grid.groundTilemap.WorldToCell(hit.point);

                // Forcer Z = 0 pour que ça n’interfère pas
                startCell.z = 0;
                targetCell.z = 0;

                

                if (!moveManager.IsCellWalkable(targetCell))
                    return;

                List<Vector3Int> path = moveManager.GenerateManhattanPath(startCell, targetCell);
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

    if (!moveManager.IsCellWalkable(nextCell))
    {
        pathCells.Clear();
        isMoving = false;
        return;
    }

    pathCells.Dequeue();

    currentTarget = grid.groundTilemap.GetCellCenterWorld(nextCell);
    currentTarget.y += 1f;

    isMoving = true;
}

}
