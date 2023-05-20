using System;
using UnityEngine;


[Serializable]
public class Level
{
    public string name;

    public int levelMoves;
    public int carAmount;
    public Color[] carColor;
    public Transform[] carPosition;
    public GameObject levelEnvironment;
}
