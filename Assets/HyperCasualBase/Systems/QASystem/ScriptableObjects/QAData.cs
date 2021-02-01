using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


public class QAData : ScriptableObject
{
    [TextArea]
    public string Question;

    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<AnswerData> AnswerDatas = new List<AnswerData>();

    private void SetDirtyEd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

[System.Serializable]
public class AnswerData
{
    [TextArea]
    public string Answer;

    public bool isCorrect;
}
