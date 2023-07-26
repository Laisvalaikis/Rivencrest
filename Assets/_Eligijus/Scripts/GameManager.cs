using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool isDragAvailable = true;
    public bool isBoardDisabled;
    public bool canButtonsBeClicked = true;
    
    private Data _data;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _data = Data.Instance;
    }

    public void SpendGold(int cost)
    {
        _data.townData.townGold -= cost;
    }
    
    private void OnDestroy()
    {
        if (this == Instance)
        {
            Instance = null;
        }
    }
}
