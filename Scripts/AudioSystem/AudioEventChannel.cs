using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SFX Channel", menuName = "Scriptable Objects/Audio/Audio Channel")]
public class AudioEventChannel : ScriptableObject
{
    // The "Action" that listeners can subscribe to.
    // We pass the Sound Data and the Position.
    public UnityAction<SoundSO, Vector3> On3DSoundRequest;

    public UnityAction<SoundSO> On2DSoundRequest;

    public void Play3DSound(SoundSO sound, Vector3 position)
    {
        if (On3DSoundRequest != null)
        {
            On3DSoundRequest.Invoke(sound, position);
        }
        else
        {
            Debug.LogWarning("Audio Request made, but nobody is listening!");
        }
    }

    /// <summary>
    /// This method is great for UI buttons, just reference a SO and play a 2D sound
    /// </summary>
    /// <param name="sound"></param>
    public void Play2DSound(SoundSO sound)
    {
        if (On2DSoundRequest != null)
        {
            On2DSoundRequest.Invoke(sound);
        }
        else
        {
            Debug.LogWarning("Audio Request made, but nobody is listening!");
        }
    }
}