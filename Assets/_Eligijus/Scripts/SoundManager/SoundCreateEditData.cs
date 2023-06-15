using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundCreateEditData
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixer;

    public SoundParameters soundParameters;

    public bool isPlaying = false;
    public bool playedOnce = false;
    public bool silinceBackground = false;
    [Range(0f, 1f)]
    public float silinceVolume = 0f;

    public void SetData(Sound data)
    {
        name = data.name;
        clip = data.clip;
        mixer = data.mixer;
        soundParameters.volume = data.soundParameters.volume;
        soundParameters.pitch = data.soundParameters.pitch;
        soundParameters.randomVolume = data.soundParameters.randomVolume;
        soundParameters.randomPitch = data.soundParameters.randomPitch;
        soundParameters.spatialBlend = data.soundParameters.spatialBlend;
        soundParameters.dopplerLevel = data.soundParameters.dopplerLevel;
        soundParameters.rolloffMode = data.soundParameters.rolloffMode;
        soundParameters.minDistance = data.soundParameters.minDistance;
        soundParameters.maxDistance = data.soundParameters.maxDistance;

        isPlaying = data.isPlaying;
        playedOnce = data.playedOnce;
        silinceBackground = data.silinceBackground;
        silinceVolume = data.silinceVolume;
    }
}
