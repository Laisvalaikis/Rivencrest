using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleSpear : Consumable
{
    public override void PickUp(GameObject WhoStepped)
    {
        if (WhoStepped.gameObject.tag == "Player")
        {
            if (WhoStepped.GetComponent<ActionManager>().FindActionByName("ThrowSpear") &&
                 WhoStepped.GetComponent<ActionManager>().FindActionByName("ThrowSpear").spawnedCharacter == transform.parent.gameObject)
            {
                WhoStepped.GetComponent<ActionManager>().FindActionByName("ThrowSpear").SpecificAbilityAction();
                Destroy(gameObject.transform.parent.gameObject);
            }
            //WhoStepped.GetComponent<PlayerInformation>().MaxHealth += HealthAmount;
            // WhoStepped.GetComponent<PlayerInformation>().health += HealthAmount;
            //transform.parent.gameObject.SetActive(false);
        }
    }
}
