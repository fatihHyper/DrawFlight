using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;
namespace AdvanceUI
{
    [AddComponentMenu("UI/AdvanceButton")]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    [TypeInfoBox("This Component uses button and image components, These components are hidden to keep the inspector tidy. Depending on the button type an Icon image or a TextMeshPro object is added but still kept hidden for keeping the inspector tidy")]
    public class AdvanceButton : AdvanceSelectable
    {
        #region Public Members
        [BoxGroup("Tracking")]
        [ValueDropdown("GetIds")]
        public string ConnectedPanel;

        #region ButtonType
        [BoxGroup("Button Type", Order = 1), GUIColor(0.996f, 0.623f, 0.949f)]
        //[OnValueChanged("SetIAPButton")]
        [EnumToggleButtons]
        public ButtonType ButtonType;
        [BoxGroup("Button Type", Order = 1), GUIColor(0.996f, 0.623f, 0.949f)]
        public GameObject PopUpToCreate;


        #endregion

        #endregion

        #region Events
        //[BoxGroup("Button Type", Order = 1), GUIColor("EventsColor")]
        //[ShowIf("isAdButton")]
        //public Action OnAdRewarded;

        //[BoxGroup("Button Type", Order = 1), GUIColor("EventsColor")]
        //[ShowIf("isAdButton")]
        //public Action OnAdRewardFail;

        [BoxGroup("Button Events", Order = 1), GUIColor("EventsColor")]
        public UnityEvent OnClickEvent;
        #endregion

        #region Animations
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        public AnimationTarget OnClickTarget;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        public OnClickAnimationType OnCLickAnimationType;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        public Ease OnClikcEase = Ease.InOutQuad;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        [PropertyRange(1, 5)]
        [ShowIf("isPunchScale")]
        public float scaleMultiplier = 1f;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        [ColorPalette]
        [ShowIf("colorAnim")]
        public Color SourceColor = Color.white;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        [ColorPalette]
        [ShowIf("colorAnim")]
        public Color TargetColor = Color.red;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        [InlineButton("GetLocalPos")]
        [ShowIf("moveToPos")]
        public Vector2 StartPos = Vector2.zero;
        [TabGroup("Animation", "OnClickAnimation", Order = 1), GUIColor("AnimationsColor")]
        [ShowIf("moveToPos")]
        public Vector2 TargetPos = Vector2.zero;
        #endregion



        #region ChildComponents
        [FoldoutGroup("Child Components", Order = 6)]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        [SerializeField]
        Button button;
        public Button Button { get { return (button == null) ? button = GetComponent<Button>() : button; } }



        #endregion



        #region InspectorVisiblity
        //InspectorVisiblity
        bool isPunchScale { get { return (OnCLickAnimationType == OnClickAnimationType.PunchScale) ? true : false; } }
        bool colorAnim { get { return (OnCLickAnimationType == OnClickAnimationType.ColorChange || OnCLickAnimationType == OnClickAnimationType.ColorChangeAndMove) ? true : false; } }
        bool moveToPos { get { return (OnCLickAnimationType == OnClickAnimationType.ColorChangeAndMove || OnCLickAnimationType == OnClickAnimationType.Move) ? true : false; } }
        bool isAdButton { get { return (ButtonType == ButtonType.Ad); } }

        //bool isIAPButton { get { return (ButtonType == ButtonType.IAP); } }

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            //IAPButton.OnTextUpdate += TextUpdate;
            //IAPButton iAPbutton = GetComponent<IAPButton>();
            //if (iAPbutton)
            //    DestroyImmediate(iAPbutton);


        }


        protected override void OnEnable()
        {
            base.OnEnable();
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnClick);

            if (ButtonType == ButtonType.Ad)
            {
                OnClickEvent.AddListener(PlayRewardedAd);
            }

            OnClickEvent.AddListener(PlayAudio);
            OnClickEvent.AddListener(PlayAnimation);
            //OnClickEvent.AddListener(CreatePopUp);

            //#if UNITY_PURCHASING
            //            if (ButtonType ==  ButtonType.IAP)
            //                OnClickEvent.AddListener(IAPButton.PurchaseProduct);
            //#endif

            if (CloseAnimationType != CloseAnimationType.None)
                OnClickEvent.AddListener(CloseObject);

