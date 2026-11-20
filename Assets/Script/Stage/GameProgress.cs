using System.Collections.Generic;
using UnityEngine;

public static class GameProgress
{
    public static int Gold { get; private set; }
    public static event System.Action<int> GoldChanged;

    private static Dictionary<string, int> upgradeLevels = new();
    private static readonly Dictionary<UpgradeType, float> statBonuses = new();

    public static float GetStatBonus(UpgradeType type)
    {
        return statBonuses.TryGetValue(type, out float bonus) ? bonus : 0f;
    }

    public static void AddGold(int amount)
    {
        if (amount < 0)
            return;

        Gold += amount;
        GoldChanged?.Invoke(Gold);
        Debug.Log($"추가 금액: {amount}G / 현재 잔액: {Gold}G");
    }

    public static int GetUpgradeLevel(string id)
    {
        return upgradeLevels.TryGetValue(id, out int level) ? level : 0;
    }

    public static bool BuyUpgrade(string id, int cost)
    {
        return TryBuyUpgrade(id, cost, null, 0f);
    }

    public static bool BuyUpgrade(StoreUpgradeData data)
    {
        return data != null && data.IsValid &&
            TryBuyUpgrade(data.Id, data.Cost, data.Type, data.IncreasePerLevel);
    }

    static bool TryBuyUpgrade(string id, int cost, UpgradeType? type, float increase)
    {
        if (string.IsNullOrEmpty(id) || cost < 0 || Gold < cost)
            return false;

        Gold -= cost;
        upgradeLevels[id] = GetUpgradeLevel(id) + 1;
        if (type.HasValue) statBonuses[type.Value] = GetStatBonus(type.Value) + increase;
        GoldChanged?.Invoke(Gold);

        return true;
    }
}
