using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

public class VfxManager : MonoBehaviour
{
    public static VfxManager Instance;

    [Header("Channels")]
    [SerializeField] private VfxEventChannel _vfxChannel;

    [Header("Settings")]
    [SerializeField] private int _defaultPoolSize = 10;
    [SerializeField] private int _maxPoolSize = 50;

    [SerializeField] private float _VfxDisableDelay = 3f;

    // A Dictionary mapping the "Prefab Asset" to a "Pool of Instances"
    private Dictionary<ParticleSystem, IObjectPool<ParticleSystem>> _pools =
        new Dictionary<ParticleSystem, IObjectPool<ParticleSystem>>();

    private void Awake()
    {
        InitializeSingleton();
    }

    private void OnEnable()
    {
        _vfxChannel.OnPlayVfx += PlayVfx;
    }

    private void OnDisable()
    {
        _vfxChannel.OnPlayVfx -= PlayVfx;
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

    public void PlayVfx(ParticleSystem prefab, Vector3 position, Quaternion rotation)
    {
        // 1. Get (or create) the specific pool for this prefab
        IObjectPool<ParticleSystem> pool = GetPoolForPrefab(prefab);

        // 2. Spawn from pool
        ParticleSystem instance = pool.Get();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.Play();

        // 3. Auto-return logic (using UniTask)
        ReturnToPoolDelayed(instance, pool).Forget();
    }

    private IObjectPool<ParticleSystem> GetPoolForPrefab(ParticleSystem prefab)
    {
        // If we already have a pool for this specific rock/blood/spark prefab, return it
        if (_pools.TryGetValue(prefab, out var pool))
        {
            return pool;
        }

        // Otherwise, create a new pool dynamically
        var newPool = new ObjectPool<ParticleSystem>(
            createFunc: () => Instantiate(prefab, transform), // Create new
            actionOnGet: (ps) => ps.gameObject.SetActive(true),
            actionOnRelease: (ps) => ps.gameObject.SetActive(false),
            actionOnDestroy: (ps) => Destroy(ps.gameObject),
            defaultCapacity: _defaultPoolSize,
            maxSize: _maxPoolSize
        );

        _pools.Add(prefab, newPool);
        return newPool;
    }

    private async UniTaskVoid ReturnToPoolDelayed(ParticleSystem instance, IObjectPool<ParticleSystem> pool)
    {
        // Get duration from the main module
        float duration = instance.main.duration + _VfxDisableDelay;

        // Wait for it to finish (handling scene destruction safely)
        var token = this.GetCancellationTokenOnDestroy();
        bool canceled = await UniTask.Delay(System.TimeSpan.FromSeconds(duration), cancellationToken: token)
                                     .SuppressCancellationThrow();

        if (canceled) return;

        // Return to the specific pool it came from
        pool.Release(instance);
    }
}