using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int goldReward;
    public ShopItemList.ItemType rewardItem;
    public CharacterController2D player;

    private bool isOpen = false;
    private Animator rewardAnimator;

    void Start()
    {
        transform.Find("ChestReward").GetComponent<SpriteRenderer>().sprite = ShopItemList.GetSprite(rewardItem);
        rewardAnimator = transform.Find("ChestReward").GetComponent<Animator>();
    }

    public void interactChest()
    {
        if (isOpen) tryTakeItem();
        else StartCoroutine(openChest());
    }

    IEnumerator openChest()
    {
        rewardAnimator.SetTrigger("ChestOpen");
        yield return new WaitForSeconds(0.1f);
        isOpen = true;
    }

    private bool canTakeItem()
    {
        if(rewardItem == ShopItemList.ItemType.Coin)
        {
            player.money += goldReward;
            return true;
        }

        if (rewardItem == ShopItemList.ItemType.Potion_1 || rewardItem == ShopItemList.ItemType.Potion_2 || rewardItem == ShopItemList.ItemType.Potion_3)
        {
            if (rewardItem != player.quickSlotItem)
            {
                if (player.quickSlotItemAmount != 0) return false;
            }
        }


        if (rewardItem == ShopItemList.ItemType.Armor_1 || rewardItem == ShopItemList.ItemType.Armor_2 || rewardItem == ShopItemList.ItemType.Armor_3)
        {
            player.AddMaxHealth(ShopItemList.GetEffect(rewardItem));
        }

        else if (rewardItem == ShopItemList.ItemType.Sword_1 || rewardItem == ShopItemList.ItemType.Sword_2 || rewardItem == ShopItemList.ItemType.Sword_3)
        {
            player.AddDamage(ShopItemList.GetEffect(rewardItem));
        }

        else 
        {
            player.AddQuickSlotItem(rewardItem, ShopItemList.GetEffect(rewardItem));
        }

        return true;
    }

    private void tryTakeItem()
    {
        if(canTakeItem()) Destroy(gameObject);
    }
}
