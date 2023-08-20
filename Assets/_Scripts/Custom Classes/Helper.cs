using System;
using UnityEngine;
using UnityEngine.Audio;

public class Helper { }

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
    public bool playOnAwake;
    public bool loop;
}

[Serializable]
public class Level
{
    public string levelName;
    public int theme;
    public int moveCount;
    public float cameraSize;
    public GameObject levelEnvironment;
    public int[] carCode;
    public Row[] rowsArray;
    public Cell[] cellsArray;
    public Transform[] carPosition;
}
