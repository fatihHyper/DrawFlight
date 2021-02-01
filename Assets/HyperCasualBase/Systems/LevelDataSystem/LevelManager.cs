using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;



public class LevelManager : Singleton<LevelManager>
{
    [BoxGroup("Level Data")]
    [SerializeField]
    [InlineEditor]
    private LevelData LevelData;

    public Level CurrentLevel { get { return LevelData.Levels[levelIndex]; } }


    [HideInInspector]
    public UnityEvent OnLevelStart = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnLevelFinish = new UnityEvent();

    bool isLevelStarted;
    [ReadOnly]
    [ShowInInspector]
    public bool IsLevelStarted { get { return isLevelStarted; } set { isLevelStarted = value; } }

   

    private int levelIndex
    {
        get
        {
            int level = PlayerPrefs.GetInt(PlayerPrefKeys.LastLevel, 0);
            if (level > LevelData.Levels.Count)
                level = 0;
            return level;
        }
        set
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.LastLevel, value);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStageFail.AddListener(ReloadLevel);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageFail.RemoveListener(ReloadLevel);
    }

    [Button]
    public void ReloadLevel()
    {
        FinishLevel();
        SceneController.Instance.LoadScene(CurrentLevel.LoadLevelID);
    }


    public void LoadLastLevel()
    {
        FinishLevel();
        SceneController.Instance.LoadScene(CurrentLevel.LoadLevelID);
    }

    [Button]
    public void LoadNextLevel()
    {
        FinishLevel();

        levelIndex++;
        if (levelIndex > LevelData.Levels.Count -1)
        {
            levelIndex = 0;
        }

        SceneController.Instance.LoadScene(CurrentLevel.LoadLevelID);
    }

    public void StartLevel()
    {
        if (IsLevelStarted)
            return;
        IsLevelStarted = true;
        OnLevelStart.Invoke();
    }

    public void FinishLevel()
    {
        if (!IsLevelStarted)
            return;
        IsLevelStarted = false;
        OnLevelFinish.Invoke();
    }
}
