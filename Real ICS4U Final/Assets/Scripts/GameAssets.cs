using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();

            return _i;
        }
        
    }

    public Sprite Armor_1;
    public Sprite Armor_2;
    public Sprite Armor_3;
    public Sprite Sword_1;
    public Sprite Sword_2;
    public Sprite Sword_3;
    public Sprite Potion_1;
    public Sprite Potion_2;
    public Sprite Potion_3;
    public Sprite bulbON;
    public Sprite bulbOFF;
}

