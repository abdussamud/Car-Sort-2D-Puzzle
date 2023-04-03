using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;
    public int unlockedLevel;
    public List<GameObject> levelGameObjectList;


    private void Awake()
    {
        Instance = this;
        unlockedLevel = PlayerPrefs.GetInt("UnlockLevel");
    }
}
