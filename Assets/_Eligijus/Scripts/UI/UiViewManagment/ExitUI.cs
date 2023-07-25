using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ExitUI : MonoBehaviour
{
    [SerializeField] private View pauseMenuView;
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
            Escape();
    }
   
   private void Escape()
   {
       if (UIStack.HasAnyViewToQuit())
       {
           UIStack.QuitLast();
       }
       else
       {
           if (pauseMenuView != null)
           {
               pauseMenuView.OpenView();
           }
       }
   }
}
