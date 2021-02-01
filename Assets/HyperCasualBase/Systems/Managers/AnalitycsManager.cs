using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Analytics;
using Facebook.Unity;
using ElephantSDK;

public class AnalitycsManager : Singleton<AnalitycsManager>
{
    private void Start()
    {
        FB.Init();
        if (!PlayerPrefs.HasKey(PlayerPrefKeys.UserID))
        {
            PlayerPrefs.SetString(PlayerPrefKeys.UserID, System.Guid.NewGuid().ToString());
            PlayerPrefs.SetInt(PlayerPrefKeys.LoginCount, 1);
            FirebaseAnalytics.LogEvent("Login_Event", "FirstLogin", "NewUserLogin");
        }
        else
        {
            int loginCount = PlayerPrefs.GetInt(PlayerPrefKeys.LoginCount, 1);
            loginCount++;
            PlayerPrefs.SetInt(PlayerPrefKeys.LoginCount, loginCount);
            FirebaseAnalytics.LogEvent("Login_Event", "ExistingUserLogedin", "LoginCount_" + loginCount);
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.OnStageFail.AddListener(SendFailEvent);
        GameManager.Instance.OnStageSuccess.AddListener(SendLevelEvent);
        SceneController.Instance.OnSceneLoaded.AddListener(SendScreenEvent);

    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageFail.RemoveListener(SendFailEvent);
        GameManager.Instance.OnStageSuccess.RemoveListener(SendLevelEvent);
        SceneController.Instance.OnSceneLoaded.RemoveListener(SendScreenEvent);
    }

    private void FacebookInitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }

    public void LogEvent(string name, string paramName, string paramValue)
    {
        FirebaseAnalytics.LogEvent(name, paramName, paramValue);
        Elephant.Event(name, SceneManager.GetActiveScene().buildIndex, Params.New().Set(paramName, paramValue));
    }

    private void SendScreenEvent()
    {
        var scene = SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel));
        FirebaseAnalytics.SetCurrentScreen(SceneManager.GetActiveScene().name, "Screen");
        Elephant.LevelStarted(SceneManager.GetActiveScene().buildIndex);
    }

    private void SendFailEvent()
    {
        FirebaseAnalytics.LogEvent("Level_Event", "Level_Fail", SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel)).name);
        Elephant.LevelFailed(SceneManager.GetActiveScene().buildIndex);
    }

    private void SendLevelEvent()
    {
        FirebaseAnalytics.LogEvent("Level_Event", "Level_Success", SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel)).name);
        Elephant.LevelCompleted(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            FirebaseAnalytics.LogEvent("Application_Event", "User_close_the_game",
                SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel)).name);

        if (!pause)
        {
            if (FB.IsInitialized)
                FB.ActivateApp();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            FirebaseAnalytics.LogEvent("Application_Event", "User_enter_the_game",
                SceneManager.GetSceneByBuildIndex(PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel)).name);
    }
}
