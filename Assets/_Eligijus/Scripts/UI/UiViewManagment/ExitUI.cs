using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ExitUI : MonoBehaviour
{
   
   
   
   public void Escape()
   {
      UIStack.QuitLast();
   }
}
