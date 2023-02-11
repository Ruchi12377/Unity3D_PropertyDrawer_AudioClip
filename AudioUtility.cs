#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
    public static class AudioUtility
    {
        public static void PlayClip(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
#if UNITY_2020_2_OR_NEWER
                "PlayPreviewClip",
#else
                "PlayPreviewClip",
#endif
                BindingFlags.Static | BindingFlags.Public,
                null,
#if UNITY_2020_2_OR_NEWER
                new[] { typeof(AudioClip), typeof(int), typeof(bool) },
#else
                new[] { typeof(AudioClip) },
#endif
                null
            );
            method.Invoke(
                null,
                new object[]
                {
                    clip, 0, false
                }
            );
        }

        public static void PauseClip(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
#if UNITY_2020_2_OR_NEWER
                "PausePreviewClip",
#else
                "PauseClip",
#endif
                BindingFlags.Static | BindingFlags.Public,
                null,
#if UNITY_2020_2_OR_NEWER
                new Type[] { },
#else
                                new[]
                {
                    typeof(AudioClip)
                },
#endif
                null
            );
            method.Invoke(
                null,
#if UNITY_2020_2_OR_NEWER
                null
#else
                new object[]
                {
                    clip
                }
#endif
            );
        }

        public static bool IsClipPlaying(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
#if UNITY_2020_2_OR_NEWER
                "IsPreviewClipPlaying",
#else
                "IsClipPlaying",
#endif
                BindingFlags.Static | BindingFlags.Public
            );

#if UNITY_2020_2_OR_NEWER
            var playing = (bool)method.Invoke(
                null,
                null
            );
#else
            var playing = (bool)method.Invoke(
				null,
				new object[] { clip }
			);
#endif

            return playing;
        }

        public static void StopAllClips()
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
#if UNITY_2020_2_OR_NEWER
                "StopAllPreviewClips",
#else
                "StopAllClips",
#endif
                BindingFlags.Static | BindingFlags.Public
            );

            method.Invoke(
                null,
                null
            );
        }

        public static int GetClipSamplePosition(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
#if UNITY_2020_2_OR_NEWER
                "GetPreviewClipSamplePosition",
#else
                "GetClipSamplePosition",
#endif
                BindingFlags.Static | BindingFlags.Public
            );

#if UNITY_2020_2_OR_NEWER
            var position = (int)method.Invoke(
                null,
                null
            );
#else
            var position = (int)method.Invoke(
                null,
                new object[] { clip }
            );
#endif


            return position;
        }

        public static int GetSampleCount(AudioClip clip)
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            var method = audioUtilClass.GetMethod(
                "GetSampleCount",
                BindingFlags.Static | BindingFlags.Public
            );

            var samples = (int)method.Invoke(
                null,
                new object[]
                {
                    clip
                }
            );

            return samples;
        }
    }
}
#endif