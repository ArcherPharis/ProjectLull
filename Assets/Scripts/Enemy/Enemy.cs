using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Damagable
{
    [SerializeField] Player player;
    public float meleeDamageAmount;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider playerDamageHitBox;
    NavMeshAgent agent;
    bool hitFlinch;
    int animationSpeed;
    int deathIndex;
    public float speed;
    public float rotateSpeed;
    float _health;

    private void Awake()
    {
        animationSpeed = Animator.StringToHash("Speed");
        deathIndex = Animator.StringToHash("AnimIndex");
        agent = GetComponent<NavMeshAgent>();
        _health = Health;
    }

    private void Update()
    {
        if (hitFlinch)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * 10f));
        }
    }
    public override void Die()
    {
        base.Die();
        CapsuleCollider hitbox = GetComponent<CapsuleCollider>();
        hitbox.enabled = false;
        Destroy(gameObject, 5f);
        animator.SetFloat(deathIndex, Random.Range(0, 2));
        animator.SetTrigger("Die");
        
    }
    public void SetConfusedState(bool condition)
    {
        animator.SetBool("Confused", condition);
    }


    public bool OnHealthChange()
    {
        if(_health != Health)
        {
            _health = Health;
            return true;
            
        }
        return false;
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

    public void EnemyHitReaction()
    {
        agent.isStopped = true;
    }

    public void EnemyHitRecovery()
    {
        Debug.Log("I recovered");
        agent.isStopped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Health >= 0)
        {
            if (collision.gameObject.GetComponent<BulletProjectile>())
            {
                BulletProjectile bulletProjectile = collision.gameObject.GetComponent<BulletProjectile>();
                if (bulletProjectile.bulletType == BulletProjectile.BulletType.Pistol)
                {
                    Debug.Log("I got hit with a pistol");
                    animator.SetLayerWeight(1, 1);
                    StartCoroutine(SetWeightBack());
                    hitFlinch = true;
                    //StopCoroutine(SetWeightBack());
                }
                else if (bulletProjectile.bulletType == BulletProjectile.BulletType.Shotgun)
                {
                    animator.SetTrigger("Superflinch");
                }
            }
        }
    }

    IEnumerator SetWeightBack()
    {
        yield return new WaitForSeconds(0.45f);
        hitFlinch = false;
    }






}
