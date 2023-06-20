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
    public Vector3 newPlayerSpawn;
    public GameObject removingWall;
    public bool enfOF = false; // end of file

    void Start()
    {
        pressx = transform.Find("PressX").gameObject;
        dialogue = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);
    }

    // dialogue[2 * page]   = character name
    // dialogue[2 * page+1] = character's line

    public void nextLine()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);

        // dialogue played for the first time, so disable bobbing 'x' image
        pressx.SetActive(false);

        // finished dialogue
        if(page*2 >= dialogue.Length)
        {
            enfOF = true;
            // remove wall, set new spawn point if exist.
            if (newPlayerSpawn != new Vector3(0, 0, 0)) player.spawnPoint = newPlayerSpawn;
            if (removingWall != null) removingWall.SetActive(false);

            conversationUIobj.SetActive(false);
            playerConversationUIobj.SetActive(false);
            player.inUI = false;
            return;
        }
        else player.inUI = true;

        // check if the line is player's or npc's
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

        // next line
        page++;
    }
}
