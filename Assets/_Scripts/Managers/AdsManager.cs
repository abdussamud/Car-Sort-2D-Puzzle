using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    #region Variables
    public static AdsManager Instance { get; private set; }

    public bool singleton;
    [SerializeField] private bool _testMode;
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] private string _iOSAdUnitId = "Interstitial_iOS";
    private string _gameId;
    private string _adUnitId;
    public string RewardedPlacementId = "rewardedVideo";
    public string BannerPlacementId = "bannerPlacement";
    public enum BannerAdPos { BOTTOM, TOP };
    public BannerAdPos BannerAdPosition;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (singleton) { DontDestroyOnLoad(gameObject); }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #if UNITY_IOS
            _gameId = _iOSGameId;
        #elif UNITY_ANDROID
            _gameId = _androidGameId;
        #elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
        #endif

        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdUnitId : _androidAdUnitId;
    }

    public void Start()
    {
        if (Advertisement.isSupported)
        {
            Debug.Log(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(_gameId, _testMode, this);
    }
    #endregion

    #region Interstitial
    public void LevelCompleted()
    {
        if (UnityInterstitialLoaded()) { ShowAd(); } else { }
    }

    public bool UnityInterstitialLoaded()
    {
        Debug.Log("Loading Ad");
        Advertisement.Load(_adUnitId, this);
        return true;
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad");
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        Debug.Log("Showing Ad");
        Advertisement.Show(_adUnitId, this);
    }
    #endregion

    #region Rewarded
    public bool UnityRewardedLoaded()
    {
        Advertisement.Load(RewardedPlacementId, this);
        return true;
    }

    public void ShowRewardedVideoUnity()
    {
        Advertisement.Show(RewardedPlacementId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(RewardedPlacementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            //MonetizationManager.instance.RewardedVideoComplete();
            GameplayUI.gui.AdsWatchedCompleted();
            // Load another ad:
            Advertisement.Load(RewardedPlacementId, this);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //MonetizationManager.instance.RewardedVideoComplete();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    #endregion

    #region Banner
    public void SetBannerPosition()
    {
        if (BannerAdPosition == BannerAdPos.TOP)
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        }
        else if (BannerAdPosition == BannerAdPos.BOTTOM)
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }
    }

    public void ShowBanner()
    {
        StartCoroutine(ShowBannerWhenReady());
    }

    private IEnumerator ShowBannerWhenReady()
    {
        yield return new WaitForSeconds(0.5f);
        Advertisement.Banner.Show(BannerPlacementId);
    }

    public void DestoryBanner()
    {
        Advertisement.Banner.Hide();
    }
    #endregion

    #region Callback
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }

    public void OnUnityAdsShowClick(string adUnitId) { }
    #endregion
}
