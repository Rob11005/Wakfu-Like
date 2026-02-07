using UnityEngine;

[CreateAssetMenu(menuName = "enemies")]
public class EnemyStat : ScriptableObject
{
    public int hp_max;
    public int pm_max;
    public int pa_max;
    public int initiative;
    public int actionRange;
}
