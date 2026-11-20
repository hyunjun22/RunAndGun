using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class StoreUpgradeButton : MonoBehaviour
{
    [SerializeField] StoreUpgradeData upgradeData;

    void OnEnable() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ShowDescription); }
    void OnDisable() { GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(ShowDescription); }

    void ShowDescription()
    {
        var store = GetComponentInParent<StoreUI>();
        if (store != null) store.ShowUpgrade(upgradeData);
    }
}
