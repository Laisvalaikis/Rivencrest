using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitButton : MonoBehaviour
{
    [HideInInspector] public bool available = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public virtual void OnPortraitClick()
    {
        
    }

    public void OnHover()
    {
        if(available)
        {
            transform.Find("Hover").GetComponent<Animator>().SetBool("hover", true);
        }
    }
    public void OffHover()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("hover", false);
    }

    public void Select()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("select", true);
    }

    public void Deselect()
    {
        transform.Find("Hover").GetComponent<Animator>().SetBool("select", false);
    }
}
