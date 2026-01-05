using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour
{
    public RuntimeSet<PlayerCharacter> PlayerSet;
    public RuntimeSet<Enemy> EnemySet;

    public GameEvent OnGameWon;
    public GameEvent OnGameLost;

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

    public void Win()
    {
        if (_hasWon) return;

        OnGameWon.Raise();
        _hasWon = true;
    }

    public void Lose()
    {
        if (_hasLost) return;

        OnGameLost.Raise();
        Time.timeScale = 0.6f;
        _hasLost = true;
    }

    public void Revive()
    {
        _hasLost = false;
    }

    public async UniTask LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //LoadingSlider.value = progress;

            await UniTask.Yield();
        }
    }
}
