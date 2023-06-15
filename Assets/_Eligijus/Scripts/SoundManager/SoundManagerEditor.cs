using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        
        
        
        SoundManager soundManager = (SoundManager) target;
        if (!soundManager.createSound && !soundManager.editSound)
        {
            DrawPropertiesExcluding(serializedObject, "soundData");
        }
        else
        {
            base.OnInspectorGUI();
        }
        
        

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene);
        }
        
        if (SoundsData.Instance == null && soundManager.SoundsData != null)
        {
            SoundsData.Instance = soundManager.SoundsData;
        }

        if (soundManager.SoundsData != null)
        {

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(soundManager);
                EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene);
            }

            if (!soundManager.createSoundType && !soundManager.createSound && !soundManager.editSound &&
                !soundManager.editSoundType && !soundManager.removeSoundType && !soundManager.removeSound)
            {



                string[] typeNames = soundManager.SoundsData.GetTypeNames();
                EditorGUI.BeginDisabledGroup(typeNames.Length == 0);
                soundManager.effectSelection =
                    EditorGUILayout.Popup("Select Sound effect Edit: ", soundManager.effectSelection, typeNames);
                EditorGUI.EndDisabledGroup();

                if (typeNames.Length > 0)
                {
                    string[] songNames = soundManager.SoundsData.GetSoundNames(soundManager.effectSelection);
                    EditorGUI.BeginDisabledGroup(songNames.Length == 0);
                    soundManager.songSelection =
                        EditorGUILayout.Popup("Select Sound Edit: ", soundManager.songSelection, songNames);
                    EditorGUI.EndDisabledGroup();
                }



                soundManager.sounds =
                    soundManager.SoundsData.GetSound(soundManager.songSelection, soundManager.songSelection);

                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(Application.isPlaying);


                if (GUILayout.Button("Create Sound Type"))
                {
                    soundManager.createSoundType = true;
                }

                if (GUILayout.Button("Create Sound"))
                {
                    soundManager.createSound = true;
                }

                EditorGUI.BeginDisabledGroup(soundManager.SoundsData.SongCount() == 0);
                if (GUILayout.Button("Edit Sound"))
                {
                    soundManager.editSound = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(soundManager.SoundsData.EffectTypeCount() == 0);
                if (GUILayout.Button("Edit Sound Type"))
                {
                    soundManager.editSoundType = true;
                }

                EditorGUI.EndDisabledGroup();

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(soundManager.SoundsData.EffectTypeCount() == 0);
                if (GUILayout.Button("Remove Sound Type"))
                {
                    soundManager.removeSoundType = true;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(soundManager.SoundsData.SongCount() == 0);
                if (GUILayout.Button("Remove Sound"))
                {
                    soundManager.removeSound = true;
                }

                EditorGUI.EndDisabledGroup();

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(soundManager.SoundsData.SongCount() == 0);
                if (GUILayout.Button("Play Sound In Editor"))
                {
                    PlayClip(soundManager.sounds.clip);
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(!Application.isPlaying);
                if (GUILayout.Button("Play Sound In Play Mode"))
                {
                    PlaySound(soundManager);
                }

                EditorGUI.EndDisabledGroup();

                GUILayout.EndHorizontal();
            }

            if (soundManager.createSoundType)
            {
                string[] allSongNames = soundManager.SoundsData.GetAllSoundNames();
                soundManager.soundTypeName =
                    EditorGUILayout.TextArea(soundManager.soundTypeName, GUILayout.MaxHeight(17));
                soundManager.selectedSoundIndex = EditorGUILayout.Popup("Select sound to add: ",
                    soundManager.selectedSoundIndex, allSongNames);

                if (soundManager.selectedSoundIndexArray == null)
                {
                    soundManager.selectedSoundIndexArray = new List<int>();
                }

                if (soundManager.selectedSoundIndexArray != null)
                {
                    for (int i = 0; i < soundManager.selectedSoundIndexArray.Count; i++)
                    {
                        soundManager.selectedSoundIndexArray[i] = EditorGUILayout.Popup("Selected Sound: ",
                            soundManager.selectedSoundIndex, allSongNames);
                    }
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Sound To Effects"))
                {
                    if (soundManager.soundsToAdd == null)
                    {
                        soundManager.soundsToAdd = new List<Sound>();
                    }

                    Sound tempSoundToAdd =
                        soundManager.SoundsData.GetSoundFromAllSounds(soundManager.selectedSoundIndex);
                    if (tempSoundToAdd != null && soundManager.soundsToAdd != null)
                    {
                        soundManager.soundsToAdd.Add(tempSoundToAdd);
                        soundManager.selectedSoundIndexArray.Add(
                            soundManager.SoundsData.GetSoundIndexFromAll(tempSoundToAdd));
                    }
                }

                EditorGUI.BeginDisabledGroup(soundManager.soundsToAdd.Count == 0);
                if (GUILayout.Button("Add Effect"))
                {
                    soundManager.soundType = new SoundType();
                    soundManager.soundType.name = soundManager.soundTypeName;
                    if (soundManager.soundType.soundList == null)
                    {
                        soundManager.soundType.soundList = new List<Sound>();
                    }

                    for (int i = 0; i < soundManager.soundsToAdd.Count; i++)
                    {
                        if (!soundManager.soundType.soundList.Contains(soundManager.soundsToAdd[i]))
                        {
                            soundManager.soundType.soundList.Add(soundManager.soundsToAdd[i]);
                        }
                    }

                    serializedObject.ApplyModifiedProperties();
                    soundManager.SoundsData.AddSoundEffect(soundManager.soundType);
                    soundManager.soundType = new SoundType();
                    soundManager.createSoundType = false;
                    soundManager.selectedSoundIndexArray = null;
                    soundManager.soundTypeName = "";
                    EditorUtility.SetDirty(soundManager.SoundsData);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Cancel"))
                {
                    soundManager.soundType = new SoundType();
                    soundManager.createSoundType = false;
                    soundManager.selectedSoundIndexArray = null;
                    soundManager.soundTypeName = "";
                }

                GUILayout.EndHorizontal();

            }

            if (soundManager.createSound)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Song"))
                {
                    string fullPath =
                        EditorUtility.SaveFilePanelInProject("Create New Sound", "NewSound", "asset", "Save file");

                    if (string.IsNullOrEmpty(fullPath) == false)
                    {
                        Sound newSound = ScriptableObject.CreateInstance<Sound>();
                        AssetDatabase.CreateAsset(newSound, fullPath);
                        AssetDatabase.SaveAssets();
                        newSound.SetData(soundManager.soundData);
                        soundManager.SoundsData.AddSound(newSound);
                        EditorUtility.SetDirty(soundManager.SoundsData);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        serializedObject.ApplyModifiedProperties();
                        soundManager.soundData = new SoundCreateEditData();
                        soundManager.createSound = false;
                    }
                }

                if (GUILayout.Button("Cancel"))
                {
                    soundManager.soundData = new SoundCreateEditData();
                    soundManager.createSound = false;

                }

                GUILayout.EndHorizontal();
            }

            if (soundManager.editSound)
            {
                string[] allSongNames = soundManager.SoundsData.GetAllSoundNames();
                soundManager.selectedSoundIndex = EditorGUILayout.Popup("Select Pose Data Edit: ",
                    soundManager.selectedSoundIndex, allSongNames);

                if (soundManager.soundToEdit !=
                    soundManager.SoundsData.GetSoundFromAllSounds(soundManager.selectedSoundIndex))
                {
                    soundManager.soundToEdit =
                        soundManager.SoundsData.GetSoundFromAllSounds(soundManager.selectedSoundIndex);
                    soundManager.soundData.SetData(soundManager.soundToEdit);
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save"))
                {
                    soundManager.soundToEdit.SetData(soundManager.soundData);

                    EditorUtility.SetDirty(soundManager.SoundsData);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    serializedObject.ApplyModifiedProperties();
                    soundManager.soundData = new SoundCreateEditData();
                    soundManager.soundToEdit = null;
                    soundManager.editSound = false;
                }

                if (GUILayout.Button("Cancel"))
                {
                    soundManager.soundData = new SoundCreateEditData();
                    soundManager.soundToEdit = null;
                    soundManager.editSound = false;
                }

                GUILayout.EndHorizontal();
            }

            if (soundManager.editSoundType && soundManager.SoundsData.EffectTypeCount() > 0)
            {

                string[] allEffectNames = soundManager.SoundsData.GetAllEffectNames();
                string[] allSongNames = soundManager.SoundsData.GetAllSoundNames();
                soundManager.soundTypeName =
                    EditorGUILayout.TextArea(soundManager.soundTypeName, GUILayout.MaxHeight(17));
                soundManager.effectSelection = EditorGUILayout.Popup("Select Pose Data Edit: ",
                    soundManager.effectSelection, allEffectNames);
                //soundManager.soundType = soundManager.SoundsData.GetSoundType(soundManager.effectSelection);
                soundManager.soundType = soundManager.SoundsData.GetSoundType(soundManager.effectSelection);

                if (soundManager.tempEffectSelection != soundManager.effectSelection)
                {
                    soundManager.soundTypeName = soundManager.soundType.name;
                    soundManager.selectedSoundIndexArray = null;
                }

                if (soundManager.soundType.soundList == null && soundManager.effectSelection >= 0)
                {
                    soundManager.soundType.soundList =
                        new List<Sound>(soundManager.SoundsData.GetAllEffectSound(soundManager.effectSelection));
                }

                if (soundManager.selectedSoundIndexArray == null)
                {
                    soundManager.selectedSoundIndexArray = new List<int>(
                        soundManager.SoundsData.GetAllEffectSongIndex(soundManager.effectSelection));
                    soundManager.soundTypeName = soundManager.soundType.name;
                }

                if (soundManager.selectedSoundIndexArray != null)
                {
                    for (int i = 0; i < soundManager.selectedSoundIndexArray.Count; i++)
                    {
                        soundManager.selectedSoundIndexArray[i] = EditorGUILayout.Popup("Select Pose Data Edit: ",
                            soundManager.selectedSoundIndexArray[i], allSongNames);
                    }

                }

                if (soundManager.tempEffectSelection != soundManager.effectSelection)
                {
                    soundManager.tempEffectSelection = soundManager.effectSelection;
                    for (int i = 0; i < soundManager.selectedSoundIndexArray.Count; i++)
                    {

                        soundManager.selectedSoundIndexArray[i] = 0;

                    }

                    soundManager.soundTypeName = soundManager.soundType.name;
                    soundManager.soundsToAdd = new List<Sound>();
                }

                //soundManager.soundType.soundList
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Sound To Effects"))
                {
                    if (soundManager.selectedSoundIndexArray == null)
                    {
                        soundManager.selectedSoundIndexArray = new List<int>();
                    }
                    else
                    {
                        soundManager.selectedSoundIndexArray.Add(0);
                    }
                }

                if (GUILayout.Button("Remove Last Sound"))
                {
                    if (soundManager.selectedSoundIndexArray == null)
                    {
                        soundManager.selectedSoundIndexArray = new List<int>();
                    }
                    else if (soundManager.selectedSoundIndexArray.Count > 0)
                    {
                        soundManager.selectedSoundIndexArray.Remove(0);
                    }

                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(soundManager.selectedSoundIndexArray.Count == 0 ||
                                             soundManager.soundType.name == "");
                if (GUILayout.Button("Save"))
                {
                    soundManager.soundType.name = soundManager.soundTypeName;
                    soundManager.soundType.soundList = new List<Sound>();

                    for (int i = 0; i < soundManager.selectedSoundIndexArray.Count; i++)
                    {
                        if (!soundManager.soundType.soundList.Contains(
                                soundManager.SoundsData.GetSoundFromAllSounds(soundManager.selectedSoundIndexArray[i])))
                        {

                            soundManager.soundType.soundList.Add(
                                soundManager.SoundsData.GetSoundFromAllSounds(soundManager.selectedSoundIndexArray[i]));

                        }
                    }

                    EditorUtility.SetDirty(soundManager.SoundsData);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    serializedObject.ApplyModifiedProperties();
                    soundManager.soundType = new SoundType();
                    soundManager.selectedSoundIndexArray = null;
                    soundManager.soundType.soundList = null;
                    soundManager.editSoundType = false;
                    soundManager.tempEffectSelection = -1;
                    soundManager.soundTypeName = "";
                }

                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Cancel"))
                {
                    soundManager.soundType = new SoundType();
                    soundManager.selectedSoundIndexArray = null;
                    soundManager.soundType.soundList = null;
                    soundManager.editSoundType = false;
                    soundManager.tempEffectSelection = -1;
                    soundManager.soundTypeName = "";
                }

                GUILayout.EndHorizontal();
            }
            else if (soundManager.editSoundType && soundManager.SoundsData.EffectTypeCount() == 0)
            {
                soundManager.editSoundType = false;
            }

            if (soundManager.removeSoundType)
            {
                string[] allEffectNames = soundManager.SoundsData.GetAllEffectNames();
                soundManager.effectSelection = EditorGUILayout.Popup("Select Pose Data Edit: ",
                    soundManager.effectSelection, allEffectNames);
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(allEffectNames.Length == 0);

                if (GUILayout.Button("Remove Sound Type"))
                {
                    soundManager.SoundsData.RemoveSoundType(soundManager.effectSelection);
                    EditorUtility.SetDirty(soundManager.SoundsData);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Cancel"))
                {
                    soundManager.removeSoundType = false;
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                soundManager.removeSoundType = false;
            }

            if (soundManager.removeSound)
            {


                string[] allSongNames = soundManager.SoundsData.GetAllSoundNames();
                soundManager.songSelection = EditorGUILayout.Popup("Select Pose Data Edit: ",
                    soundManager.effectSelection, allSongNames);
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(allSongNames.Length == 0);

                if (GUILayout.Button("Remove Sound"))
                {
                    soundManager.SoundsData.RemoveSound(soundManager.songSelection);
                    EditorUtility.SetDirty(soundManager.SoundsData);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Cancel"))
                {
                    soundManager.removeSound = false;
                }

                GUILayout.EndHorizontal();
            }
        }

    }
    
    public void PlaySound(SoundManager soundManager)
    {
        soundManager.CreateSound(soundManager.effectSelection, soundManager.songSelection, soundManager.transform);
    }
    
    public void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
 
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );
        
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }
    
}
#endif