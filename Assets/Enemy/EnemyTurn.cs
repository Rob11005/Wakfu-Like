using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    Player player;
    MoveManager moveManager;

    void Awake()
    {
        player = Player.Instance;
        moveManager = MoveManager.Instance;
    }
    public void TurnManager(Vector3Int self, Vector3Int target, int range)
    {
        float distance = Vector3Int.Distance(target, self);
        if(distance < range)
        {
            Action();
        }
        else
        {
            MoveTowardPlayer(self, target);
        }


    }
    public void MoveTowardPlayer(Vector3Int start, Vector3Int target)
    {
        moveManager.GenerateManhattanPath(start, target);

        
    }

    public void Action()
    {
        
    }
}
