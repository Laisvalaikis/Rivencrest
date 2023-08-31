using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindDart : BaseAction
{
    // Start is called before the first frame update
    private GameObject BlindedTarget = null;
    void Start()
    {
        actionStateName = "BlindDart";
        isAbilitySlow = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTurnStart()
    {
        
    }
}
