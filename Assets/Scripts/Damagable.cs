using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable: MonoBehaviour
{
    [SerializeField] float health = 100;

    public float Health
    {
        get { return health = Mathf.Clamp(health, 0, health); }
        set { health = value;  }
    }

    public void DealDamage(float damageDealt)
    {
        Health -= damageDealt;
    }



    public virtual void Die()
    {

        Debug.Log("I'M DEAD!!!!!");
            

    }

    
}
