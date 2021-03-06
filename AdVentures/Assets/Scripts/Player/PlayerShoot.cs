﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : MonoBehaviour, IShotLocations
{    
    [SerializeField]
    float shootSpeed = 15f;

    [Header("Shot Locations")]
    [SerializeField]
    private Transform left;
    [SerializeField]
    private Transform normal;
    [SerializeField]
    private Transform right;

    private float lastShot;
    private float shotThreshold = 10f;
    private float shotForce = 300f;
    private Projectile projectile;
    private bool isShootingDisabled;
    private Animator animator;

    private List<WeaponType> currentWeapons = new List<WeaponType>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentWeapons.Add(new WeaponType(WeaponUpgradeTypeEnum.Normal, null));
    }

    private void Update()
    {
        //foreach (var item in currentWeapons)
        //{
        //    item.UpdateTime(Time.deltaTime);
        //    if (item.ExpiresInSeconds.HasValue && item.ExpiresInSeconds <= 0)
        //        StartCoroutine(RemoveWeaponAtEndOfFrame(item));
        //}
    }

    private void LateUpdate()
    {
        if (CanShoot())
        {
            foreach (var weapon in currentWeapons)
            {
                ShotFactory.Fire(weapon.Weapon, projectile, shotForce, this);
                animator.SetTrigger("PlayerShoot");
            }
        }
    }

    private bool CanShoot()
    {
        if (isShootingDisabled)
            return false;

        lastShot += Time.deltaTime * shootSpeed;

        if (lastShot > shotThreshold)
        {
            lastShot = 0f;
            return true;
        }

        return false;
    }

    private IEnumerator RemoveWeaponAtEndOfFrame(WeaponType type)
    {
        yield return new WaitForEndOfFrame();

        currentWeapons.Remove(type);
    }

    #region Public Methods

    public void IncreaseShotSpeed()
    {
        shotThreshold *= .9f;
    }

    public Transform Left()
    {
        return left;
    }

    public Transform Normal()
    {
        return normal;
    }

    public Transform Right()
    {
        return right;
    }

    public void AddWeapon(WeaponUpgradeTypeEnum type, float? expireInSeconds = null)
    {
        var existingType = currentWeapons.FirstOrDefault(x => x.Weapon == type);

        if (existingType == null)
            currentWeapons.Add(new WeaponType(type, expireInSeconds));
        else if (expireInSeconds.HasValue)
            existingType.ExpiresInSeconds = expireInSeconds.Value;
    }

    public void SetProjectile(Projectile projectile)
    {
        this.projectile = projectile;
    }

    public void ToggleShootingEnabled(bool isEnabled)
    {
        isShootingDisabled = !isEnabled;
    }

    public List<WeaponUpgradeTypeEnum> GetCurrentWeapons()
    {
        return currentWeapons.Select(x => x.Weapon).ToList();
    }

    #endregion
}
