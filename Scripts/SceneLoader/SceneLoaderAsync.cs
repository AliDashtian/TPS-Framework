using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderAsync : MonoBehaviour
{
    public Slider LoadingSlider;

    public void LoadSceneBtn(int sceneIndex)
    {
        // We call a separate async method to handle the await logic
        // The "_ =" is a way to tell the compiler "we know we aren't awaiting this, just let it run"
        _ = LoadSceneRoutine(sceneIndex);
    }

    private async UniTask LoadSceneRoutine(int index)
    {
        LoadingSlider.gameObject.SetActive(true);

        await LoadSceneAsync(index, (progress) =>
        {
            LoadingSlider.value = progress;
        });

        Debug.Log("Loading Complete!");
    }

    public async UniTask LoadSceneAsync(int index, Action<float> onProgress)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        // Prevent the scene from flipping immediately if you want to hold at 100%
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Calculate progress (Unity stops at 0.9, so we scale it to 0-1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Report the progress back to whoever called this method
            onProgress?.Invoke(progress);

            // If we are effectively done (at 0.9), finish up
            if (operation.progress >= 0.9f)
            {
                // Optional: You could add a small delay here if the load is too fast
                // await Task.Delay(1000); 

                operation.allowSceneActivation = true;
            }

            // Wait for next frame
            await UniTask.Yield();
        }

        // Ensure we send the final 1.0 (100%) in case the loop exited early
        onProgress?.Invoke(1f);
    }
}
