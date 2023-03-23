﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(ShopItemList.GetSprite(ShopItemList.ItemType.Armor_1), "Armor 1", ShopItemList.GetCost(ShopItemList.ItemType.Armor_1), 0);
        //    CreateItemButton(ShopItemList.GetSprite(ShopItemList.ItemType.Armor_2), "Armor 2", ShopItemList.GetCost(ShopItemList.ItemType.Armor_2), 1);

    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        //RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        //float shopItemHeight = 30f;
        //shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
    }
}
