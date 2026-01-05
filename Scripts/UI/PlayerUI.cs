using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public RuntimeObject<PlayerCharacter> ActiveCharacter;

    public Text ammoText;
    public Image crosshairImage;
    public Image weaponImage;

    private void Start()
    {
        ActiveCharacter.Object.OnWeaponSwapped += UpdateWeaponUI;

        UpdateWeaponUI();
    }

    private void Update()
    {
        UpdateAmmoUI();
    }

    void UpdateWeaponUI()
    {
        if (ActiveCharacter.Object.GetCurrentWeapon() != null)
        {
            weaponImage.sprite = ActiveCharacter.Object.GetCurrentWeapon().WeaponData.WeaponSprite;
            crosshairImage.sprite = ActiveCharacter.Object.GetCurrentWeapon().WeaponData.CrosshairSprite;
        }
    }

    void UpdateAmmoUI()
    {
        if (ActiveCharacter.Object.GetCurrentWeapon() != null)
        {
            ammoText.text = $"{ActiveCharacter.Object.GetCurrentWeapon().CurrentMagAmmo} / {ActiveCharacter.Object.GetCurrentWeapon().CurrentPouchAmmo}";
        }
    }
}
