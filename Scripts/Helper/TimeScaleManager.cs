using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    private void Start()
    {
        ChangeTimeScale(1);
    }

    public static void ChangeTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
