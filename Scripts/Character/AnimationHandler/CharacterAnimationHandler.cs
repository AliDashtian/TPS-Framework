using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private BaseCharacter _character;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<BaseCharacter>();

        _character.OnWeaponSwapped += UpdateRuntimeAnimatorController;

        UpdateRuntimeAnimatorController();
    }

    private void UpdateRuntimeAnimatorController()
    {
        if (_character.GetCurrentWeapon().WeaponData.OverrideController != null)
        _animator.runtimeAnimatorController = _character.GetCurrentWeapon().WeaponData.OverrideController;
    }
}
