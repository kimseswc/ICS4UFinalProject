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
    public AudioSource soundEffectPlayer;

    private void Awake()
    {
        container = transform.Find("container");
        armorShop = transform.Find("armorShop");
        swordShop = transform.Find("swordShop");
        potionShop = transform.Find("potionShop");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    // create Armor, Sword, Potion buttons on shopUI
    private void Start()
    {
        player = playerCL.GetComponent<CharacterController2D>();

        CreateItemButton(ShopItemList.ItemType.Armor_1, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_1), "+20 max health", ShopItemList.GetCost(ShopItemList.ItemType.Armor_1), 0, armorShop);
        CreateItemButton(ShopItemList.ItemType.Armor_2, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_2), "+45 max health", ShopItemList.GetCost(ShopItemList.ItemType.Armor_2), 1, armorShop);
        CreateItemButton(ShopItemList.ItemType.Armor_3, ShopItemList.GetSprite(ShopItemList.ItemType.Armor_3), "+70 max health", ShopItemList.GetCost(ShopItemList.ItemType.Armor_3), 2, armorShop);

        CreateItemButton(ShopItemList.ItemType.Sword_1, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_1), "+20 attack damage", ShopItemList.GetCost(ShopItemList.ItemType.Sword_1), 0, swordShop);
        CreateItemButton(ShopItemList.ItemType.Sword_2, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_2), "+45 attack damage", ShopItemList.GetCost(ShopItemList.ItemType.Sword_2), 1, swordShop);
        CreateItemButton(ShopItemList.ItemType.Sword_3, ShopItemList.GetSprite(ShopItemList.ItemType.Sword_3), "+70 attack damage", ShopItemList.GetCost(ShopItemList.ItemType.Sword_3), 2, swordShop);

        CreateItemButton(ShopItemList.ItemType.Potion_1, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_1), "+20 HP", ShopItemList.GetCost(ShopItemList.ItemType.Potion_1), 0, potionShop);
        CreateItemButton(ShopItemList.ItemType.Potion_2, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_2), "+45 HP", ShopItemList.GetCost(ShopItemList.ItemType.Potion_2), 1, potionShop);
        CreateItemButton(ShopItemList.ItemType.Potion_3, ShopItemList.GetSprite(ShopItemList.ItemType.Potion_3), "+70 HP", ShopItemList.GetCost(ShopItemList.ItemType.Potion_3), 2, potionShop);
    }

    // sets button's image, text, positions
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

        // when this button is clicked -> TryBuyItem(itemName)
        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItem(itemType, itemCost); });
    }

    private void TryBuyItem(ShopItemList.ItemType i, int itemCost)
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);

        // if it is not possible to replace the quickslot, it will not try to buy potion
        if (i == ShopItemList.ItemType.Potion_1 || i == ShopItemList.ItemType.Potion_2 || i == ShopItemList.ItemType.Potion_3)
        {
            if(i != player.quickSlotItem)
            {
                if(player.quickSlotItemAmount != 0) return;
            }
        }

        // if player can buy item
        if (player.TrySpend(itemCost))
        {
            // max health
            if(i == ShopItemList.ItemType.Armor_1 || i == ShopItemList.ItemType.Armor_2 || i == ShopItemList.ItemType.Armor_3)
            {
                player.AddMaxHealth(ShopItemList.GetEffect(i));
            }
            // damage
            else if(i == ShopItemList.ItemType.Sword_1 || i == ShopItemList.ItemType.Sword_2 || i == ShopItemList.ItemType.Sword_3)
            {
                player.AddDamage(ShopItemList.GetEffect(i));
            }
            // potion
            else
            {
                player.AddQuickSlotItem(i, ShopItemList.GetEffect(i));
            }
        }
    }

    // XOR switch to turn the UI on and OFF
    public void OnOffUI(GameObject i)
    {
        i.SetActive(UIVisability ^= true);
    }
}
