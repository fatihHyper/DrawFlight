#region Custom UI

#region Panel
public enum PanelState
{
    Open,
    Close
}
#endregion

#region Button
public enum ButtonType
{
    Normal,
    Ad
}

public enum OnClickAnimationType
{
    None,
    Move,
    PunchScale,
    PunchRotate,
    ColorChange,
    ColorChangeAndMove
}
#endregion

#region Dropdown

#endregion

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VerticalStretchLeft,
    VerticalStretchRight,
    VerticalStretchCenter,

    HorizontalStretchTop,
    HorizontalStretchMiddle,
    HorizontalStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight
}

#region Slider
public enum SliderAnimationTarget
{

}
#endregion

#region UI Animation
public enum AnimationTarget
{
    Self,
    Icon,
    Text
}

public enum CloseAnimationType
{
    None,
    Shrink,
    Fade
}

public enum IdleAnimationType
{
    None,
    SpriteAnimation,
    IdlePunchScale,
    IdlePulseRotate,
    IdleJump
}

public enum PanelAnimation
{
    None,
    Scale,
    Slide
}
#endregion
#endregion