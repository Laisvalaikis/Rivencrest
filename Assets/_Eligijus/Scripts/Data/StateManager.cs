using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Data _gameData;
    // Start is called before the first frame update
    void Start()
    {
        _gameData = Data.Instance;
        
    }

    public void ChangeSinglePlayerState(bool singlePlayer)
    {
        _gameData.townData.singlePlayer = singlePlayer;
    }

}
