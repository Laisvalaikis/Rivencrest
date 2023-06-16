using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleWasPressed : MonoBehaviour
{
    [SerializeField] private UnityEvent onToggle;
    [SerializeField] private UnityEvent offToggle;

    public void InvokeOnClick(bool clicked)
    {
        if (clicked)
        {
            Debug.Log(true);
            onToggle.Invoke();
        }
        else
        {
            Debug.Log(false);
            offToggle.Invoke();
        }
    }
}
