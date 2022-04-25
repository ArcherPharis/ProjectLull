using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string weaponName;
    [SerializeField] float weaponDamage;
    [SerializeField] int currentAmmo;
    [SerializeField] int maxCapacity;
    [SerializeField] float fireRate;
    [SerializeField] float weaponAccuracy;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem[] muzzleEffect;
    public Inventory inventory;
    [Tooltip("Order: Fire, Dryfire, Reload, Equip")]
    [SerializeField] AudioClip[] audioClips;
    public GameObject hitEffect;
    public GameObject fireEffect;
    public Transform firingPoint;
    public Sprite weaponSprite;
    public int AnimationID;
    [SerializeField] int ammoType;

    public string WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
        
    }

    public int AmmoType
    {
        get { return ammoType; }
        set { ammoType = value; }
    }

    public int CurrentAmmo
    {
        get { return currentAmmo; }
        set { currentAmmo = Mathf.Clamp(value, 0, MaxCapacity); }
    }

    public float WeaponAccuracy
    {
        get { return weaponAccuracy; }
        set { weaponAccuracy = value; }
    }

    public float WeaponDamage
    {
        get { return weaponDamage; }
        set { weaponDamage = value; }
    }
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    public int MaxCapacity
    {
        get { return maxCapacity; }
        set { maxCapacity = value; }
    }


    // Start is called before the first frame update
    void Start()
    {

        muzzleEffect = GetComponentsInChildren<ParticleSystem>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartFiring()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();

        foreach(ParticleSystem ps in muzzleEffect)
        {
            ps.Play();
        }
        
    }
    public virtual void OutOfAmmo()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    public virtual void OnSwapWeapon()
    {
       
    }

    public virtual void ReloadWeapon()
    {
        if (CurrentAmmo != MaxCapacity && AmmoType != 0) 
        {
            Debug.Log("If we see this at max capacity we did something wrong.");
            int remainder = MaxCapacity - CurrentAmmo;

            if (remainder > AmmoType && AmmoType != 0)
            {
                Debug.Log("Can't get a fresh mag");
                CurrentAmmo += AmmoType;
                AmmoType = 0;
                return;
            }

            AmmoType -= remainder;
            CurrentAmmo = MaxCapacity;
        }

    }



    public void SetBulletDamage()
    {
        BulletProjectile[] bp = fireEffect.GetComponentsInChildren<BulletProjectile>();
        foreach(BulletProjectile bulletProj in bp)
        {
            bulletProj.damage = weaponDamage;
            bulletProj.SetBulletAccuracy(weaponAccuracy);
        }


        //bp.damage = weaponDamage;
    }
}
