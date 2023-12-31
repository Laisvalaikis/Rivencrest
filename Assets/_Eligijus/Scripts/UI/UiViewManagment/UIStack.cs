using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStack : MonoBehaviour
{
    public static UIStack Instance { get; private set; }

    private List<View> _views;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _views = new List<View>();
        }
    }

    public int AddView(View view)
    {
        _views.Add(view);
        return _views.Count - 1;
    }

    public void QuitLastView()
    {
        if (_views.Count > 0)
        {
            _views[Instance._views.Count - 1].ExitView();
        }
    }
    
    public void QuitView(int index)
    {
        if (index >= 0)
        {
            _views.RemoveAt(index);
            for (int i = index; i < _views.Count; i++)
            {
                _views[i].UpdateIndex(i);
            }
        }
    }
    public static void Quit(int index)
    {
        Instance.QuitView(index);
    }

    public static void ClearStack()
    {
        Instance._views.Clear();
    }
    
    public static void QuitLast()
    {
        if (Instance._views.Count > 0)
        {
            Instance._views[Instance._views.Count - 1].ExitView();
        }
    }

    public static bool HasAnyViewToQuit()
    {
        return Instance._views.Count > 0;
    }
    
    private void OnDestroy()
    {
        if (this == Instance)
        {
            Instance = null;
        }
    }

}
