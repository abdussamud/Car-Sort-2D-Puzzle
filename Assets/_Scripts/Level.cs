using System;
using UnityEngine;

[Serializable]
public class Level
{
    public string levelName;
    public GameObject levelEnvironment;
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
