using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindDart : BaseAction
{
    private GameObject BlindedTarget = null;
    void Start()
    {
        isAbilitySlow = false;
    }
    
    public override void OnTurnStart()
    {
        
    }
}
