using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundParameters
{
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;


    public bool looping = false;
    public bool playOnAwake = false;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;
    [Range(0f, 1f)]
    public float spatialBlend = 0f;
    [Range(0f, 5f)]
    public float dopplerLevel = 1f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
    public float minDistance = 1;
    public float maxDistance = 500f;
}
