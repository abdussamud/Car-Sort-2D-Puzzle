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
    public GameObject levelEnvironment;
    public GameObject[] carGO;
    public Transform[] carPosition;
    public Row[] rowsArray;
    public Cell[] cellsArray;
    public CarInfo[] carInfos;
}

[Serializable]
public class CarInfo
{
    public string carName;
    public int carCode;
    public int carAmount;
    public Color carColor;
}
