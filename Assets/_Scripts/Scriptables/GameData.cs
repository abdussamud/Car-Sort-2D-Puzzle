using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/Game Data", order = 1)]


public class GameData : ScriptableObject
{
    public int diamonds;
    public int gems;
    public int carPrefab;
    public int unlockedLevel;
    public int gameplaySceneBG;
    public bool isSoundOn;
    public bool isMusicOn;
    public bool isFirstTime;


    #region Data from server
    //[Header("Data From Server")]
    //
    ///*-------------Data From Server-------------*/
    //
    //[Header("Priority Instertial For MainMenu To Level")]
    //public string FirstPriorityMainMenuToLevel;
    //public string SecondPriorityMainMenuToLevel;
    //public string ThirdPriorityMainMenuToLevel;
    //
    //[Header("Priority Instertial For Level To Loading")]
    //public string FirstPriorityLevelToLoading;
    //public string SecondPriorityLevelToLoading;
    //public string ThirdPriorityLevelToLoading;
    //
    //[Header("Priority Instertial For Pause")]
    //public string FirstPriorityPause;
    //public string SecondPriorityPause;
    //public string ThirdPriorityPause;
    //
    //[Header("Priority Instertial For Level Complete")]
    //public string FirstPriorityLevelComplete;
    //public string SecondPriorityLevelComplete;
    //public string ThirdPriorityLevelComplete;
    //
    //[Header("Priority Instertial For Level Fail")]
    //public string FirstPriorityLevelFail;
    //public string SecondPriorityLevelFail;
    //public string ThirdPriorityLevelFail;
    //
    //[Header("Priority Instertial For GamePlay To MainMenu")]
    //public string FirstPriorityGamePlayToMainMenu;
    //public string SecondPriorityGamePlayToMainMenu;
    //public string ThirdPriorityGamePlayToMainMenu;
    //
    //[Header("Priority Banner")]
    //public string FirstPriorityBanner;
    //public string SecondPriorityBanner;
    //
    //[Header("Priority Small Banner")]
    //public string FirstPrioritySmallBanner;
    //public string SecondPrioritySmallBanner;
    //
    //[Header("Priority Rewarded")]
    //public string FirstPriorityRewarded;
    //public string SecondPriorityRewarded;
    //public string ThirdPriorityRewarded;
    //
    //[Header("Instertial Ads")]
    //public bool PlayButtonInstertial;
    //public bool LevelButtonInstertitial;
    //public bool LevelCompleteInstertial;
    //public bool LevelFailInstertial;
    //public bool PauseButtonInstertial;
    //public bool MenuButtonInstertial;
    //
    //[Header("Banner")]
    //public bool MainBanner;
    //public bool SmallBanner;
    //
    //[Header("Rewarded Video")]
    //public bool RewardedVideoServer;
    #endregion
}
