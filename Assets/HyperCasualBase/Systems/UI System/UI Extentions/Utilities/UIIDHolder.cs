using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public static class UIIDHolder
{
    public static string GameStartPanel = "GameStartPanel";
    public static string SkinPanel = "SkinPanel";
    public static string LevelPanel = "LevelPanel";
    public static string MultiChoicePanel = "MultiChoicePanel";
    public static string JoystickPanel = "JoystickPanel";
    public static string QAPanel = "QAPanel";

    public static List<string> UIIds
    {
        get
        {
            List<string> ids = new List<string>();
            ids.Add(GameStartPanel);
            ids.Add(SkinPanel);
            ids.Add(LevelPanel);
            ids.Add(JoystickPanel);
            ids.Add(QAPanel);
            return ids;
        }
    }

    public static Dictionary<string, AdvanceUI.AdvancePanel> Panels = new Dictionary<string, AdvanceUI.AdvancePanel>();
}
