using UnityEngine;

public class RecoilSystem
{
    private WeaponRecoilSO _data;
    private Vector2 _currentRecoil;
    private Vector2 _targetRecoil;
    private float _lastFireTime;

    public RecoilSystem(WeaponRecoilSO data)
    {
        _data = data;
    }

    public void UpdateRecoilTarget()
    {
        if (_data == null) return;

        // Reset the timer so we don't return while firing
        _lastFireTime = Time.time;

        // Calculate random kick
        // Fix: Use Min/Max correctly to avoid Up/Down jitter
        float randomX = Random.Range(_data.RecoilKickX.x, _data.RecoilKickX.y);
        float randomY = Random.Range(_data.RecoilKickY.x, _data.RecoilKickY.y);

        // Add to target (Assuming Negative Y is UP in your camera setup)
        _targetRecoil += new Vector2(randomX, -randomY);
    }

    public void ApplyRecoil(ref float yaw, ref float pitch)
    {
        if (_data == null) return;

        // 1. THE FIX: Only start returning if we haven't fired recently
        if (Time.time > _lastFireTime + _data.RecoilRecoveryDelay)
        {
            _targetRecoil = Vector2.Lerp(_targetRecoil, Vector2.zero, _data.RecoilReturnSpeed * Time.deltaTime);
        }

        // 2. Snap Current to Target
        Vector2 wantedRecoil = Vector2.Lerp(_currentRecoil, _targetRecoil, _data.RecoilSnappiness * Time.deltaTime);

        // 3. Apply Delta
        Vector2 delta = wantedRecoil - _currentRecoil;
        _currentRecoil = wantedRecoil;

        yaw += delta.x;
        pitch += delta.y;
    }
}
