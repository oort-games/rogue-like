using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
[CanEditMultipleObjects]
public class SoundManagerEditor : Editor
{
    SoundManager _soundManager;

    float _volumeBGM;
    float _volumeSFX;

    bool _muteBGM;
    bool _muteSFX;

    public override void OnInspectorGUI()
    {
        _soundManager = (SoundManager)target;
        base.OnInspectorGUI();

        if (Application.isPlaying == false) return;

        serializedObject.Update();
        DrawBGM();
        DrawSFX();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawBGM()
    {
        GUILayout.Space(5);
        GUILayout.BeginVertical();
       
        EditorGUILayout.LabelField("BGM");

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mute");
        EditorGUI.BeginChangeCheck();
        _muteBGM = EditorGUILayout.Toggle(_soundManager.GetMute(SoundType.BGM));
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.SetMute(SoundType.BGM, _muteBGM);
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        _volumeBGM = EditorGUILayout.Slider(_soundManager.GetVolume(SoundType.BGM), 0f, SoundManager.MAX_VALUE);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.SetVolume(SoundType.BGM, _volumeBGM);
            serializedObject.ApplyModifiedProperties();
        }

        GUILayout.EndVertical();
    }

    void DrawSFX()
    {
        GUILayout.Space(5);
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("SFX");

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mute");
        EditorGUI.BeginChangeCheck();
        _muteSFX = EditorGUILayout.Toggle(_soundManager.GetMute(SoundType.SFX));
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.SetMute(SoundType.SFX, _muteSFX);
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        _volumeSFX = EditorGUILayout.Slider(_soundManager.GetVolume(SoundType.SFX), 0f, SoundManager.MAX_VALUE);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.SetVolume(SoundType.SFX, _volumeSFX);
            serializedObject.ApplyModifiedProperties();
        }

        GUILayout.EndVertical();
    }
}