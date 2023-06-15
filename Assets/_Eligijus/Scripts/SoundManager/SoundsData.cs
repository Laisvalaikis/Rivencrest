using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SoundsData", menuName = "ScriptableObjects/SoundsData", order = 1)]
public class SoundsData : ScriptableObject
{

    public static SoundsData Instance;
    [SerializeField] private List<SoundType> typeList;
    [SerializeField] private List<Sound> soundList;

    public string[] GetTypeNames()
    {
         if(typeList == null) 
                typeList = new List<SoundType>();
         int index = 0;
         return typeList.Count > 0 ? typeList.Select(sound => sound.name + " " + index++).ToArray() : new string[0];
    }

    public string[] GetAllSoundNames()
    {
        if (soundList == null)
        {
            soundList = new List<Sound>();
            return new string[0];
        }
        else
        {
            if (soundList.Count > 0)
            {
                return soundList.Count > 0 ? soundList.Select(sound => sound.name).ToArray() : new string[0]; 
            }
            else
            {
                return new string[0];
            }
            
        }
        
        return null;
    }
    
    public int SongCount()
    {
        return soundList.Count;
    }

    public string[] GetSoundNames(int typeIndex)
    {
        if (typeList == null)
        {
            typeList = new List<SoundType>();
            return new string[0];
        }
        else
        {
            if (typeList.Count > typeIndex && typeIndex >= 0)
            {
                if (typeList[typeIndex].soundList == null)
                {
                    typeList[typeIndex].soundList = new List<Sound>();
                }

                int index = 0;
                return typeList[typeIndex].soundList.Count > 0 ? typeList[typeIndex].soundList.Select(sound => sound.name + " " + index++).ToArray() : new string[0]; 
            }
            else
            {
                return new string[0];
            }
            
        }
        
    }

    public SoundType GetSoundType(int typeIndex)
    {
        if (typeList.Count > typeIndex && typeIndex >= 0)
        {
            return typeList[typeIndex];
        }

        return null;
    }

    public int[] GetAllEffectSongIndex(int typeIndex)
    {
        
        if (typeList.Count > typeIndex && typeIndex >= 0 && typeList[typeIndex].soundList.Count > 0)
        {
            
                int[] tempArray = new int[typeList[typeIndex].soundList.Count];
                for (int i = 0; i < soundList.Count; i++)
                {
                    for (int j = 0; j < typeList[typeIndex].soundList.Count; j++)
                    {
                        if (soundList[i] == typeList[typeIndex].soundList[j])
                        {
                            tempArray[j] = i;

                        }
                    }
                    
                }
            return tempArray;
            
        }
        return null;
    }

    
    public string[] GetAllEffectNames()
    {
        string[] names = new string[typeList.Count];
        if (typeList.Count > 0)
        {
            for (int i = 0; i < typeList.Count; i++)
            {
                names[i] = typeList[i].name;
            }
            return names;
            
        }

        return null;
    }
    
    public int EffectTypeCount()
    {
        return typeList.Count;
    }
    
    public List<Sound> GetAllEffectSound(int typeIndex)
    {
        if (typeList.Count > typeIndex && typeIndex >= 0)
        {
            
            return typeList[typeIndex].soundList;
            
        }

        return null;
    }
    
    public Sound GetSound(int typeIndex, int soundIndex)
    {
        if (typeList.Count > typeIndex && typeIndex >= 0 && soundIndex >= 0 && typeList[typeIndex].soundList.Count > soundIndex)
        {
            return typeList[typeIndex].soundList[soundIndex];
        }

        return null;
    }
    
    public Sound GetSoundFromAllSounds(int soundIndex)
    {
        if (soundList.Count > soundIndex && soundIndex >= 0)
        {
            return soundList[soundIndex];
        }

        return null;
    }

    public void AddSoundEffect(SoundType type)
    {
        if (typeList == null)
        {
            typeList = new List<SoundType>();
        }
        typeList.Add(type);
    }
    
    public void AddSound(Sound sound)
    {
        if (soundList == null)
        {
            soundList = new List<Sound>();
        }
        soundList.Add(sound);
    }

    public int GetSoundIndexFromAll(Sound sound)
    {
        return soundList.IndexOf(sound);
    }

    public void RemoveSoundType(int index)
    {
        typeList.RemoveAt(index);
    }

    public void RemoveSound(int soundIndex)
    {
        Sound sound = soundList[soundIndex];
        for (int i = 0; i < typeList.Count; i++)
        {
            if (typeList[i].soundList.Contains(sound))
            {
                typeList[i].soundList.Remove(sound);
            }
        }

        soundList.Remove(sound);
    }
}
