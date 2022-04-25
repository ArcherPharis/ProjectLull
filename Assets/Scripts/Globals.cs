using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool teleportationInUse = false;
    
}

public class GlobalMethods: MonoBehaviour
{
    public static GlobalMethods gm;

    private void Start()
    {
        GlobalMethods.gm = this;
    }
}
