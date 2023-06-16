using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;




public class SoundManager : MonoBehaviour
{
	#region Inspector

	[HideInInspector] public int effectSelection;
	[HideInInspector] public int tempEffectSelection;
	[HideInInspector] public int songSelection;
	[HideInInspector] public Sound sounds;
	[HideInInspector] public int selectedSoundIndex;
	[HideInInspector] public List<int> selectedSoundIndexArray;
	[HideInInspector] public bool createSound;
	[HideInInspector] public bool editSound;
	[HideInInspector] public bool createSoundType;
	[HideInInspector] public bool editSoundType;
	[HideInInspector] public bool removeSoundType;
	[HideInInspector] public bool removeSound;
	[HideInInspector] public string soundTypeName;
	[HideInInspector] public List<Sound> soundsToAdd;
	[HideInInspector] public SoundType soundType;
	[HideInInspector] public Sound soundToEdit;
	public SoundsData SoundsData;
	#endregion

	public static SoundManager Instance { get; private set; }

	[SerializeField]
	public SoundCreateEditData soundData;
	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("More than one AudioManager in the scene.");
		}
		else
		{
			Instance = this;
		}
	}

	public void CreateSound(int effectsIndex, int songIndex, Transform location)
	{
		Sound sound = SoundsData.GetSound(effectsIndex, songIndex);
		SoundType localSoundType = SoundsData.GetSoundType(effectsIndex);
		if (sound != null)
		{
			GameObject _go = new GameObject("Sound_" + "_" + localSoundType.name + "_" + sound.name);
			sound.SetSource(_go.AddComponent<AudioSource>());
			_go.transform.position = location.transform.position;
			AudioSource source = _go.GetComponent<AudioSource>();
			source.outputAudioMixerGroup = sound.mixer;
			
			source.Play();
			sound.isPlaying = true;
			sound.playedOnce = true;
			DestroySound destroySound = _go.AddComponent<DestroySound>();
			if (sound.silinceBackground)
			{
				MusicManager.Instance.ChangeSoundVolume(sound.silinceVolume);
				destroySound.resetBackgroundSound = true;
			}
			
			destroySound.effectIndex = effectsIndex;
			destroySound.songIndex = songIndex;
			return;
		}

		Debug.LogWarning("AudioManager: Sound not found in sounds");
	}
 
	public void SetSoundToSource(int effectsIndex, int songIndex, AudioSource source)
	{
		Sound sound = SoundsData.GetSound(effectsIndex, songIndex);
		if(sound != null)
			{
				sound.SetSource(source);
				source.outputAudioMixerGroup = sound.mixer;
				source.Play();
				return;
			}
		Debug.LogWarning("AudioManager: Sound not found in sounds");
	}
 
	public void StopPlaying(int effectsIndex, int songIndex)
	{
		
		Sound sound = SoundsData.GetSound(effectsIndex, songIndex);
		if (sound != null)
		{
			sound.isPlaying = false;
		}

	}
 
	public bool IsPlaying(int effectsIndex, int songIndex)
    {
	    Sound sound = SoundsData.GetSound(effectsIndex, songIndex);
		return sound.isPlaying;
    }
 
	public bool PlayedOnce(int effectsIndex, int songIndex)
    {
	    Sound sound = SoundsData.GetSound(effectsIndex, songIndex);
		return sound.playedOnce;
    }
}
