using UnityEngine;

public class CharacterSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioEventChannel _sfxChannel;

    private BaseCharacter _character;

    private void Awake()
    {
        _character = GetComponent<BaseCharacter>();
    }

    private void OnEnable()
    {
        _character.OnTriggerPulled += PlayFireSound;
    }

    private void OnDisable()
    {
        _character.OnTriggerPulled -= PlayFireSound;
    }

    void PlayFireSound(bool newFire)
    {
        _sfxChannel.Play3DSound(_character.GetCurrentWeapon().WeaponData.FireSound, transform.position);
    }
}
