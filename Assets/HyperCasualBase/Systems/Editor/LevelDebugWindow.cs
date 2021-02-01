using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public class LevelDebugWindow : OdinEditorWindow
{

    static LevelDebugWindow()
    {
        EditorApplication.playModeStateChanged += HandleWindow;
    }

    [ValueDropdown("Levels")]
    public int SelectLevel;

    static List<int> Levels
    {
        get
        {
            
            List<int> levelNames = new List<int>();
            for (int i = 0; i < LevelData.Levels.Count; i++)
            {
                levelNames.Add(i);
            }
            return levelNames;
        }
    }

    static LevelData LevelData
    {
        get
        {
            var levelDatas = AssetDatabase.FindAssets("t:LevelData");
            return  AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(levelDatas[0]), typeof(LevelData)) as LevelData;
        }
    }

    private static IEnumerable GetLevelData()
    {
        return UnityEditor.AssetDatabase.FindAssets("t:LevelData")
            .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
            .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<LevelData>(x)));
    }

    private static Vector2 windowSize = new Vector2(400, 50);


    public static void OpenWindow()
    {
        GetWindow<LevelDebugWindow>().Show();
        GetWindow<LevelDebugWindow>().position = new Rect((Screen.width * .5f) - (windowSize.x * .5f), (Screen.height * .5f) - (windowSize.y * .5f), windowSize.x, windowSize.y);
    }

    [Button]
    private void SetCurrentLevel()
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.LastLevel, SelectLevel);

        EditorApplication.isPaused = false;
        if(Application.isPlaying)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0, UnityEngine.SceneManagement.LoadSceneMode.Single);

        GetWindow<LevelDebugWindow>().Close();
    }

    private static void HandleWindow(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                GetWindow<LevelDebugWindow>().Close();
                break;
            case PlayModeStateChange.ExitingEditMode:
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                EditorApplication.isPaused = true;
                GetWindow<LevelDebugWindow>().Show();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                EditorApplication.isPaused = false;
                GetWindow<LevelDebugWindow>().Close();
                break;
            default:
                break;
        }
    }
}
