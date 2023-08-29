using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    // [SerializeField] private RaiseRock _raiseRock;
    // [SerializeField] private WallSmash _wallSmash;
    // [SerializeField] private Cage _cage;
    // [SerializeField] private SwordPush _swordPush;
    
    
    public void Die()
    {
        transform.parent.GetComponent<PlayerInformation>().Die();
    }
    public void DestroyProp() 
    {
        Destroy(transform.parent.gameObject);
    }
    public void RaiseRockAnimationEnd()
    {
        // _raiseRock.RaiseRockAnimationEnd();
    }
    public void WallSmashAnimationEnd()
    {
        // _wallSmash.WallSmashAnimationEnd();
    }
    public void FreeCagedCharacter()
    {
        // _cage.FreeCagedCharacter();
    }
    public void SwordPushAnimationStart()
    {
        // _swordPush.SwordPushAnimationStart();
    }
    public void SwordPushAnimationEnd()
    {
        // _swordPush.SwordPushAnimationEnd();
    }
}
