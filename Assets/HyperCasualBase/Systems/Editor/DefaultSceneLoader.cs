using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        //EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    

    static void LoadDefaultScene(PlayModeStateChange state)
    {

        //if (state == PlayModeStateChange.ExitingEditMode)
        //{
        //    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        //}

        //if (state == PlayModeStateChange.EnteredPlayMode)
        //{
        //    LevelDebugWindow.OpenWindow();
        //}
    }
}