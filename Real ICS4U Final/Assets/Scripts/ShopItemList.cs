using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemList: MonoBehaviour
{
    public enum ItemType
    {
        Armor_1,
        Armor_2
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Armor_1: return 10;
            case ItemType.Armor_2: return 30;
        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
        default:
        case ItemType.Armor_1: return GameAssets.i.Armor_1;
        case ItemType.Armor_2: return GameAssets.i.Armor_2;
        }
    }
}