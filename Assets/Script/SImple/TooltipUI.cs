using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] GameObject tooltipPanel;
    [SerializeField] Text tooltipText;

    RectTransform tooltipRect;
    private bool setPos = false;

    void Awake()
    {
        tooltipRect = tooltipPanel.GetComponent<RectTransform>();

        tooltipPanel.SetActive(false);
    }

    public void Show(string description, Vector2 size)
    {
        SetPosition();
        tooltipText.text = description;
        tooltipRect.sizeDelta = size;
        tooltipPanel.SetActive(true);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
        setPos = false;
    }

    void SetPosition()
    {
        if (setPos)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 currentOffset;

        if (mousePosition.y < Screen.height * 0.2f)
        {
            currentOffset = new Vector2(70f, 40f);
        }
        else
        {
            currentOffset = new Vector2(70f, -60f);
        }

        tooltipRect.position = mousePosition + currentOffset;

        setPos = true;
    }
}
