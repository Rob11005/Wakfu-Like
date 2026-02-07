using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance = null;
    public static Player Instance => _instance;
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
}
