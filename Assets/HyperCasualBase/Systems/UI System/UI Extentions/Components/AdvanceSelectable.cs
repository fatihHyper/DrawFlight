using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;
using System.Linq;

namespace AdvanceUI
{
    public class AdvanceSelectable : AdvancePanel
    {
        //[ValueDropdown("GetIDList", AppendNextDrawer = true)]
        //[InlineButton("AddValueToElementIDHolder", "Add")]
        //public string ID;


//#if UNITY_EDITOR
//        public List<string> GetIDList { get { return elementsIDHolder.SelectableIDs; } }

//        private UIElementsIDHolder elementsIDHolder => (UIElementsIDHolder)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/PerfectHeist/Systems/UI System/UI Extentions/Data/UIelements ID Holder.asset", typeof(UIElementsIDHolder));
//#endif

        #region Graphic
        [TabGroup("Graphic", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [PreviewField]
        public Sprite BackgroundSprite;

        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ColorPalette]
        public Color NormalColor = Color.white;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ColorPalette]
        public Color HighlightedColor = Color.white;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ColorPalette]
        public Color PressedColor = Color.gray;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ColorPalette]
        public Color SelectedColor = Color.white;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ColorPalette]
        public Color DisabledColor = Color.gray;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [Range(1, 5)]
        public float ColorMultiplier = 1;
        [TabGroup("Interactable Colors", Order = 2)]
        [OnValueChanged("SetGraphic")]
        public float FadeDuration = 0.1f;

        [TabGroup("Graphic", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [PreviewField]
        public Sprite IconSprite;
        [TabGroup("Graphic", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ShowIf("isIconActive")]
        [ColorPalette]
        public Color IconNormalColor = Color.white;

        [TabGroup("Graphic", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [TextArea]
        public string ComponentText;
        [TabGroup("Graphic", Order = 2)]
        [OnValueChanged("SetGraphic")]
        [ShowIf("isTextActive")]
        [ColorPalette]
        public Color TextColor = Color.white;
        #endregion

        #region Audio

        [TabGroup("Audio", Order = 2), GUIColor("AudioColor")]
        [ValueDropdown("audioKeys")]
        //[InlineEditor (InlineEditorModes.SmallPreview, PreviewHeight = 130)]
        public string ClickSound = AudioKeys.ButtonClick;
        #endregion

        #region Events
        //Events
        [GUIColor("EventsColor")]
        [BoxGroup("Events", Order = 2)]
        public UnityEvent OnEnableEvent;
        [GUIColor("EventsColor")]
        [BoxGroup("Events", Order = 2)]
        public UnityEvent OnDisableEvetn;

        #endregion

        #region Animation
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Target")]
        public AnimationTarget IdleTarget;
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Animation Type")]
        public IdleAnimationType IdleAnimationType;
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Ease")]
        public Ease IdleEase = Ease.InOutQuad;
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        public float IdleDelay = 0;
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [PropertyRange(0.1f, 5)]
        public float IdleDuration = 2;
        [TabGroup("Animation", "IdleAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [ShowIf("spriteAnim")]
        public List<Sprite> sprites;

        [TabGroup("Animation", "CloseAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [InfoBox("If Target Object is Null Animation will target the CloseTarget else animation will target the given transform")]
        public Transform TargetObject;
        [TabGroup("Animation", "CloseAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Target")]
        public AnimationTarget CloseTarget;
        [TabGroup("Animation", "CloseAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Animation Type")]
        public CloseAnimationType CloseAnimationType;
        [TabGroup("Animation", "CloseAnimation", Order = 2)]
        [GUIColor("AnimationsColor")]
        [LabelText("Ease")]
        public Ease CloseEase = Ease.InOutQuad;
        #endregion

        #region Child Components
        [FoldoutGroup("Child Components", Order = 6)]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        public Image BackgroundImage;

        [FoldoutGroup("Child Components")]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        public Image IconImage;

        [FoldoutGroup("Child Components")]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        public TextMeshProUGUI TextMesh;

        private Selectable selectable;

        private Selectable Selectable { get { return selectable ?? (selectable = GetComponentInChildren<Selectable>()); } }
        #endregion

        #region Starting References
        //Starting References
        Vector3 objectStartScale;
        Vector2 objectStartPosition;
        #endregion

        #region GUI Colors
        Color AudioColor = new Color(0.901f, 0.780f, 0.019f);
        #endregion

        #region Inspector Visibility
        bool spriteAnim { get { return (IdleAnimationType == IdleAnimationType.SpriteAnimation) ? true : false; } }

        bool isTextActive { get { return (!string.IsNullOrEmpty(ComponentText)); } }

        bool isIconActive { get { return (IconSprite); } }
        #endregion


        #region Unity Methods
        protected virtual void Start()
        {
            UISelectableHolder.AddToHolder(this);
            objectStartScale = transform.localScale;
            objectStartPosition = transform.position;
        }
        protected virtual void OnEnable()
        {
            SetIdleAnimation();

            OnEnableEvent.Invoke();
        }
        protected virtual void OnDisable()
        {
            OnDisableEvetn.Invoke();
        }
        #endregion

        #region Public Methods
        public virtual void SetGraphic()
        {
            if (BackgroundImage)
                BackgroundImage.sprite = BackgroundSprite;

            if (TextMesh)
            {
                TextMesh.SetText(ComponentText);
            }

            if (IconImage)
                IconImage.sprite = IconSprite;

            //BackgroundImage.color = Color.white;
            if (IconImage)
                IconImage.color = IconNormalColor;
            if (TextMesh)
                TextMesh.color = TextColor;

            if (IconImage)
                IconImage.enabled = ((string.IsNullOrEmpty(ComponentText)) ? IconImage.enabled = true : IconImage.enabled = false);
            //if (IconImage)
            //    IconImage.gameObject.hideFlags = ((string.IsNullOrEmpty(ComponentText)) ? HideFlags.None : HideFlags.HideInHierarchy);

            //if (TextMesh)
            //    TextMesh.gameObject.hideFlags = ((string.IsNullOrEmpty(ComponentText)) ? HideFlags.HideInHierarchy : HideFlags.None);

            if (IconImage)
                IconImage.enabled = (IconSprite == null) ? false : true;
            if (BackgroundImage)
                BackgroundImage.enabled = (BackgroundSprite == null) ? false : true;
            //if (BackgroundImage)
            //    BackgroundImage.SetNativeSize();



            SetInteractable();
        }

        protected void SetInteractable()
        {
            var colors = Selectable.colors;
            colors.normalColor = NormalColor;
            colors.highlightedColor = HighlightedColor;
            colors.pressedColor = PressedColor;
            colors.selectedColor = SelectedColor;
            colors.disabledColor = DisabledColor;
            colors.colorMultiplier = ColorMultiplier;
            colors.fadeDuration = FadeDuration;
            colors.colorMultiplier = ColorMultiplier;
            colors.fadeDuration = FadeDuration;
            Selectable.colors = colors;
        }

        public override void ShowPanel()
        {
            UISelectableHolder.AddToHolder(this);
            base.ShowPanel();
        }

        public override void HidePanel()
        {
            UISelectableHolder.RemoveFromHolder(this);
            base.HidePanel();
        }

        #endregion

        #region Private Methods
        protected virtual void PlayAudio()
        {
            AudioManager.Instance?.PlayOneShot2D(ClickSound);
        }

        protected virtual void SetIdleAnimation()
        {
            Transform targetTransform = transform;
            Image targetImage = BackgroundImage;
            TextMeshProUGUI targetText = TextMesh;
            switch (IdleTarget)
            {
                case AnimationTarget.Self:
                    targetTransform = transform;
                    targetImage = BackgroundImage;
                    break;
                case AnimationTarget.Icon:
                    targetTransform = IconImage.transform;
                    targetImage = IconImage;
                    break;
                case AnimationTarget.Text:
                    targetTransform = TextMesh.transform;
                    break;
            }

            switch (IdleAnimationType)
            {
                case IdleAnimationType.IdleJump:
                    Sequence JumpSequence = DOTween.Sequence();
                    JumpSequence.Append(targetTransform.DOJump(transform.position, 75, 2, IdleDuration).SetDelay(IdleDelay).SetLoops(-1).SetEase(IdleEase));
                    break;
                case IdleAnimationType.IdlePulseRotate:
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(targetTransform.DOPunchRotation(Vector3.forward * 5f, IdleDuration)).SetDelay(IdleDelay).SetEase(IdleEase);
                    sequence.SetLoops(-1);
                    break;
                case IdleAnimationType.IdlePunchScale:
                    Sequence ScaleSequence = DOTween.Sequence();
                    ScaleSequence.Append(targetTransform.DOPunchScale(Vector3.one * 0.1f, IdleDuration, 0).SetEase(IdleEase)).AppendInterval(IdleDelay).SetLoops(-1);
                    break;
                case IdleAnimationType.SpriteAnimation:
                    GameObject animationImage = new GameObject();
                    animationImage.gameObject.name = "Sprite Animation Image";
                    animationImage.transform.SetParent(transform);
                    targetImage = animationImage.AddComponent<Image>();
                    targetImage.preserveAspect = true;
                    targetImage.raycastTarget = false;
                    RectTransform animationImageRect = animationImage.GetComponent<RectTransform>();
                    animationImageRect.anchoredPosition = Vector2.zero;
                    animationImageRect.sizeDelta = Vector2.zero;
                    animationImageRect.anchorMin = new Vector2(0, 0);
                    animationImageRect.anchorMax = new Vector2(1, 1);
                    animationImageRect.pivot = new Vector2(0.5f, 0.5f);

                    StartCoroutine(PlaySpriteAnimation(targetImage));
                    break;
            }
        }

        protected IEnumerator PlaySpriteAnimation(Image sourceImage)
        {
            while (true)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    sourceImage.sprite = sprites[i];
                    yield return new WaitForSeconds(0.1f);
                }
            }

        }

        protected virtual void PlayAnimation()
        {

        }

        protected void CloseObject()
        {
            Transform TargetTransform = transform;
            switch (CloseTarget)
            {
                case AnimationTarget.Self:
                    TargetTransform = transform;
                    break;
                case AnimationTarget.Icon:
                    TargetTransform = IconImage.transform;
                    break;
                case AnimationTarget.Text:
                    TargetTransform = TextMesh.transform;
                    break;
            }

            if (TargetObject != null)
                TargetTransform = TargetObject;

            switch (CloseAnimationType)
            {
                case CloseAnimationType.Shrink:
                    var tween = TargetTransform.DOScale(Vector3.zero, 1f).SetEase(CloseEase).OnComplete(HidePanel);
                    transform.localScale = objectStartScale;
                    break;
                case CloseAnimationType.Fade:
                    BackgroundImage.DOFade(0, 0.5f).OnComplete(HidePanel).SetEase(CloseEase).OnComplete(ResetSelectable);
                    IconImage.DOFade(0, 0.5f).SetEase(CloseEase).OnComplete(ResetSelectable);
                    TextMesh.DOFade(0, 0.5f).SetEase(CloseEase).OnComplete(ResetSelectable);
                    break;
            }
        }

        #endregion

        void ResetSelectable()
        {
            //BackgroundImage.color = BackgroundNormalColor;
            IconImage.color = IconNormalColor;
            TextMesh.color = TextColor;
        }
//#if UNITY_EDITOR

//        void AddValueToElementIDHolder()
//        {
//            if (!string.IsNullOrEmpty(ID))
//            {
//                elementsIDHolder.SelectableIDs.Add(ID);
//                UnityEditor.EditorUtility.SetDirty(elementsIDHolder);
//            }
//        }
//#endif
    }
}

