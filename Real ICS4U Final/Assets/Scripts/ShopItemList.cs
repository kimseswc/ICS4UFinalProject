using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemList: MonoBehaviour
{
    public enum ItemType
    {
        Armor_1,
        Armor_2,
        Armor_3,
        Sword_1,
        Sword_2,
        Sword_3,
        Potion_1,
        Potion_2,
        Potion_3,
        Coin
    }

    public static int GetEffect(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Armor_1: return 20;
            case ItemType.Armor_2: return 45;
            case ItemType.Armor_3: return 70;
            case ItemType.Sword_1: return 20;
            case ItemType.Sword_2: return 45;
            case ItemType.Sword_3: return 70;
            case ItemType.Potion_1: return 20;
            case ItemType.Potion_2: return 45;
            case ItemType.Potion_3: return 70;
            case ItemType.Coin: return 0;
        }
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Armor_1: return 40;
            case ItemType.Armor_2: return 80;
            case ItemType.Armor_3: return 120;
            case ItemType.Sword_1: return 40;
            case ItemType.Sword_2: return 80;
            case ItemType.Sword_3: return 120;
            case ItemType.Potion_1: return 20;
            case ItemType.Potion_2: return 35;
            case ItemType.Potion_3: return 50;
            case ItemType.Coin: return 0;
        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
        default:
        case ItemType.Armor_1: return GameAssets.i.Armor_1;
        case ItemType.Armor_2: return GameAssets.i.Armor_2;
        case ItemType.Armor_3: return GameAssets.i.Armor_3;
        case ItemType.Sword_1: return GameAssets.i.Sword_1;
        case ItemType.Sword_2: return GameAssets.i.Sword_2;
        case ItemType.Sword_3: return GameAssets.i.Sword_3;
        case ItemType.Potion_1: return GameAssets.i.Potion_1;
        case ItemType.Potion_2: return GameAssets.i.Potion_2;
        case ItemType.Potion_3: return GameAssets.i.Potion_3;
        case ItemType.Coin: return GameAssets.i.Coin;

        }
    }
}