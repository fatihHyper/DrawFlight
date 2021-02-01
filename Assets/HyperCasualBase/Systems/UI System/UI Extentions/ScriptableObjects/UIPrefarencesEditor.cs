using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI Prefarences", menuName = "UI/UI Preferences", order = 1)]
public class UIPrefarencesEditor : ScriptableObject
{
    #region Button
    [TabGroup("Components", "Button Preferences")]
    [LabelText("BackgroundSprite")]
    [PreviewField]
    public Sprite ButtonBackgroundSprite;
    [TabGroup("Components", "Button Preferences")]
    [LabelText("IconSprite")]
    [PreviewField]
    public Sprite ButtonIconSprite;
    #endregion

    #region Text
    [TabGroup("Components", "Text Preferences")]
    public TMP_FontAsset TextFont;
    [TabGroup("Components", "Text Preferences")]
    public FontStyles FontStyle;
    [TabGroup("Components", "Text Preferences")]
    public int FontSize;
    [TabGroup("Components", "Text Preferences")]
    public TextAlignmentOptions alignmentTypes;
    #endregion

}
