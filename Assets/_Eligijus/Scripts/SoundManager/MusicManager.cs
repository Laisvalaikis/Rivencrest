using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField]
    private List<MusicLevel> musicLevels;
    [SerializeField]
    private SoundParameters soundData;

    private int _level = 0;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent<AudioSource>(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _audioSource.clip = musicLevels[_level].audioClips[0];
        UpdateSettings();
        _audioSource.Play();
    }

    public void UpdateSettings()
    {
        _audioSource.volume = soundData.volume;
        _audioSource.pitch = soundData.pitch;
        _audioSource.spatialBlend = soundData.spatialBlend;
        _audioSource.dopplerLevel = soundData.dopplerLevel;
        _audioSource.rolloffMode = soundData.rolloffMode;
        _audioSource.minDistance = soundData.minDistance;
        _audioSource.maxDistance = soundData.maxDistance;
        _audioSource.playOnAwake = soundData.playOnAwake;
        _audioSource.loop = soundData.looping;
    }

    public void ChangeLevelMusic(int levelMusicIndex)
    {
        if (_level != levelMusicIndex)
        {
            if (levelMusicIndex >= 0 && levelMusicIndex < musicLevels.Count)
            {
                _level = levelMusicIndex;
                StartCoroutine(FadeIt(musicLevels[_level].audioClips[0], soundData.volume));
            }
            else
            {
                Debug.LogError(
                    "Music index is out of bounds, please set index that is less or equal to music song count");
            }
        }
    }

    public void ChangeSoundVolume(float level)
    {
        _audioSource.volume = level;
    }

    public void ResetVolume()
    {
        _audioSource.volume = soundData.volume;
    }

    public void ChangePlaybackSpeed(float playbackSpeed)
    {
        _audioSource.pitch = playbackSpeed;
        _audioSource.outputAudioMixerGroup.audioMixer.SetFloat("MusicPich", 1f/ playbackSpeed);
    }
    
    IEnumerator FadeIt(AudioClip clip, float volume)
    {

        AudioSource originalAudioSource = GetComponent<AudioSource>();
        AudioSource fadeOutSource = gameObject.AddComponent<AudioSource>();
        fadeOutSource.clip = originalAudioSource.clip;
        fadeOutSource.time = originalAudioSource.time;
        fadeOutSource.volume = originalAudioSource.volume;
        fadeOutSource.pitch = originalAudioSource.pitch;
        fadeOutSource.loop = originalAudioSource.loop;
        fadeOutSource.outputAudioMixerGroup = originalAudioSource.outputAudioMixerGroup;
        
        fadeOutSource.Play();
        
        originalAudioSource.volume = 0f;
        originalAudioSource.clip = clip;
        float t = 0;
        float v = fadeOutSource.volume;
        originalAudioSource.Play();

        while (t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * 0.8f);
            fadeOutSource.volume = Mathf.Lerp(v, 0f, t);
            originalAudioSource.volume = Mathf.Lerp(0f, volume, t);
            yield return null;
        }
        originalAudioSource.volume = volume;
        Destroy(fadeOutSource);
    }
}
