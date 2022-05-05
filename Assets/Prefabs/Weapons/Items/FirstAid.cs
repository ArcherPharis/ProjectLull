using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utility Items/First Aid")]
public class FirstAid : Item
{
    [SerializeField] float healAmount;

    public override void GiveItemToInventory()
    {
        base.GiveItemToInventory();
    }

    public override void ApplyItemEffects()
    {
        base.ApplyItemEffects();
        Player player = GameObject.Find("Player").GetComponent<Player>();

        if (player.Health >= healAmount)
        {
            Debug.Log("The health is gonna go over.");
            player.Health = player.MaxHealth;
        }
        else if(player.Health <= healAmount)
        {
            player.Health += healAmount;
        }
        

    
        
    }
}
