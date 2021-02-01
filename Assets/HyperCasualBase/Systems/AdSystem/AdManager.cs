using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdManager : Singleton<AdManager>
{
    public string AndroidAppID;
    public string IOSAppID;

    private UnityEvent OnRewardRewardedEvent = new UnityEvent();
    private UnityEvent OnRewardFailEvent = new UnityEvent();
    private UnityEvent OnInterstitialLoaded = new UnityEvent();

    private int levelPassedCount;

    private bool rewardedPlayed;

    public bool IsActive = false;

    private void Start()
    {

        if (!IsActive)
            return;

        if (!string.IsNullOrEmpty(GetAppID()))
        {
            IronSource.Agent.init(GetAppID());
            IronSource.Agent.validateIntegration();
            Debug.Log("Ironsource Initilized,  device id for testing is " + IronSource.Agent.getAdvertiserId());
            IronSource.Agent.shouldTrackNetworkState(true);
        }
        else
        {
            AnalitycsManager.Instance.LogEvent("Ad_Event", "App ID is empty or null", "Platform: " + Application.platform);
            Debug.LogError("App ID is empty or null");
        }

        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        IronSource.Agent.loadInterstitial();

    }

    private void OnEnable()
    {

        if (!IsActive)
            return;

        //Subscribe to interstitial events
        IronSourceEvents.onInterstitialAdClickedEvent += OnInterstitialClick;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialLoadFail;
        IronSourceEvents.onInterstitialAdOpenedEvent += OnInterstitialOpen;
        IronSourceEvents.onInterstitialAdClickedEvent += OnInterstitialClose;

        //Subscribe to rewarded events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;


        SceneController.Instance.OnSceneLoaded.AddListener(ShowInterstitialVideo);
    }


    private void OnDisable()
    {
        if (!IsActive)
            return;

        //Subscribe to interstitial events
        IronSourceEvents.onInterstitialAdClickedEvent -= OnInterstitialClick;
        IronSourceEvents.onInterstitialAdLoadFailedEvent -= OnInterstitialLoadFail;
        IronSourceEvents.onInterstitialAdOpenedEvent -= OnInterstitialOpen;

        //Subscribe to rewarded events
        IronSourceEvents.onRewardedVideoAdOpenedEvent -= RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent -= RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent -= RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent -= RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent -= RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent -= RewardedVideoAdShowFailedEvent;

        

        SceneController.Instance.OnSceneLoaded.RemoveListener(ShowInterstitialVideo);

    }

    #region AdManagerCalls

    private int interstitialRequestCount = 0;
    public void ShowInterstitialVideo()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;

        if (levelPassedCount <= 1)
        {
            levelPassedCount++;
            return;
        }

        if (rewardedPlayed)
        {
            rewardedPlayed = false;
            return;
        }

        if (IronSource.Agent.isInterstitialReady())
        {
            Debug.Log("Interstital Shown");
            IronSource.Agent.showInterstitial();
            interstitialRequestCount = 0;
        }
        else
        {
            if (interstitialRequestCount != 0)
                return;
            Debug.Log("Interstital Requested");

            IronSource.Agent.loadInterstitial();
            interstitialRequestCount++;
        }
    }

    private int rewardedRequestCount = 0;
    public void ShowRewardedVideo(System.Action OnRewarded, Action OnFail = null)
    {
#if UNITY_EDITOR
        OnRewarded.Invoke();
        return;
#endif
        rewardedPlayed = true;
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return;

        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            OnRewardRewardedEvent.AddListener(OnRewarded.Invoke);
            if (OnFail != null)
                OnRewardFailEvent.AddListener(OnFail.Invoke);

            IronSource.Agent.showRewardedVideo();
        }
    }

    #endregion

    #region IronSourceCallBacks
    #region RewardedCallBacks
    private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
    {
        var rewardAmount = PlayerPrefs.GetInt(PlayerPrefKeys.RewardAmount, 0);
        rewardAmount += obj.getRewardAmount();

        AnalitycsManager.Instance.LogEvent("Ad_Event", "RewardedVideoClicked", "Reward amount_" + rewardAmount);
        PlayerPrefs.SetInt(PlayerPrefKeys.RewardAmount, rewardAmount);
    }

    //Invoked when the RewardedVideo ad view has opened.
    //Your Activity will lose focus. Please avoid performing heavy 
    //tasks till the video ad will be closed.
    void RewardedVideoAdOpenedEvent()
    {
        AudioManager.Instance.MuteMasterSound();
    }
    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    void RewardedVideoAdClosedEvent()
    {

    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available. 
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    void RewardedVideoAvailabilityChangedEvent(bool available)
    {
        //Change the in-app 'Traffic Driver' state according to availability.
        bool rewardedVideoAvailability = available;
    }

    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for 
    // the callback from the  ironSource server.
    //@param - placement - placement object which contains the reward data
    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
    {
        var rewardAmount = PlayerPrefs.GetInt(PlayerPrefKeys.RewardAmount, 0);
        rewardAmount += placement.getRewardAmount();
        AnalitycsManager.Instance.LogEvent("Ad_Event", "RewardedVideoSuccess", rewardAmount.ToString());
        OnRewardRewardedEvent.Invoke();
        OnRewardRewardedEvent.RemoveAllListeners();
        PlayerPrefs.SetInt(PlayerPrefKeys.RewardAmount, rewardAmount);
    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        AnalitycsManager.Instance.LogEvent("Ad_Event", "RewardedVideoFail", "Reason_" + error.getDescription() + " errorCode_" + error.getErrorCode());
        OnRewardFailEvent.Invoke();
        OnRewardFailEvent.RemoveAllListeners();
    }

    // ----------------------------------------------------------------------------------------
    // Note: the events below are not available for all supported rewarded video ad networks. 
    // Check which events are available per ad network you choose to include in your build. 
    // We recommend only using events which register to ALL ad networks you include in your build. 
    // ----------------------------------------------------------------------------------------

    //Invoked when the video ad starts playing. 
    void RewardedVideoAdStartedEvent()
    {
        int watchCount = PlayerPrefs.GetInt(PlayerPrefKeys.RewardedWatchCount, 0);
        watchCount++;
        AnalitycsManager.Instance.LogEvent("Ad_Event", "RewardedOpen", watchCount.ToString());
        PlayerPrefs.SetInt(PlayerPrefKeys.RewardedWatchCount, watchCount);
    }
    //Invoked when the video ad finishes playing. 
    void RewardedVideoAdEndedEvent()
    {
        AudioManager.Instance.UnmuteMasterSound();
    }
    #endregion
    #region InterstitialCallbacks
    private void OnInterstitialOpen()
    {
        levelPassedCount = 0;
        int watchCount = PlayerPrefs.GetInt(PlayerPrefKeys.InterstitialWatchCount, 0);
        watchCount++;
        AnalitycsManager.Instance.LogEvent("Ad_Event", "InterstitalOpen", watchCount.ToString());
        PlayerPrefs.SetInt(PlayerPrefKeys.InterstitialWatchCount, watchCount);
        AudioManager.Instance.MuteMasterSound();
    }

    private void OnInterstitialLoadFail(IronSourceError obj)
    {
        AnalitycsManager.Instance.LogEvent("Ad_Event", "FailedToLoadInterstitialVideo", "Desc_" + obj.getDescription() + " errorCode_" + obj.getErrorCode());
        Debug.Log("FailedToLoadInterstitialVideo " + "Desc: " + obj.getDescription() + " error code: " + obj.getErrorCode());
    }

    private void OnInterstitialClick()
    {
        int clickCount = PlayerPrefs.GetInt(PlayerPrefKeys.InterstitialClickCount, 0);
        clickCount++;
        AnalitycsManager.Instance.LogEvent("Ad_Event", "InterstitalClick", clickCount.ToString());
        PlayerPrefs.SetInt(PlayerPrefKeys.InterstitialClickCount, clickCount);
    }

    private void OnInterstitialClose()
    {
        AudioManager.Instance.UnmuteMasterSound();
    }

    #endregion
    #endregion
    public string GetAppID()
    {
#if UNITY_ANDROID
        return AndroidAppID;
#elif UNITY_IOS
        return IOSAppID;
#else
        return AndroidAppID;
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
}
