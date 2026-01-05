using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// List of all of the alive enemies
    /// </summary>
    public RuntimeSet<Enemy> EnemySet;

    /// <summary>
    /// List of all of the alive players
    /// </summary>
    public RuntimeSet<PlayerCharacter> PlayerSet;

    private void Awake()
    {
        AssignEnemyTarget();
    }

    private void AssignEnemyTarget()
    {
        foreach (Enemy enemy in EnemySet.Items)
        {
            if (enemy.Target == null)
            {
                int randomPlayerIndex = Random.Range(0, PlayerSet.Count());
                enemy.Target = PlayerSet.Items[randomPlayerIndex].transform;
            }
        }
    }
}
