using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is a class for holding Choice Data and a boolean
/// </summary>
public class MultiChoiceData : MonoBehaviour
{
    public List<Choice> Choices = new List<Choice>();
}
[System.Serializable]
public abstract class Choice : IChoice
{
    public ChoiceData ChoiceData;
    public bool IsCorrect;

    public abstract void Job();
}