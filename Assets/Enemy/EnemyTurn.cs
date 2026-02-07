using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    #region Singleton
    private static EnemyTurn _instance = null;
    public static EnemyTurn Instance => _instance;
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        player = Player.Instance;
        moveManager = MoveManager.Instance;
        enemy_Cac = GetComponent<Enemy_Cac>();
    }
    
    #endregion
    public Player player;
    int speed = 3;
    bool canMove;
    Vector3 target;
    MoveManager moveManager;
    Queue<Vector3Int> pathCells = new Queue<Vector3Int>();
    Enemy_Cac enemy_Cac;

    
    public void TurnManager(Vector3Int self, Vector3Int target, EnemyStat stat)
    {
        float distance = Vector3Int.Distance(target, self);
        if(distance < stat.actionRange)
        {
            Action();
        }
        else
        {
            MoveTowardPlayer(self, target, stat.pm_max);
        }
    }
    public void MoveTowardPlayer(Vector3Int start, Vector3Int target, int pm)
    {
        List<Vector3Int> path = moveManager.GenerateManhattanPath(start, target);
        pathCells.Clear();

        foreach (var cell in path)
        {
            pathCells.Enqueue(cell);
        }
        if (pathCells.Count == 0)
            return;
        while(pm > 0 && pathCells.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathCells.Peek(), speed * Time.deltaTime);

            if(Vector3.Distance(pathCells.Peek(), transform.position) < 0.05f)
            {
                pm--;
                TrySetNextTarget();
            }  
        }
        enemy_Cac.turn = false;
    }

    public void Action()
    {
        
    }

    void TrySetNextTarget()
{
    Vector3Int nextCell = pathCells.Peek();

    if (!moveManager.IsCellWalkable(nextCell))
    {
        pathCells.Clear();
        return;
    }

    pathCells.Dequeue();

    target = GridManager.Instance.groundTilemap.CellToWorld(nextCell);
    target.y += 1f;
}
}
