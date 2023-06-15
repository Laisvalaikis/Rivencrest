using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestroySound : MonoBehaviour
{
	public int effectIndex;
	public int songIndex;
	public bool resetBackgroundSound = false;
	private AudioSource _source;
	
    // Start is called before the first frame update
    void Start()
    {
	    _source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_source.isPlaying)
		{
			SoundManager.Instance.StopPlaying(effectIndex, songIndex);
			if (resetBackgroundSound)
            {
				MusicManager.Instance.ResetVolume();
            }
			Destroy(gameObject);
		}
    }

}
