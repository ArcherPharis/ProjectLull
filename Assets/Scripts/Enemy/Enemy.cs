using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damagable
{
    Inventory damageTakenFromPlayerBullet;
    [SerializeField] Player player;
    public float meleeDamageAmount;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider playerDamageHitBox;
    int animationSpeed;
    int deathIndex;
    public float speed;
    public float rotateSpeed;
    private void Awake()
    {
        animationSpeed = Animator.StringToHash("Speed");
        deathIndex = Animator.StringToHash("AnimIndex");
    }

    private void Update()
    {
        
    }
    public override void Die()
    {
        base.Die();
        Destroy(gameObject, 5f);
        animator.SetFloat(deathIndex, Random.Range(0, 2));
        animator.SetTrigger("Die");
        
    }

    public void ChangeMovementSpeed(float desiredValue, float dampTime)
    {
        animator.SetFloat(animationSpeed, desiredValue, dampTime, Time.deltaTime);
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void HitBoxOn()
    {
        playerDamageHitBox.enabled = true;
    }

    public void HitBoxOff()
    {
        playerDamageHitBox.enabled = false;
    }






}
