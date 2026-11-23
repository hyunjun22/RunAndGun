using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GoldTestPanel : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    GameObject panel;

    void Update()
    {
        if (Keyboard.current?.f12Key.wasPressedThisFrame != true)
            return;

        if (panel == null)
        {
            CreatePanel();
            return;
        }

        panel.SetActive(!panel.activeSelf);
    }

    void CreatePanel()
    {
        panel = new GameObject("GoldTestPanel", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        var panelRect = (RectTransform)panel.transform;
        panelRect.SetParent(transform, false);
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -16f);
        panelRect.sizeDelta = new Vector2(180f, 64f);
        panel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.12f, 0.12f, 0.16f);

        var buttonObject = new GameObject("AddGoldButton", typeof(RectTransform),
            typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
        var buttonRect = (RectTransform)buttonObject.transform;
        buttonRect.SetParent(panelRect, false);
        buttonRect.sizeDelta = new Vector2(156f, 40f);
        var button = buttonObject.GetComponent<UnityEngine.UI.Button>();
        button.targetGraphic = buttonObject.GetComponent<UnityEngine.UI.Image>();
        button.onClick.AddListener(() => GameProgress.AddGold(100));

        var labelObject = new GameObject("Label", typeof(RectTransform), typeof(UnityEngine.UI.Text));
        var labelRect = (RectTransform)labelObject.transform;
        labelRect.SetParent(buttonRect, false);
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.sizeDelta = Vector2.zero;
        var label = labelObject.GetComponent<UnityEngine.UI.Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = "+100 G";
        label.fontSize = 20;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Color.black;
        label.raycastTarget = false;
    }
#endif
}
