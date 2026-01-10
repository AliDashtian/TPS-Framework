using System.Threading.Tasks;
using UnityEngine;

public class SniperScope : MonoBehaviour
{
    [SerializeField]
    private GameObject _scope;

    [SerializeField]
    private GameObject _crosshair;

    [SerializeField]
    private int _scopeEnableDelay = 300;

    private Weapon _weapon;

    private void Awake()
    {
        if (TryGetComponent(out _weapon))
        {
            _weapon.OnAimed += SetScopeActive;
        }
    }

    async void SetScopeActive(bool active)
    {
        if (active)
        {
            await Task.Delay(_scopeEnableDelay);
        }

        _scope.SetActive(active);

        if (_crosshair != null)
        {
            _crosshair.SetActive(!active);
        }
    }
}
