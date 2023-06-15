using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleWasPressed : MonoBehaviour
{
    public UnityEvent onToggle;
    public UnityEvent offToggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
