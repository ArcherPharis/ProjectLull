using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.OnHit();
            player.DealDamage(enemy.meleeDamageAmount);
        }
    }
}
