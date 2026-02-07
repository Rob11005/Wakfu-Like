using UnityEngine;

public class Enemy_Cac : EnemyTurn
{
    public EnemyStat stat;
    public bool turn;
    GridManager grid;


    void Awake()
    {
        grid = GridManager.Instance;
    }
    void Update()
    {
        if(turn)
        {
            Vector3Int startPos = grid.groundTilemap.WorldToCell(transform.position);
            Vector3Int targetPos = grid.groundTilemap.WorldToCell(Player.Instance.transform.position);
            TurnManager(startPos, targetPos, stat.actionRange);
        }
    }
}
