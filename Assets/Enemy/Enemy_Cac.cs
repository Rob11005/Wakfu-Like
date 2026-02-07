using UnityEngine;

public class Enemy_Cac : MonoBehaviour
{
    public EnemyStat stat;
    EnemyTurn enemyTurn;
    public bool turn;
    GridManager grid;

    void Start()
    {
        grid = GridManager.Instance;
        enemyTurn = EnemyTurn.Instance;
        if (enemyTurn == null)
            Debug.LogError("EnemyTurn.Instance est NULL");
    }
    void Update()
    {
        if(turn)
        {
            Vector3Int startPos = grid.groundTilemap.WorldToCell(transform.position);
            Vector3Int targetPos = grid.groundTilemap.WorldToCell(Player.Instance.transform.position);
            enemyTurn.TurnManager(startPos, targetPos, stat);
        }
    }
}
