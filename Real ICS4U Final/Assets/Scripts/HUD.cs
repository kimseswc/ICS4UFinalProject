using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject player;
    private CharacterController2D script;
    public GameObject QuickSlotUIobj;

    private Transform HealthBarUI;
    private Transform MoneyUI;
    private Transform QuickSlotUI;
    

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<CharacterController2D>();
        HealthBarUI = transform.Find("HealthBarUI");
        MoneyUI = transform.Find("MoneyUI");
        QuickSlotUI = transform.Find("QuickSlotUI");
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarUI.Find("HealthBar").GetComponent<Image>().fillAmount = ((float)script.health / (float)script.maxHealth);
        HealthBarUI.Find("HealthBarText").GetComponent<TextMeshProUGUI>().SetText(script.health.ToString() + " / " + script.maxHealth.ToString());
        MoneyUI.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText("$ " + script.money.ToString());
        QuickSlotUI.Find("QuickSlotSprite").GetComponent<Image>().sprite = ShopItemList.GetSprite(script.quickSlotItem);
        QuickSlotUI.Find("QuickSlotText").GetComponent<TextMeshProUGUI>().SetText(script.quickSlotItemAmount.ToString());
        QuickSlotUIobj.SetActive(script.quickSlotItemAmount > 0);

    }
}
