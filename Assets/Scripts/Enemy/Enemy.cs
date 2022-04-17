using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damagable
{
    public override void Die()
    {
        base.Die();
        Debug.Log("hello, I'm from the derived class, ready to do more specific tasks");
    }


}
