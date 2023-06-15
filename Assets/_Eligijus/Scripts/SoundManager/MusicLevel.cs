using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "ScriptableObjects/MusicLevel", order = 1)]
public class MusicLevel : ScriptableObject
{
    public List<AudioClip> audioClips;
}
