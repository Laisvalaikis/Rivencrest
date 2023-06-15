using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound", order = 1)]
public class Sound : ScriptableObject
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixer;
    public SText[] SubText;

    public SoundParameters soundParameters;
    

    private AudioSource source;

    public bool isPlaying = false;
    public bool playedOnce = false;
    public bool silinceBackground = false;
    [Range(0f, 1f)]
    public float silinceVolume = 0f;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = soundParameters.volume;
        source.pitch = soundParameters.pitch;
        source.playOnAwake = soundParameters.playOnAwake;
        source.loop = soundParameters.looping;
        source.spatialBlend = soundParameters.spatialBlend;
        source.dopplerLevel = soundParameters.dopplerLevel;
        source.rolloffMode = soundParameters.rolloffMode;
        source.minDistance = soundParameters.minDistance;
        source.maxDistance = soundParameters.maxDistance;
    }

    public void Play()
    {
        source.volume = soundParameters.volume * (1 + Random.Range(-soundParameters.randomVolume / 2f, soundParameters.randomVolume / 2f));
        source.pitch = soundParameters.pitch * (1 + Random.Range(-soundParameters.randomPitch / 2f, soundParameters.randomPitch / 2f)); ;
        source.Play();
    }

    public void SetData(SoundCreateEditData data)
    {
        name = data.name;
        clip = data.clip;
        mixer = data.mixer;
        soundParameters.volume = data.soundParameters.volume;
        soundParameters.pitch = data.soundParameters.pitch;
        soundParameters.playOnAwake = data.soundParameters.playOnAwake;
        soundParameters.looping = data.soundParameters.looping;
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
