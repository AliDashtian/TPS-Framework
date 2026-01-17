using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Audio/Sound")]
public class SoundSO : ScriptableObject
{
    [Header("Audio Data")]
    public AudioClip[] clips; // Array for variation (e.g., 3 different gunshot sounds)
    public AudioMixerGroup mixerGroup;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    [Header("Randomization")]
    public bool randomizePitch = true;
    [Range(0f, 0.5f)] public float randomPitchRange = 0.1f;

    [Header("Spatial (3D)")]
    [Range(0f, 1f)] public float spatialBlend = 1f; // 1 = 3D, 0 = 2D
    public float minDistance = 1f;
    public float maxDistance = 50f;

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}
