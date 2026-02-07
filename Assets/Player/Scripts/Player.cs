using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance = new Player();
    public int pmPlayer;
    public int maxPM_player = 6;
    public int initiative;
    public int hpPlayer;
    public int maxHP_player = 50;

    void Start()
    {
        pmPlayer = maxPM_player;
        hpPlayer = maxHP_player;
    }

    public static Player Instance
    {
        get
        {
            return _instance;
        }
    }
}
