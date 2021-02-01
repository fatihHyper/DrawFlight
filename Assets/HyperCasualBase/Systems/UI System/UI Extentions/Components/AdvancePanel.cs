using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace AdvanceUI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutElement))]
    [TypeInfoBox("This Component Uses Canvas Group and Layout Element, These two components are added to the game object but hidden to keep inspector tidy")]
    [BoxGroup("Panel", Order = 5)]
    public class AdvancePanel : MonoBehaviour
    {
        [BoxGroup("Tracking")]
        [ValueDropdown("GetIds")]
        public string PanelID;

        protected List<string> GetIds { get { return new List<string>(UIIDHolder.UIIds); } }

        public PanelState panelState;
        public bool isOpen
        {
            get
            {
                return panelState.Equals(PanelState.Open);
            }
        }

        #region Privite Components
        private CanvasGroup canvasGroup;
        private CanvasGroup CanvasGroup { get { return (canvasGroup == null) ? canvasGroup = GetComponent<CanvasGroup>() : canvasGroup; } }

        LayoutElement layoutElement;
        private LayoutElement LayoutElement { get { return (layoutElement == null) ? layoutElement = GetComponent<LayoutElement>() : layoutElement; } }
        #endregion

        #region Public Variables
        [BoxGroup("Panel/Settings", Order = 5)]
        public bool DestroyOnHide = false;
        [BoxGroup("Panel/Settings", Order = 5)]
        [OnValueChanged("SetPanelComponents")]
        public bool IgnoreLayout = false;
        #endregion

        #region Events
        [BoxGroup("Panel", Order = 5)]
        [FoldoutGroup("Panel/Events", Order = 5), GUIColor("EventsColor")]
        public UnityEvent OnShowPanel;
        [FoldoutGroup("Panel/Events", Order = 5), GUIColor("EventsColor")]
        public UnityEvent OnHidePanel;
        #endregion

        #region Animation

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [InfoBox("If Null Transform it self will be animated.")]
        public Transform AnimationTargetPanel;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [InfoBox("If True the animation will play independent of timescale.")]
        public bool TimeScaleIndependent = false;

        [GUIColor("AnimationsColor")]
        [FoldoutGroup("Panel/Animation", Order = 5)]
        public float AnimationDelay = 0;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [PropertyRange(1, 5)]
        public float OpenSpeedMultiplier = 1;


        [GUIColor("AnimationsColor")]
        [FoldoutGroup("Panel/Animation", Order = 5)]
        public PanelAnimation OpenAnimation;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimOpen")]
        [LabelText("StartPosition")]
        public Vector2 OpenStartPosition;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimOpen")]
        [LabelText("TargetPosition")]
        [InlineButton("GetCurrentPositionTarget", "GetCurrentPosition")]
        public Vector2 OpenTargetPosition;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimOpen")]
        [LabelText("TargetPosition")]
        public Transform OpenTargetTransform;

        #region Audio

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [ValueDropdown("audioKeys")]
        public string OpenPanelSoundEffect = AudioKeys.PopUpOpen;
        #endregion
        private Sequence sequence;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [PropertyRange(1, 5)]
        public float CloseSpeedMultiplier = 1;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        public PanelAnimation CloseAnimation;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimClose")]
        [LabelText("StartPosition")]
        public Vector2 CloseStartPosition;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimClose")]
        [LabelText("TargetPosition")]
        [InlineButton("GetCurrentPositionTarget", "GetCurrentPosition")]
        public Vector2 CloseTargetPosition;

        [FoldoutGroup("Panel/Animation", Order = 5)]
        [GUIColor("AnimationsColor")]
        [ShowIf("slideAnimClose")]
        [LabelText("StartPosition")]
        public Transform CloseTargetTransfrom;

        #region Audio
       
        [FoldoutGroup("Panel/Animation", Order = 5)]
        [ValueDropdown("audioKeys")]
        public string ClosePanelSoundEffect = AudioKeys.PopUpClose;
        #endregion
        #endregion


        #region Inspector Visibility
        bool slideAnimOpen { get { return (OpenAnimation == PanelAnimation.Slide) ? true : false; } }
        bool slideAnimClose { get { return (CloseAnimation == PanelAnimation.Slide) ? true : false; } }

        #endregion


        #region GUI Colors
        protected Color EventsColor = new Color(0.223f, 0.992f, 0.996f);
        protected Color AnimationsColor = new Color(1f, 0.992f, 0.639f);
        protected Color GraphicsColor = new Color(1f, 0.631f, 0.239f);
        #endregion


        protected Transform targetTransform { get { return (AnimationTargetPanel == null) ? transform : AnimationTargetPanel; } }

        Vector2 startPos;
        #region Editor Code
#if UNITY_EDITOR

        protected static List<string> audioKeys
        {
            get
            {
                List<string> keys = new List<string>();
                for (int i = 0; i < AudioDatas.Count; i++)
                {
                    foreach (var item in AudioDatas[i].AudioClips)
                    {
                        keys.Add(item.Key);
                    }
                }
                return keys;
            }
        }


        static List<AudioData> AudioDatas
        {
            get
            {
                var guid = UnityEditor.AssetDatabase.FindAssets("t:AudioData");

                List<AudioData> datas = new List<AudioData>();
                for (int i = 0; i < guid.Length; i++)
                {
                    datas.Add(UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guid[i]), typeof(AudioData)) as AudioData);
                }
                return datas;
            }
        }
