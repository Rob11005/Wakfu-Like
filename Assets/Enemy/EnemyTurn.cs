using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    public bool turn;
    public Player player;
    int speed = 1;
    bool isMoving = false;
    float distance;
    public float moveSpeed;
    Vector3 currentTarget;
    [SerializeField] private MoveManager moveManager;
    [SerializeField] private GridManager grid;
    Queue<Vector3Int> pathCells = new Queue<Vector3Int>();
    EnemyStat stats;

    void Start()
    {
        player = Player.Instance;
        grid = GridManager.Instance;
        stats = GetComponent<EnemyStat>();
        grid.occupiedCells.Add(grid.groundTilemap.WorldToCell(transform.position), stats);
    }
    void Update()
    {
            
        if(!turn) return;  // ← stoppe tout si ce n'est pas son tour

        if(isMoving)
        {
            MoveTowardPlayer();
        }
            else
            {
                EndTurn();
            }
    
    }
        public void StartTurn()
    {
        stats.pm_current = stats.pm_max;
        stats.pa_current = stats.pa_max;
        turn = true;
        TurnManager(stats.pm_current);
    }

    public void EndTurn()
    {
        turn = false;
        GameManager.Instance.PlayNextEnemy();
    }

    #region Action&Movement
    
    public void TurnManager(int pm_current)
    {
        
        Vector3Int self = grid.groundTilemap.WorldToCell(transform.position);
        Vector3Int target = grid.groundTilemap.WorldToCell(player.transform.position);
        distance = Vector3Int.Distance(target, self);
        Debug.Log(distance);
        self.z = 0;
        target.z = 0;
        if(stats.actionRange > distance &&  stats.pa_current > 0)
        {
            Action();
        }
        else
        {
            List<Vector3Int> path = moveManager.FindPath(self, target);

        pathCells.Clear();

        foreach (var cell in path)
                {
                    if(pm_current > 0)
                    {
                        pathCells.Enqueue(cell);
                        stats.pm_current--;
                    }
                    else
                        return;
                }
        grid.occupiedCells.Remove(grid.groundTilemap.WorldToCell(transform.position));
        SetNextTarget();
        }
        
    }

    private void MoveTowardPlayer()
{
    transform.position = Vector3.MoveTowards(
        transform.position,
        currentTarget,
        moveSpeed * Time.deltaTime
    );

    if (Vector3.Distance(transform.position, currentTarget) < 0.05f)
    {
        SetNextTarget();
    }
}

    public void Action()
    {
        stats.pa_current -= 2;
        Debug.Log("Action");
    }

    void SetNextTarget()
{
    if (pathCells.Count == 0 || stats.pm_current <= 0)
    {
        isMoving = false;
        grid.occupiedCells.Add(grid.groundTilemap.WorldToCell(transform.position), stats);  
        return;
    }

    Vector3Int nextCell = pathCells.Peek();

    pathCells.Dequeue();

    currentTarget = grid.groundTilemap.GetCellCenterWorld(nextCell);
    currentTarget.y += 1f;

    isMoving = true;
}
#endregion

 private bool CanPlay()
    {
        if(stats.pa_current > 0)
        return true;
        else if(stats.pm_current > 0)
        return true;
        else
        return false;
    }
}
