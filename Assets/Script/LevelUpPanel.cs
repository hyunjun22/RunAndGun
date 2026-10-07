using UnityEngine;
using UnityEngine.UI;
using System;

// LevelUpPanel에 하나씩 붙여줄 스크립트
public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] RawImage icon;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Button selectButton;
    

    public void Setup(UpgradeData data, Action<UpgradeData> onSelect)
    {
        icon.texture = data.icon;
        nameText.text = data.upgradeName;
        descriptionText.text = data.description;
        descriptionText.text = data.description.Replace("{Value}", data.value.ToString());

        selectButton.onClick.RemoveAllListeners();

        selectButton.onClick.AddListener(() =>
        {
            onSelect(data);
        });
    }
}
