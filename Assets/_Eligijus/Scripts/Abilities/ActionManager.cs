using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public Sprite AbilityBackground;
    public List<ActionList> ActionScripts;
    [HideInInspector] public bool hasSlowAbilityBeenCast = false;

    public BaseAction FindActionByName(string name)
    {
        for(int i = 0; i < ActionScripts.Count; i++)
        {
            if(ActionScripts[i].actionName == name)
            {
                return ActionScripts[i].action;
            }
        }
        return null;
    }
    public ActionList FindActionListByName(string name)
    {
        for (int i = 0; i < ActionScripts.Count; i++)
        {
            if (ActionScripts[i].actionName == name)
            {
                return ActionScripts[i];
            }
        }
        return null;
    }
    public ActionList FindActionByIndex(int index)
    {
        for (int i = 0; i < ActionScripts.Count; i++)
        {
            if (ActionScripts[i].AbilityIndex == index)
            {
                return ActionScripts[i];
            }
        }
        return null;
    }
    public void RemoveAllActionPoints() 
    {
        /*for (int i = 0; i < ActionScripts.Count; i++)
        {
            ActionScripts[i].action.RemoveActionPoints();
        }*/
        GetComponent<GridMovement>().AvailableMovementPoints=0;//debatable
        hasSlowAbilityBeenCast = true;

    }
    public void RemoveAttackActionPoints()
    {
        for (int i = 0; i < ActionScripts.Count; i++)
        {
            if (ActionScripts[i].action.AttackAbility)
            {
                ActionScripts[i].action.RemoveActionPoints();
            }
        }
    }
    public void AddAvailableAttackToAll()//prideda visiems po viena
    {
        for (int i = 0; i < ActionScripts.Count; i++)
        {
            ActionScripts[i].action.AvailableAttacks++;
        }
    }
    public void ActivateBlessingBuffs()
    {
        for (int i = 0; i < ActionScripts.Count; i++)
        {
            //if (ActionScripts[i].AbilityIndex >= 0 )
            //{
                ActionScripts[i].action.BuffAbility();
            //}
        }
    }
}
[System.Serializable]
public class ActionList
{
    public BaseAction action;
    public string actionName;
    public Sprite AbilityIcon;
    public int AbilityIndex;

}
