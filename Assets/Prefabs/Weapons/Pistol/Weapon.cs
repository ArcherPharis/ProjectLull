using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string weaponName;
    [SerializeField] float weaponDamage;
    [SerializeField] int maxCapacity;
    public ParticleSystem[] muzzleFlash;
    public Transform firingPoint;
    public Sprite weaponSprite;
    public bool isFiring = false;

    public string WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
        
    }

    public float WeaponDamage
    {
        get { return weaponDamage; }
        set { weaponDamage = value; }
    }

    public int MaxCapacity
    {
        get { return maxCapacity; }
        set { maxCapacity = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        //may not work on changed weapons idk, could move to update, let's see. Might also have to be part of a method.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartFiring()
    {
        isFiring = true;
        foreach(ParticleSystem muzzleFlashes in muzzleFlash)
        {
            muzzleFlashes.Play();
        }

        
    }
    public virtual void StopFiring()
    {
        isFiring = false;
    }
}
