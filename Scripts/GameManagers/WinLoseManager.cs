using System.Threading.Tasks;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    public RuntimeSet<PlayerCharacter> PlayerSet;
    public RuntimeSet<Enemy> EnemySet;

    public GameEvent OnGameWon;
    public GameEvent OnGameLost;

    [Tooltip("Delay in miliseconds to show Win or Lose popups")]
    [SerializeField]
    private int _popupDelay = 500;

    private bool _hasWon = false;
    private bool _hasLost = false;

    private void OnEnable()
    {
        PlayerSet.OnItemAdded += Revive;
        PlayerSet.OnItemRemoved += Lose;
        EnemySet.OnSetEmptied += Win;
    }

    private void OnDisable()
    {
        PlayerSet.OnItemAdded -= Revive;
        PlayerSet.OnItemRemoved -= Lose;
        EnemySet.OnSetEmptied -= Win;
    }

    public async void Win()
    {
        if (_hasWon) return;

        await Task.Delay(_popupDelay);

        OnGameWon.Raise();
        _hasWon = true;
    }

    public async void Lose()
    {
        if (_hasLost) return;

        await Task.Delay(_popupDelay);

        OnGameLost.Raise();
        _hasLost = true;
    }

    public void Revive()
    {
        _hasLost = false;
    }
}
