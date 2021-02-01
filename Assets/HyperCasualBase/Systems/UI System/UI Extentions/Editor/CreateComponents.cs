using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using AdvanceUI;
using TMPro;
using UnityEngine.Events;

public class CreateComponents 
{

    [MenuItem("GameObject/UI/Advance Panel", false, 0)]
    public static void CreateAdvancePanel()
    {
        CreateComponent(CreatePanel);
    }

    static void CreatePanel(Transform parent)
    {
        UIPrefarencesEditor uIPrefarencesEditor = GetUIPrefarences();
        GameObject NatumPanel = new GameObject();
        NatumPanel.transform.SetParent(parent);
        NatumPanel.gameObject.name = "Panel";
        NatumPanel.AddComponent<AdvancePanel>();

        RectTransform panelRect = NatumPanel.GetComponent<RectTransform>();
        panelRect.SetAnchor(AnchorPresets.StretchAll);
        panelRect.anchoredPosition = Vector3.zero;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.localScale = Vector3.one;
    }


    [MenuItem("GameObject/UI/Advance Button", false, 0)]
    public static void CreateAdvanceButton()
    {
        CreateComponent(CreateButton);
    }

    static void CreateButton(Transform parent)
    {
        UIPrefarencesEditor uIPrefarencesEditor = GetUIPrefarences();
        GameObject NatumButtonObject = new GameObject();
        NatumButtonObject.gameObject.name = "Button";
        NatumButtonObject.transform.SetParent(parent.transform);
        AdvanceButton nButton = NatumButtonObject.AddComponent<AdvanceButton>();
        nButton.BackgroundImage = NatumButtonObject.GetComponent<Image>(); ;
        nButton.BackgroundSprite = uIPrefarencesEditor.ButtonBackgroundSprite;
        nButton.IconSprite = uIPrefarencesEditor.ButtonIconSprite;
        nButton.BackgroundImage.preserveAspect = true;
        nButton.transform.localScale = Vector3.one;
        nButton.BackgroundImage.hideFlags = HideFlags.HideInInspector;

        GameObject iconImageObject = new GameObject();
        iconImageObject.gameObject.name = "Icon Image";
        iconImageObject.transform.SetParent(NatumButtonObject.transform);
        Image iconImage = iconImageObject.AddComponent<Image>();
        nButton.IconImage = iconImage;
        nButton.IconImage = iconImage;
        iconImage.raycastTarget = false;
        iconImage.preserveAspect = true;
        //iconImage.SetNativeSize();

        //iconImageObject.hideFlags = HideFlags.HideInHierarchy;

        GameObject textMeshObject = new GameObject();
        textMeshObject.transform.SetParent(NatumButtonObject.transform);
        textMeshObject.gameObject.name = "TextMeshProText";
        TextMeshProUGUI textMesh = textMeshObject.AddComponent<TextMeshProUGUI>();
        nButton.TextMesh = textMesh;
        textMesh.raycastTarget = false;
        textMesh.fontSize = 36;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.font = uIPrefarencesEditor.TextFont;

        //textMeshObject.hideFlags = HideFlags.HideInHierarchy;
        nButton.SetGraphic();

       

        RectTransform textMeshRect =  textMeshObject.GetComponent<RectTransform>();
        textMeshRect.anchoredPosition = Vector2.zero;
        textMeshRect.sizeDelta = Vector2.zero;
        textMeshRect.transform.localScale = Vector3.one;
        textMeshRect.anchorMin = new Vector2(0, 0);
        textMeshRect.anchorMax = new Vector2(1, 1);
        textMeshRect.pivot = new Vector2(0.5f, 0.5f);

        RectTransform iconRect = iconImageObject.GetComponent<RectTransform>();
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.sizeDelta = Vector2.zero;
        iconRect.transform.localScale = Vector3.one;
        iconRect.anchorMin = new Vector2(0, 0);
        iconRect.anchorMax = new Vector2(1, 1);
        iconRect.pivot = new Vector2(0.5f, 0.5f);

        RectTransform buttonRect = NatumButtonObject.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = Vector2.zero;
        nButton.BackgroundImage.SetNativeSize();

        Selection.activeObject = NatumButtonObject;  
    }


    static Transform CreateCanvas()
    {
        GameObject canvasObject = new GameObject();
        canvasObject.gameObject.name = "Canvas";
        canvasObject.AddComponent<RectTransform>();
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(480, 800);

        return canvasObject.transform;
    }

    static void CreateComponent(UnityAction<Transform> action)
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject)
        {
            RectTransform rectTransform = selectedObject.GetComponent<RectTransform>();
            if (rectTransform)
                action.Invoke(selectedObject.transform);
            else
            {
                Canvas canvas = GameObject.FindObjectOfType<Canvas>();

                if (canvas)
                {
                    action.Invoke(canvas.transform);
                }
                else
                    action.Invoke(CreateCanvas());
            }
        }
        else
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas)
                action.Invoke(canvas.transform);
            else
                action.Invoke(CreateCanvas());
        }

        UnityEngine.EventSystems.EventSystem eventSystem = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if(eventSystem == null)
        {
            GameObject eventSystemObj = new GameObject();
            eventSystemObj.name = "Event System";
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    static UIPrefarencesEditor GetUIPrefarences()
    {
        string[] guid = AssetDatabase.FindAssets("t:UIPrefarencesEditor");
        return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid[0]), (typeof(UIPrefarencesEditor))) as UIPrefarencesEditor;
    }
}
