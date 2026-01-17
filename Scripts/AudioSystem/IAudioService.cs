using UnityEngine;

public interface IAudioService
{
    // Play a sound at a specific position (3D)
    void PlaySound(SoundSO sound, Vector3 position);

    // Play a sound continuously (2D - UI or Music)
    void PlayGlobal(SoundSO sound);
}