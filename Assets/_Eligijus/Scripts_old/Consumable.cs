using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public abstract class Consumable : MonoBehaviour
    {
        public bool Stop = false;
        public virtual void PickUp(GameObject WhoStepped)
        {
        }
    }
