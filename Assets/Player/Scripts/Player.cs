using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance = null;
    public static Player Instance => _instance;
    public PlayerStat stats;

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
        stats = GetComponent<PlayerStat>();
    }

    void Start()
    {
        
    }
}
