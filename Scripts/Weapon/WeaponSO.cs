using System;
using Unity.Cinemachine;
using UnityEngine;

public abstract class WeaponSO : ScriptableObject
{
    public string Name;

    public WeaponFireType FireType;
    public DamageType DamageType;

    [Tooltip("Used to determine which holster to attach the weapon to")]
    public WeaponSocketTypes HolsterType;

    public PhysicalMaterialDataBaseSO PhysicalMaterialData;
    public RecoilDataSO RecoilData;

    public float Damage = 1;
    public float FireRate = 5;
    public float FireRange = 1000;
    public float ZoomSpeed = 0.1f;
    public float ReloadDuration = 2f;
    public float ImpactForce = 5f;

    public int ZoomFOV = 40;
    public int PouchCapacity = 90;
    public int MagCapacity = 30;

    public LayerMask ShootableLayers;

    public WeaponAttachmentPositions WeaponAttachmentPositions;

    [Header("Visual")]
    public AnimatorOverrideController OverrideController;
    public string EquipAnimName;
    public string UnequipAnimName;

    public Sprite CrosshairSprite;
    public Sprite WeaponSprite;

    public AudioClip FireSound;
    public ParticleSystem MuzzlePS;

    public bool DisableMeshIfNoAmmo;

    public abstract void Fire(Weapon weapon);
}

[Serializable]
public struct WeaponAttachmentPositions
{
    public Vector3 InHandPos;
    public Vector3 InHandRot;

    public Vector3 InHolsterPos;
    public Vector3 InHolsterRot;
}