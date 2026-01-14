using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    private int turnCount;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI hpText;
    void Update()
    {
        turnText.text = turnCount.ToString();
        hpText.text = player.hpPlayer.ToString();
    }

    public void NextTurn()
    {
        player.pmPlayer = player.maxPM_player;
        turnCount++;
        //Faire jouer les ennemis
    }
}
