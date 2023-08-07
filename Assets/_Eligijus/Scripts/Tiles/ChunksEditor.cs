using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Chunks))]
public class ChunksEditor : Editor
{
 
    public override void OnInspectorGUI()
    {
        Chunks chunks = (Chunks) target;
        base.OnInspectorGUI();
        
        // if (GUI.changed)
        // {
        //     EditorUtility.SetDirty(target);
        //     EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene);
        // }
        //
        // if (SoundsData.Instance == null && soundManager.SoundsData != null)
        // {
        //     SoundsData.Instance = soundManager.SoundsData;
        // }

        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("GenerateChunks"))
        {
            chunks.GenerateChunks();  
        }
                
        if (GUILayout.Button("ClearChuncks"))
        {
            chunks.ResetChunks();  
        }

        GUILayout.EndHorizontal();
        
    }

}
#endif