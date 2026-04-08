using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy_Cac enemy_Cac;
    private int turnCount;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI hpText;
    void Start()
    {
        player = Player.Instance;
    }
    void Update()
    {
        turnText.text = turnCount.ToString();
        hpText.text = player.stats.hp_current.ToString();
    }

    public void NextTurn()
    {
        player.stats.pm_current = player.stats.pm_max;
        turnCount++;
        enemy_Cac.turn = true;
    }
}
