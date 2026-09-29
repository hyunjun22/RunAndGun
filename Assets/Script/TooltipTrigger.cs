using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TooltipUI tooltipUI;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Vector2 tooltipSize = new Vector2(200, 80);

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipUI.Show(description, tooltipSize);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipUI.Hide();
    }
}
