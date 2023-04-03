using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    private int money;
    private int health;
    private int maxHealth;
    public GameObject player;
    private CharacterController2D script;

    private Transform HealthBarUI;
    private Transform MoneyUI;
    

    // Start is called before the first frame update
    void Start()
    {
        script = player.GetComponent<CharacterController2D>();
        HealthBarUI = transform.Find("HealthBarUI");
        MoneyUI = transform.Find("MoneyUI");
    }

    // Update is called once per frame
    void Update()
    {
        money = script.money;
        health = script.health;
        maxHealth = script.maxHealth;
        HealthBarUI.Find("HealthBar").GetComponent<Image>().fillAmount = ((float)health / (float)maxHealth);
        HealthBarUI.Find("HealthBarText").GetComponent<TextMeshProUGUI>().SetText(health.ToString() + " / " + maxHealth.ToString());
        MoneyUI.Find("MoneyText").GetComponent<TextMeshProUGUI>().SetText("$ " + money.ToString());
    }
}
