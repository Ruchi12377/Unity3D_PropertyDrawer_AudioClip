#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(AudioClip))]
    public class AudioClipPropertyDrawer : PropertyDrawer
    {
        private const string ScratchAudioLabel = "ScratchAudio";
        private static readonly Color ScratchAudioColor = new Color32(255, 182, 193, 255);

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(prop);
        }

        private readonly Dictionary<ButtonState, Action<SerializedProperty, AudioClip>> _audioButtonStates = new()
        {
            { ButtonState.Play, Play },
            { ButtonState.Stop, Pause },
        };

        private enum ButtonState
        {
            Play,
            Stop
        }

        private static string _currentClip;
        private static readonly GUIStyle TempStyle = new();


        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);

            if (prop.objectReferenceValue != null)
            {
                var totalWidth = position.width;
                position.width = totalWidth - (totalWidth / 4);
                EditorGUI.PropertyField(position, prop, label, true);

                position.x += position.width;
                position.width = totalWidth / 4;
                DrawButton(position, prop,
                    Array.IndexOf(AssetDatabase.GetLabels(prop.objectReferenceValue), ScratchAudioLabel) > -1);
            }
            else
            {
                EditorGUI.PropertyField(position, prop, label, true);
            }

            EditorGUI.EndProperty();
        }

        private void DrawButton(Rect position, SerializedProperty prop, bool isScratchAudio)
        {
            if (prop.objectReferenceValue != null)
            {
                position.x += 4;
                position.width -= 5;

                var clip = prop.objectReferenceValue as AudioClip;

                var buttonRect = new Rect(position)
                {
                    width = 20
                };

                var guiEnabledCache = GUI.enabled;
                GUI.enabled = true;

                var waveformRect = new Rect(position);
                waveformRect.x += 22;
                waveformRect.width -= 22;
                if (Event.current.type == EventType.Repaint)
                {
                    var waveformTexture = AssetPreview.GetAssetPreview(prop.objectReferenceValue);
                    if (waveformTexture != null)
                    {
                        GUI.color = isScratchAudio ? ScratchAudioColor : Color.white;
                        TempStyle.normal.background = waveformTexture;
                        TempStyle.Draw(waveformRect, GUIContent.none, false, false, false, false);
                        GUI.color = Color.white;
                    }
                }

                if (isScratchAudio)
                {
                    var style = new GUIStyle();
                    var content = new GUIContent("⁻ˢᶜʳᵃᵗᶜʰ⁻", "This AudioClip Asset is labeled as 'ScratchAudio'");
                    style.normal.textColor = ScratchAudioColor;
                    style.alignment = TextAnchor.MiddleCenter;
                    GUI.Box(waveformRect, content, style);
                }

                var isPlaying = AudioUtility.IsClipPlaying(clip) && (_currentClip == prop.propertyPath);
                string buttonText;
                Action<SerializedProperty, AudioClip> buttonAction;
                if (isPlaying)
                {
                    EditorUtility.SetDirty(prop.serializedObject.targetObject);
                    buttonAction = GetStateInfo(ButtonState.Stop, out buttonText);

                    var progressRect = new Rect(waveformRect);
                    var percentage = (float)AudioUtility.GetClipSamplePosition(clip) / AudioUtility.GetSampleCount(clip);
                    var width = progressRect.width * percentage;
                    progressRect.width = Mathf.Clamp(width, 6, width);
                    GUI.Box(progressRect, "", "SelectionRect");
                }
                else
                {
                    buttonAction = GetStateInfo(ButtonState.Play, out buttonText);
                }

                if (GUI.Button(buttonRect, buttonText))
                {
                    AudioUtility.StopAllClips();
                    buttonAction(prop, clip);
                }

                GUI.enabled = guiEnabledCache;
            }
        }

        private static void Play(SerializedProperty prop, AudioClip clip)
        {
            _currentClip = prop.propertyPath;
            AudioUtility.PlayClip(clip);
        }

        private static void Pause(SerializedProperty prop, AudioClip clip)
        {
            _currentClip = "";
            AudioUtility.PauseClip(clip);
        }

        private Action<SerializedProperty, AudioClip> GetStateInfo(ButtonState state, out string buttonText)
        {
            switch (state)
            {
                case ButtonState.Play:
                    buttonText = "►";
                    break;
                case ButtonState.Stop:
                    buttonText = "■";
                    break;
                default:
                    buttonText = "?";
                    break;
            }

            return _audioButtonStates[state];
        }
    }
}
#endif