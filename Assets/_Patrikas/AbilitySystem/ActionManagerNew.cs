using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerNew : MonoBehaviour
{
    [SerializeField]
    private List<Ability> _abilities;
    

    public List<Ability> ReturnAbilities()
    {
        return _abilities;
    }
}

