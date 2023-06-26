using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PortraitButtonData
{
    public GameObject button;
    [HideInInspector]public GameObject CharacterPrefab = null;
    public int buttonIndex;
    public int characterIndex;
}

