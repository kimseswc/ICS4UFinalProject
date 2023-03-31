using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private bool UIVisability = false;
    public Collider2D playerCL;
    private CharacterController2D player;
    private Transform container;
    private Transform armorShop;
    private Transform swordShop;
    private Transform potionShop;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        armorShop = transform.Find("armorShop");
        swordShop = transform.Find("swordShop");
        potionShop = transform.Find("potionShop");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        player = playerCL.GetComponent<CharacterController2D>();
        CreateItemButton(ShopItemList.ItemType.Armor_1, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_1), "Armor 1", ShopItemList.GetCost(ShopItemList.ItemType.Armor_1), 0, armorShop);
        CreateItemButton(ShopItemList.ItemType.Armor_2, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_2), "Armor 2", ShopItemList.GetCost(ShopItemList.ItemType.Armor_2), 1, armorShop);
        CreateItemButton(ShopItemList.ItemType.Armor_3, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_3), "Armor 3", ShopItemList.GetCost(ShopItemList.ItemType.Armor_3), 2, armorShop);
        CreateItemButton(ShopItemList.ItemType.Sword_1, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_1), "Sword 1", ShopItemList.GetCost(ShopItemList.ItemType.Sword_1), 0, swordShop);
        CreateItemButton(ShopItemList.ItemType.Sword_2, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_2), "Sword 2", ShopItemList.GetCost(ShopItemList.ItemType.Sword_2), 1, swordShop);
        CreateItemButton(ShopItemList.ItemType.Sword_3, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_3), "Sword 3", ShopItemList.GetCost(ShopItemList.ItemType.Sword_3), 2, swordShop);
        CreateItemButton(ShopItemList.ItemType.Potion_1, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_1), "Potion 1", ShopItemList.GetCost(ShopItemList.ItemType.Potion_1), 0, potionShop);
        CreateItemButton(ShopItemList.ItemType.Potion_2, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_2), "Potion 2", ShopItemList.GetCost(ShopItemList.ItemType.Potion_2), 1, potionShop);
        CreateItemButton(ShopItemList.ItemType.Potion_3, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_3), "Potion 3", ShopItemList.GetCost(ShopItemList.ItemType.Potion_3), 2, potionShop);
    }

    private void CreateItemButton(ShopItemList.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex, Transform parentContainer)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, parentContainer);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        float shopItemHeight = 120;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.gameObject.SetActive(true);

        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType, itemCost); });

    }

    private void TryBuyItem(ShopItemList.ItemType itemType, int itemCost)
    {

        if (player.TrySpend(itemCost))
        {
            player.AddHealth(ShopItemList.GetEffect(itemType));
        }
    }

    public void OnOffUI(GameObject i)
    {

        i.SetActive(UIVisability ^= true);
    }
}
