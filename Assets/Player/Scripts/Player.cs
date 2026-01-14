using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int pmPlayer;
    public int maxPM_player = 6;

    public int hpPlayer;
    public int maxHP_player = 50;

    void Start()
    {
        pmPlayer = maxPM_player;
        hpPlayer = maxHP_player;
    }
}
