using UnityEngine;

public class EnemyStat : UnitStats
{
    void Start()
    {
       pm_current = pm_max; 
       pa_current = pa_max;
    }
}
