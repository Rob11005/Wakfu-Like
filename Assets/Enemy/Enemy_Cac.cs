using UnityEngine;

public class Enemy_Cac : MonoBehaviour
{
    public UnitStats stat;
    EnemyTurn enemyTurn;

    void Start()
    {
        enemyTurn = GetComponent<EnemyTurn>();
        if (enemyTurn == null)
            Debug.LogError("EnemyTurn.Instance est NULL");
    }
    void Update()
    {

    }
}
