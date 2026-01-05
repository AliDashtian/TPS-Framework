using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "Scriptable Objects/UI/SceneLoader")]
public class SceneLoaderSO : ScriptableObject
{   
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
