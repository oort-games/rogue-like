#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    SoundManager _manager;

    private void OnEnable()
    {
        _manager = (SoundManager)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();
        if (Application.isPlaying == false) return;

        EditorGUILayout.Space(4);

        DrawChannel(SoundType.Master, "Master");
        EditorGUILayout.Space(2);
        DrawChannel(SoundType.BGM, "BGM");
        EditorGUILayout.Space(2);
        DrawChannel(SoundType.SFX, "SFX");

        serializedObject.ApplyModifiedProperties();
    }

    void DrawChannel(SoundType type, string label)
    {
        GUIStyle boxStyle = new(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 6, 6),
            margin = new RectOffset(0, 0, 4, 4)
        };

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        
        bool prevMute = _manager.GetMute(type);
        float prevVolume = _manager.GetVolume(type);

        EditorGUILayout.BeginHorizontal();

        Texture icon = EditorGUIUtility.IconContent("AudioSource Icon").image;
        GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));

        bool newMute = EditorGUILayout.ToggleLeft("Mute", prevMute, GUILayout.Width(60));
        float newVolume = EditorGUILayout.Slider(prevVolume, 0f, SoundManager.MAX_VALUE);
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        if (newMute != prevMute)
            _manager.SetMute(type, newMute);

        if (!Mathf.Approximately(newVolume, prevVolume))
        {
            _manager.SetVolume(type, newVolume);

            if (prevMute)
            {
                _manager.SetMute(type, false);
            }
        }
    }

    public override bool RequiresConstantRepaint() => true;
}
#endif