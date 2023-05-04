using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject playerGameObject;
    private CharacterController2D player;
    public GameObject QuickSlotUIobj;

    private Transform HealthBarUI;
    private Transform MoneyUI;
    private Transform QuickSlotUI;
    
    void Start()
    {
        player = playerGameObject.GetComponent<CharacterController2D>();
        HealthBarUI = transform.Find("HealthBarUI");
        MoneyUI = transform.Find("MoneyUI");
        QuickSlotUI = transform.Find("QuickSlotUI");
    }

    void Update()
    {
        HealthBarUI.Find("HealthBar").GetComponent<Image>().fillAmount = ((float)player.health / (float)player.maxHealth);
        HealthBarUI.Find("HealthBarText").GetComponent<TextMeshProUGUI>().SetText(player.health.ToString() + " / " + player.maxHealth.ToString());
        MoneyUI.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText("$ " + player.money.ToString());
        QuickSlotUI.Find("QuickSlotSprite").GetComponent<Image>().sprite = ShopItemList.GetSprite(player.quickSlotItem);
        QuickSlotUI.Find("QuickSlotText").GetComponent<TextMeshProUGUI>().SetText(player.quickSlotItemAmount.ToString());
        QuickSlotUIobj.SetActive(player.quickSlotItemAmount > 0);
    }
}
