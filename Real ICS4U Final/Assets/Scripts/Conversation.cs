using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour
{
    public CharacterController2D player;
    public string characterName;
    public TextAsset textAsset;
    private string[] dialogue;
    private int page = 0;
    public Transform conversationUI;
    public GameObject conversationUIobj;
    public Transform playerConversationUI;
    public GameObject playerConversationUIobj;
    private GameObject pressx;
    public AudioSource soundEffectPlayer;

    // Start is called before the first frame update
    void Start()
    {
        pressx = transform.Find("PressX").gameObject;
        dialogue = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);
    }

    public void nextLine()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        pressx.SetActive(false);
        if(page*2 >= dialogue.Length)
        {
            conversationUIobj.SetActive(false);
            playerConversationUIobj.SetActive(false);
            player.inUI = false;
            return;
        }
        else player.inUI = true;
        if(dialogue[page*2] == characterName)
        {
            conversationUIobj.SetActive(true);
            playerConversationUIobj.SetActive(false);
            conversationUI.Find("ConversationName").GetComponent<TextMeshProUGUI>().SetText(dialogue[page*2]);
            conversationUI.Find("ConversationText").GetComponent<TextMeshProUGUI>().SetText(dialogue[page*2+1]);
        }
        else
        {
            conversationUIobj.SetActive(false);
            playerConversationUIobj.SetActive(true);
            playerConversationUI.Find("ConversationName").GetComponent<TextMeshProUGUI>().SetText(dialogue[page * 2]);
            playerConversationUI.Find("ConversationText").GetComponent<TextMeshProUGUI>().SetText(dialogue[page * 2 + 1]);
        }

        page++;
    }

    
}
