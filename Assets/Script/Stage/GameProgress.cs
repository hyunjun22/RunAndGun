using System.Collections.Generic;
using UnityEngine;

public static class GameProgress
{
    public static int Gold { get; private set; }

    private static Dictionary<string, int> upgradeLevels = new();

    public static void AddGold(int amount)
    {
        if (amount < 0)
            return;

        Gold += amount;
        Debug.Log($"°ñµå È¹µæ: {amount}G / º¸À¯: {Gold}G");
    }

    public static int GetUpgradeLevel(string id)
    {
        return upgradeLevels.TryGetValue(id, out int level) ? level : 0;
    }

    public static bool BuyUpgrade(string id, int cost)
    {
        if (string.IsNullOrEmpty(id) || cost < 0 || Gold < cost)
            return false;

        Gold -= cost;
        upgradeLevels[id] = GetUpgradeLevel(id) + 1;

        return true;
    }
}
