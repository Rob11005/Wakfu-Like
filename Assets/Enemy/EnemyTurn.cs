using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    
    public Player player;
    int speed = 1;
    bool canMove;
    Vector3 target;
    [SerializeField] private MoveManager moveManager;
    [SerializeField] private GridManager grid;
    Queue<Vector3Int> pathCells = new Queue<Vector3Int>();
    Enemy_Cac enemy_Cac;

    void Awake()
    {
        player = Player.Instance;
        grid = GridManager.Instance;
        enemy_Cac = GetComponent<Enemy_Cac>();
    }
    
    
    public void TurnManager(Vector3Int self, Vector3Int target, EnemyStat stat)
    {
        float distance = Vector3Int.Distance(target, self);
        if(distance < stat.actionRange)
        {
            Action();
        }
        else
        {
            StartCoroutine(MoveTowardPlayer(self, target, stat.pm_max));
        }
    }
    public IEnumerator MoveTowardPlayer(Vector3Int start, Vector3Int target, int pm)
{
    List<Vector3Int> path = moveManager.GenerateManhattanPath(start, target);
    pathCells.Clear();

    foreach (var cell in path)
        pathCells.Enqueue(cell);

    TrySetNextTarget();

    if (pathCells.Count == 0)
    {
        enemy_Cac.turn = false;
        yield break;
    }

    while (pm > 0 && pathCells.Count > 0)
    {
        Vector3 nextPos = target; // ta cible courante issue de TrySetNextTarget

        // Déplace vers la prochaine case
        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            yield return null; // <-- laisse Unity render le frame
        }

        transform.position = nextPos; // snap propre sur la case
        pm--;
        TrySetNextTarget();

        if (pathCells.Count > 0)
            nextPos = target;
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

    target = grid.groundTilemap.CellToWorld(nextCell);
    target.z += 2f;
}
}
