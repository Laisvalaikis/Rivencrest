using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
//Used to manage gameplay, like EndTurn, etc.
public class GameplayManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private CinemachineFramingTransposer transposer;
    private void Start()
    { 
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void FocusSelectedCharacter(GameObject character)
    {
        transposer.m_XDamping = 1;
        transposer.m_YDamping = 1;
        transposer.m_TrackedObjectOffset = new Vector3(0, 0.5f, 0);
        virtualCamera.Follow = character.transform;
    }
}
