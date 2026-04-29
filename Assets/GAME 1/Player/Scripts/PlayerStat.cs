using UnityEngine;

public class PlayerStat : UnitStats
{
    void Start()
    {
        pm_current = pm_max;
    }
    
    public void ResetStats()
    {
        pm_current = pm_max;
        pa_current = pa_max;
    }
}
