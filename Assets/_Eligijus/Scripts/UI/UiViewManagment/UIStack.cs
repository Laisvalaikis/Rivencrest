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
        Instance = this;
        _views = new List<View>();
    }

    public int AddView(View view)
    {
        _views.Add(view);
        return _views.Count - 1;
    }

    public void QuitLastView()
    {
        _views.RemoveAt(_views.Count-1);
    }
    
    public void QuitView(int index)
    {
        _views.RemoveAt(index);
        for (int i = index; i < _views.Count; i++)
        {
            _views[i].UpdateIndex(i);
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
        Instance._views.RemoveAt(Instance._views.Count-1);
    }

}
