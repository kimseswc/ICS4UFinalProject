using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ShopUI;
    public GameObject ShopTarget;
    private UI_Shop SIL;

    // Start is called before the first frame update
    void Start()
    {
        SIL = ShopUI.GetComponent<UI_Shop>();
    }

    public void OpenShop()
    {
        SIL.OnOffUI(ShopTarget);
        Debug.Log("Opened Shop");
    }
}
