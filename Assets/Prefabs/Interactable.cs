using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Interactable: MonoBehaviour
{
    public Item item;
    public string message;
    public virtual void InteractItem() 
    {

    }

    public virtual string Message()
    {
        return message;
    }
}
