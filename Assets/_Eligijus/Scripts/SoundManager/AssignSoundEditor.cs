using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomEditor(typeof(AssignSound))]
public class AssignSoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AssignSound assignSound = (AssignSound) target;
        
        if (SoundsData.Instance == null)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(SoundsData).Name);  //FindAssets uses tags check documentation for more info
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                SoundsData.Instance = AssetDatabase.LoadAssetAtPath<SoundsData>(path);
            }
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(assignSound.gameObject.scene);
        }

        string[] typeNames = SoundsData.Instance.GetTypeNames();
        EditorGUI.BeginDisabledGroup(typeNames.Length == 0);
        assignSound.effectSelection = EditorGUILayout.Popup("Select Sound Effect Group: ", assignSound.effectSelection, typeNames);
        EditorGUI.EndDisabledGroup();
        assignSound.soundNames = new string[0];
        if (typeNames.Length > 0)
        {
            assignSound.soundNames = SoundsData.Instance.GetSoundNames(assignSound.effectSelection);
            EditorGUI.BeginDisabledGroup(assignSound.soundNames.Length == 0);
            assignSound.songSelection = EditorGUILayout.Popup("Select Sound Effect: ", assignSound.songSelection, assignSound.soundNames);
            EditorGUI.EndDisabledGroup();
        }

        assignSound.sounds = SoundsData.Instance.GetSound(assignSound.effectSelection, assignSound.songSelection);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Assign Selected Sound"))
        {
            if (typeNames.Length > 0 && assignSound.soundNames.Length > 0)
            {
                assignSound.SetSelectedSong(assignSound.effectSelection, assignSound.songSelection);
            }
            else
            {
                Debug.Log("Can't assign selected song");
            }
        }
        
        if (GUILayout.Button("Play Sound In Editor"))
        {
            PlayClip(assignSound.sounds.clip);
        }
        
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Play Sound In Play Mode"))
        {
            assignSound.PlaySound();
        }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        

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