            SetGraphic();
        }

        protected override void OnDisable()
        {
            //Button.onClick.RemoveAllListeners();
            OnClickEvent.RemoveListener(PlayAudio);
            OnClickEvent.RemoveListener(PlayAnimation);
            OnClickEvent.RemoveListener(CreatePopUp);
        }
        #endregion

        #region Advertisement
        public void PlayRewardedAd()
        {
            Debug.Log("Show Ad Button Pressed from NButton");
        }
        #endregion

        #region Button Methods
        public override void SetGraphic()
        {
            base.SetGraphic();
        }

        protected override void PlayAnimation()
        {
            base.PlayAnimation();
            Transform targetTransform = transform;
            Image targetImage = BackgroundImage;
            TextMeshProUGUI targetText = TextMesh;
            switch (OnClickTarget)
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

            switch (OnCLickAnimationType)
            {
                case OnClickAnimationType.Move:
                    targetTransform.DOComplete();

                    if (TimeScaleIndependent)
                    {
                        targetTransform.DOLocalMove(((Vector2)targetTransform.localPosition == TargetPos) ? StartPos : TargetPos, 1f).SetUpdate(true).OnComplete(CreatePopUp);
                    }
                    else
                    {
                        targetTransform.DOLocalMove(((Vector2)targetTransform.localPosition == TargetPos) ? StartPos : TargetPos, 1f).OnComplete(CreatePopUp);
                    }
                    break;
                case OnClickAnimationType.PunchScale:
                    transform.DOComplete();

                    if (TimeScaleIndependent)
                    {
                        transform.DOPunchScale(Vector3.one * (-0.1f * scaleMultiplier), 0.2f, 2, 0).SetEase(OnClikcEase).SetUpdate(true).OnComplete(CreatePopUp);
                    }
                    else
                    {
                        transform.DOPunchScale(Vector3.one * (-0.1f * scaleMultiplier), 0.2f, 2, 0).SetEase(OnClikcEase).OnComplete(CreatePopUp);
                    }
                    break;
                case OnClickAnimationType.PunchRotate:
                    transform.DOComplete();

                    if (TimeScaleIndependent)
                    {
                        transform.DOPunchRotation(Vector3.one * 0.3f, 0.3f, 3, 0).SetEase(OnClikcEase).SetUpdate(true).OnComplete(CreatePopUp);
                    }
                    else
                    {
                        transform.DOPunchRotation(Vector3.one * 0.3f, 0.3f, 3, 0).SetEase(OnClikcEase).OnComplete(CreatePopUp);
                    }
                    break;
                case OnClickAnimationType.ColorChange:
                    targetImage.DOComplete();
                    targetText.DOComplete();

                    if (TimeScaleIndependent)
                    {
                        targetImage.DOColor((targetImage.color == TargetColor) ? SourceColor : TargetColor, 0.5f).SetUpdate(true).OnComplete(CreatePopUp);
                        targetText.DOColor((targetText.color == TargetColor) ? SourceColor : TargetColor, 0.5f).SetUpdate(true).OnComplete(CreatePopUp);
                    }
                    else
                    {
                        targetImage.DOColor((targetImage.color == TargetColor) ? SourceColor : TargetColor, 0.5f).OnComplete(CreatePopUp);
                        targetText.DOColor((targetText.color == TargetColor) ? SourceColor : TargetColor, 0.5f).OnComplete(CreatePopUp);
                    }
                    break;
                case OnClickAnimationType.ColorChangeAndMove:
                    targetTransform.DOComplete();
                    targetImage.DOComplete();
                    targetText.DOComplete();

                    if (TimeScaleIndependent)
                    {
                        targetTransform.DOLocalMove(((Vector2)targetTransform.localPosition == TargetPos) ? StartPos : TargetPos, 1f).SetUpdate(true).OnComplete(CreatePopUp);
                        targetImage.DOColor((targetImage.color == TargetColor) ? SourceColor : TargetColor, 0.5f).SetUpdate(true).OnComplete(CreatePopUp);
                        targetText.DOColor((targetText.color == TargetColor) ? SourceColor : TargetColor, 0.5f).SetUpdate(true).OnComplete(CreatePopUp);
                    }
                    else
                    {
                        targetTransform.DOLocalMove(((Vector2)targetTransform.localPosition == TargetPos) ? StartPos : TargetPos, 1f).OnComplete(CreatePopUp);
                        targetImage.DOColor((targetImage.color == TargetColor) ? SourceColor : TargetColor, 0.5f).OnComplete(CreatePopUp);
                        targetText.DOColor((targetText.color == TargetColor) ? SourceColor : TargetColor, 0.5f).OnComplete(CreatePopUp);
                    }
                    break;
                case OnClickAnimationType.None:
                    CreatePopUp();
                    break;
            }

            //Debug.Log("Click Animation Played " + OnClickTarget + " " + OnCLickAnimationType);
        }

        void OnClick()
        {
            OnClickEvent.Invoke();
            if (!string.IsNullOrEmpty(ConnectedPanel))
                UIIDHolder.Panels[ConnectedPanel].TooglePanel();

        }
        #endregion

        void GetLocalPos()
        {
            StartPos = transform.localPosition;
        }

        void CreatePopUp()
        {
            if (PopUpToCreate)
            {

            }
        }
    }
}

