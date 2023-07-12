using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    public int Priority = 0;
    void Start()
    {
        //Changing Order in layer
        if (transform.Find("CharacterModel") != null)
        {
            transform.Find("CharacterModel").GetComponent<SpriteRenderer>().sortingOrder = int.Parse((Math.Floor(transform.position.y)).ToString()) * -10 + Priority;
        }
    }
}
