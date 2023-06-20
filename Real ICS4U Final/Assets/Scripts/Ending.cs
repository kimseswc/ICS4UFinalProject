using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private Conversation conv;
    public GameObject endingScene;

    void Start()
    {
        conv = transform.GetComponent<Conversation>();
        endingScene.SetActive(false);
    }

    // if player finish reading the very last dialogue
    // pops ending screen up
    void Update()
    {
        if(conv.enfOF == true)
        {
            endingScene.SetActive(true);
        }
    }
}
