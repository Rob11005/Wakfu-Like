using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;
    public Player player;
    
    private int turnCount;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI hpText;
    public TurnState currentState;
    
    [SerializeField] private List<EnemyTurn> enemies = new List<EnemyTurn>();
    private int currentEnemyIndex = 0;
    void Awake()
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
    }
    void Start()
    {
        player = Player.Instance;
        // Trouver tous les ennemis dans la scène
        enemies = FindObjectsOfType<EnemyTurn>().ToList();
        StartPlayerTurn();
    }
    void Update()
    {
        turnText.text = turnCount.ToString();
        hpText.text = player.stats.hp_current.ToString();
    }

    public void StartPlayerTurn()
    {
        currentState = TurnState.PlayerTurn;
        player.stats.ResetStats();
    }

    public void StartEnemyTurn()
    {
        currentState = TurnState.EnemyTurn;
        currentEnemyIndex = 0;
        PlayNextEnemy();
    }

    public void PlayNextEnemy()
    {
        if(currentEnemyIndex >= enemies.Count)
        {
            // Tous les ennemis ont joué
            StartPlayerTurn();
            return;
        }

        enemies[currentEnemyIndex].StartTurn();
        Debug.Log(currentState);
        currentEnemyIndex++;
    }

    public void OnEndTurnButton()
    {
        StartEnemyTurn();
    }
}
