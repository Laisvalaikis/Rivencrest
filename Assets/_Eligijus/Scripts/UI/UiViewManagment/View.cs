using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class View : MonoBehaviour
{
    [SerializeField] private bool viewCanBeDisabled = true;
    [SerializeField] private bool addViewToStack = true;
    public UnityEvent openView;
    public UnityEvent closeView;
    private int viewIndex = -1;
    private bool disabled = true;

    private void Start()
    {
        OpenView();
    }

    public void OpenView()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        if (viewIndex == -1 && addViewToStack)
        {
            viewIndex = UIStack.Instance.AddView(this);
        }
        disabled = false;
        openView.Invoke();
    }

    public void OpenCloseView()
    {
        if (disabled)
        {
            OpenView();
        }
        else
        {
            ExitView();
        }
    }

    public void ExitViewWithoutRemoveFromStack()
    {
        if (viewCanBeDisabled)
        {
            gameObject.SetActive(false);
        }

        disabled = true;
    }

    public void ExitView()
    {
        if (viewCanBeDisabled)
        {
            gameObject.SetActive(false);
        }

        if (addViewToStack)
        {
            UIStack.Quit(viewIndex);
        }

        disabled = true;
        closeView.Invoke();
        viewIndex = -1;
    }

    public void UpdateIndex(int index)
    {
        viewIndex = index;
    }


}
