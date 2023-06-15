using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        SetupDontDestroyManager(this);
    }

    private static void SetupDontDestroyManager(DontDestroyManager dontDestroyManager)
    {
        if (Instance == null)
        {
            Instance = dontDestroyManager;
            DontDestroyOnLoad(dontDestroyManager.gameObject);
        }
        else
        {
            Destroy(dontDestroyManager.gameObject);
        }
    }

}
