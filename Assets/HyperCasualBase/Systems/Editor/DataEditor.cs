using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class DataEditor : OdinEditorWindow
{
    public PlayerData PlayerData;

    [MenuItem("HyperCasualBase/Data Editor")]
    private static void OpenWindow()
    {
        GetWindow<DataEditor>().Show();

    }
    [Button]
    protected override void Initialize()
    {
        base.Initialize();
        PlayerData = SaveLoadManager.LoadPDP<PlayerData>(SavedFileNameHolder.PlayerData, new PlayerData());
    }


    [Button]
    public void SaveData()
    {
        SaveLoadManager.SavePDP(PlayerData, SavedFileNameHolder.PlayerData);
    }

    [Button]
    public void DeleteData()
    {
        SaveLoadManager.DeleteFile(SavedFileNameHolder.PlayerData);
        PlayerData = new PlayerData();
    }

}
