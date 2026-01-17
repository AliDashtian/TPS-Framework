using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour, IAudioService
{
    public static AudioManager Instance { get; private set; }

    /// <summary>
    /// Using event channel is more modular and generally better but I'm using singleton because it's easier to call
    /// and I don't have a designer teammate to wory about.
    /// It's still better to use this for UI button sounds
    /// </summary>
    [SerializeField] private AudioEventChannel sfxChannel;

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 30; // 30 simultaneous sounds is usually enough for mobile
    [SerializeField] private GameObject audioSourcePrefab;

    private IObjectPool<AudioSource> _sourcePool;

    private void Awake()
    {
        InitializeSingleton();

        _sourcePool = new ObjectPool<AudioSource>(
            createFunc: () => 
            {
                var obj = Instantiate(audioSourcePrefab, transform);
                return obj.GetComponent<AudioSource>();
            },
            actionOnGet: (source) => 
            {
                source.volume = 1f;
                source.pitch = 1f;
            },
            actionOnRelease: (source) => 
            {
                source.Stop();
                source.clip = null;
            },
            actionOnDestroy: (source) =>
            {

            },
            collectionCheck: true,
            defaultCapacity: initialPoolSize
        );
    }

    private void OnEnable()
    {
        sfxChannel.On3DSoundRequest += PlaySound;
        sfxChannel.On2DSoundRequest += PlayGlobal;
    }

    private void OnDisable()
    {
        sfxChannel.On3DSoundRequest -= PlaySound;
        sfxChannel.On2DSoundRequest -= PlayGlobal;
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- IAudioService Implementation ---

    public void PlaySound(SoundSO sound, Vector3 position)
    {
        if (!CanPlay(sound)) return;

        AudioSource source = _sourcePool.Get();
        ConfigureSource(source, sound);

        source.transform.position = position;
        source.Play();

        // We pass the source.pitch to calculate exact duration
        ReturnToPoolDelayed(source, source.clip.length, source.pitch).Forget();
    }

    public void PlayGlobal(SoundSO sound)
    {
        if (!CanPlay(sound)) return;

        AudioSource source = _sourcePool.Get();
        ConfigureSource(source, sound);

        source.spatialBlend = 0f; // Force 2D
        source.Play();

        // We pass the source.pitch to calculate exact duration
        ReturnToPoolDelayed(source, source.clip.length, source.pitch).Forget();
    }

    // --- Helper Methods ---

    private bool CanPlay(SoundSO sound)
    {
        return sound != null && sound.clips.Length > 0;
    }

    private void ConfigureSource(AudioSource source, SoundSO sound)
    {
        source.clip = sound.GetRandomClip();
        source.outputAudioMixerGroup = sound.mixerGroup;
        source.volume = sound.volume;

        // Handle Pitch Randomization (Vital for TPS guns)
        if (sound.randomizePitch)
        {
            source.pitch = sound.pitch + Random.Range(-sound.randomPitchRange, sound.randomPitchRange);
        }
        else
        {
            source.pitch = sound.pitch;
        }

        // Spatial settings
        source.spatialBlend = sound.spatialBlend;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear; // Cheaper calculation for mobile
    }

    private async UniTaskVoid ReturnToPoolDelayed(AudioSource source, float clipLength, float pitch)
    {
        float duration = clipLength / Mathf.Abs(pitch);

        // If the AudioManager is destroyed (Scene Change), this cancels the timer 
        // so we don't try to access a destroyed pool.
        var token = this.GetCancellationTokenOnDestroy();

        // suppressCancellationThrow: true prevents an error message in the console when you quit the game.
        bool canceled = await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: token)
                                     .SuppressCancellationThrow();

        if (canceled) return; // If we quit/changed scenes, stop here.

        source.Stop();
        _sourcePool.Release(source);
    }
}