#endif
        #endregion

        protected virtual void Start()
        {
            startPos = GetComponent<RectTransform>().anchoredPosition;
            UIIDHolder.Panels[PanelID] = this;

        }

        /// <summary>
        /// Shows the panel if it's hidden, hides if open.
        /// </summary>
        public virtual void TooglePanel()
        {

            switch (panelState)
            {
                case PanelState.Open:
                    HidePanelAnimated();
                    break;
                case PanelState.Close:
                    ShowPanelAnimated();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Shows panel without animation
        /// </summary>
        [ButtonGroup("Panel/PanelControl"), GUIColor(0.847f, 0.992f, 0.768f)]
        public virtual void ShowPanel()
        {
            if (panelState == PanelState.Open)
                return;

            OnShowPanel.Invoke();
            PlaySound(OpenPanelSoundEffect);
            panelState = PanelState.Open;
            SetPanel(false, 1, true, true);
        }
        /// <summary>
        /// Shows panel with animation
        /// </summary>
        [ButtonGroup("Panel/AnimatedPanelControl"), GUIColor(0.847f, 0.992f, 0.768f)]
        public virtual void ShowPanelAnimated()
        {
            if (panelState == PanelState.Open)
                return;

            sequence?.Complete();
            sequence = DOTween.Sequence();

            switch (OpenAnimation)
            {
                case PanelAnimation.Scale:
                    panelState = PanelState.Open;
                    //targetTransform.localPosition = Vector2.zero;
                    //targetTransform.localScale = Vector3.zero;
                    targetTransform.localScale = Vector3.one;

                    sequence.AppendInterval(AnimationDelay).Append(targetTransform.DOScale(Vector3.zero, 0.01f)).AppendCallback(() => PlaySound(OpenPanelSoundEffect))
                    .AppendCallback(() => SetPanel(false, 1, true, true))
                .Append(targetTransform.DOScale(Vector3.one, 0.5f / OpenSpeedMultiplier))
                .Append(targetTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0))

                    .OnComplete(OnShowPanel.Invoke).SetUpdate(TimeScaleIndependent);

                    return;
                case PanelAnimation.Slide:
                    panelState = PanelState.Open;

                    targetTransform.localScale = Vector3.one;
                    targetTransform.localPosition = OpenStartPosition;
                    if (OpenTargetTransform != null)
                        OpenTargetPosition = OpenTargetTransform.localPosition;

                    sequence.AppendInterval(AnimationDelay).AppendCallback(() => PlaySound(OpenPanelSoundEffect))
                    .AppendCallback(() => SetPanel(false, 1, true, true)).Append(targetTransform.DOLocalMove(OpenTargetPosition, 0.5f / OpenSpeedMultiplier))
                    .Append(targetTransform.DOPunchPosition(GetDirection(OpenStartPosition) * 10, 0.5f))
                    .OnComplete(() => OnShowPanel.Invoke()).SetUpdate(TimeScaleIndependent);
                    return;
                case PanelAnimation.None:
                    ShowPanel();
                    break;
            }
        }
        /// <summary>
        /// Shows panel with animation. Calls the Call back when Animation Finishes.
        /// 
        /// Usage: () => Callback Method
        /// </summary> 
        /// <param name="action"></param>
        public virtual void ShowPanelAnimated(TweenCallback action)
        {
            if (panelState == PanelState.Open)
                return;

            sequence?.Complete();
            sequence = DOTween.Sequence();

            switch (OpenAnimation)
            {
                case PanelAnimation.Scale:
                    panelState = PanelState.Open;

                    //targetTransform.localPosition = Vector2.zero;
                    targetTransform.localScale = Vector3.zero;

                    sequence.Append(targetTransform.DOScale(Vector3.one, 0.5f))
                    .AppendCallback(() => PlaySound(OpenPanelSoundEffect))
                    .AppendCallback(() => SetPanel(false, 1, true, true))
                    .Append(targetTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0)).AppendCallback(OnShowPanel.Invoke)
                    .OnComplete(action).SetUpdate(TimeScaleIndependent);
                    return;
                case PanelAnimation.Slide:
                    panelState = PanelState.Open;

                    targetTransform.localScale = Vector3.one;
                    targetTransform.localPosition = OpenStartPosition;

                    if (OpenTargetTransform != null)
                        OpenTargetPosition = OpenTargetTransform.localPosition;

                    sequence.Append(targetTransform.DOLocalMove(OpenTargetPosition, 0.5f)).Append(targetTransform.DOPunchPosition(GetDirection(OpenStartPosition) * 10, 0.5f))
                    .AppendCallback(() => PlaySound(OpenPanelSoundEffect))
                    .AppendCallback(() => SetPanel(false, 1, true, true))
                    .AppendCallback(OnShowPanel.Invoke)
                    .OnComplete(action).SetUpdate(TimeScaleIndependent);
                    return;

                case PanelAnimation.None:
                    ShowPanel();
                    break;
            }
        }

        /// <summary>
        /// Hides panel without animation
        /// </summary>
        [ButtonGroup("Panel/PanelControl"), GUIColor(1f, 0.239f, 0.309f)]
        public virtual void HidePanel()
        {
            if (panelState == PanelState.Close)
                return;

            panelState = PanelState.Close;
            PlaySound(ClosePanelSoundEffect);
            OnHidePanel.Invoke();
            SetPanel(true, 0, false, false);

            if (DestroyOnHide)
            {
                if (Application.isPlaying)
                    Destroy(gameObject);
            }
        }

        /// <summary>
        /// Hides panel without animation
        /// </summary>
        public virtual void HidePanel(bool destroyOnHide)
        {
            if (panelState == PanelState.Close)
                return;

            panelState = PanelState.Close;
            PlaySound(ClosePanelSoundEffect);
            SetPanel(true, 0, false, false);
            OnHidePanel.Invoke();

            if (destroyOnHide)
            {
                if (Application.isPlaying)
                    Destroy(gameObject);
            }
        }
        /// <summary>
        /// Hides panel with animation
        /// </summary>
        [ButtonGroup("Panel/AnimatedPanelControl"), GUIColor(1f, 0.239f, 0.309f)]
        public virtual void HidePanelAnimated()
        {
            if (panelState == PanelState.Close)
                return;

            sequence?.Complete();
            sequence = DOTween.Sequence();

            PlaySound(ClosePanelSoundEffect);

            switch (CloseAnimation)
            {
                case PanelAnimation.Scale:
                    panelState = PanelState.Close;

                    sequence.Append(targetTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0)).Append(targetTransform.DOScale(Vector3.zero, 0.5f / CloseSpeedMultiplier)).AppendCallback(OnHidePanel.Invoke)
                .OnComplete(() =>
                {
                    SetPanel(true, 0, false, false);
                    if (DestroyOnHide)
                    {
                        if (Application.isPlaying)
                            Destroy(gameObject);
                    }
                }).SetUpdate(TimeScaleIndependent);

                    return;
                case PanelAnimation.Slide:
                    panelState = PanelState.Close;

                    targetTransform.localScale = Vector3.one;

                    if (OpenTargetTransform != null)
                        CloseStartPosition = OpenTargetTransform.localPosition;

                    targetTransform.localPosition = CloseStartPosition;



                    if (CloseTargetTransfrom != null)
                        CloseTargetPosition = CloseTargetTransfrom.localPosition;

                    sequence.Append(targetTransform.DOLocalMove(CloseTargetPosition, 0.2f / CloseSpeedMultiplier))
                .AppendCallback(OnHidePanel.Invoke)
                .OnComplete(() =>
                {
                    SetPanel(true, 0, false, false);
                    if (DestroyOnHide)
                    {
                        if (Application.isPlaying)
                            Destroy(gameObject);
                    }
                }).SetUpdate(TimeScaleIndependent);
                    return;

                case PanelAnimation.None:
                    HidePanel();
                        break;
            }
        }


        /// <summary>
        /// Hides panel with animation. Calls the Call back when Animation Finishes.
        /// 
        /// Usage: () => Callback Method
        /// </summary> 
        /// <param name="action"></param>
        public virtual void HidePanelAnimated(TweenCallback action)
        {

            if (panelState == PanelState.Close)
                return;
            sequence?.Complete();
            sequence = DOTween.Sequence();



            PlaySound(ClosePanelSoundEffect);

            switch (CloseAnimation)
            {
                case PanelAnimation.Scale:
                    panelState = PanelState.Close;

                    sequence.Append(targetTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 1, 0))
               .Append(targetTransform.DOScale(Vector3.zero, 0.5f / CloseSpeedMultiplier))
               .AppendCallback(OnHidePanel.Invoke)
               .AppendCallback(() =>
               {
                   SetPanel(true, 0, false, false);
                   if (DestroyOnHide)
                   {
                       if (Application.isPlaying)
                           Destroy(gameObject);
                   }
               })
           .OnComplete(action).SetUpdate(TimeScaleIndependent);
                    return;
                case PanelAnimation.Slide:
                    panelState = PanelState.Close;

                    if (OpenTargetTransform != null)
                        CloseStartPosition = OpenTargetTransform.localPosition;

                    targetTransform.localScale = Vector3.one;
                    targetTransform.localPosition = CloseStartPosition;

                    sequence.Append(targetTransform.DOPunchPosition(GetDirection(CloseTargetPosition) * 10, 0.5f))
                .Append(targetTransform.DOLocalMove(CloseTargetPosition, 0.2f / CloseSpeedMultiplier))
                .AppendCallback(OnHidePanel.Invoke)
                .AppendCallback(() =>
                {
                    SetPanel(true, 0, false, false);
                    if (DestroyOnHide)
                    {
                        if (Application.isPlaying)
                            Destroy(gameObject);
                    }
                })
                .OnComplete(action).SetUpdate(TimeScaleIndependent);
                    return;

                case PanelAnimation.None:
                    HidePanel();
                    break;
            }
        }

        protected void SetPanel(bool ignoreLayout, float alpha, bool blockRaycast, bool interactable)
        {
            if (LayoutElement == null)
                Debug.LogError("Layout Element is null");

            if (CanvasGroup == null)
                Debug.LogError("CanvasGroup is null");

            if (!IgnoreLayout)
                LayoutElement.ignoreLayout = ignoreLayout;

            CanvasGroup.alpha = alpha;
            CanvasGroup.blocksRaycasts = blockRaycast;
            CanvasGroup.interactable = interactable;

            RectTransform parent = transform.parent.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);

        }

        void SetPanelComponents()
        {
            LayoutElement.ignoreLayout = IgnoreLayout;
        }

        protected Vector2 GetDirection(Vector2 source)
        {
            if (source.x > 0)
                return Vector2.right;
            if (source.x < 0)
                return Vector2.left;
            if (source.y > 0)
                return Vector2.up;
            if (source.y < 0)
                return Vector2.down;

            return Vector2.zero;
        }

        void GetCurrentPositionTarget()
        {
            OpenStartPosition = (Vector2)targetTransform.localPosition;
            CloseTargetPosition = OpenStartPosition;
        }

        protected void PlaySound(string soundName)
        {
            if (string.Equals(soundName, "None"))
                return;

            AudioManager.Instance?.PlayOneShot2D(soundName);
        }

        private void OnDestroy()
        {
            UIIDHolder.Panels.Remove(PanelID);
        }
    }
}