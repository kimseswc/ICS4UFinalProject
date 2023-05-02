/*
 * https://en.wikipedia.org/wiki/Light_Up_(puzzle)
 * This script must be on every bulb on Light Up puzzle
 * each bulbs that are in the same puzzle should have same GameObject wall
 * each bulbs that are in the same puzzle should have it's upper side, lower side, right side and left side bulb GameObject 
 * if the bulb is at the edge of the puzzle, the GameObjects can be null objects
 * each bulbs can be modified by changing the status. bulb is turned on if the status is ture
 * if all bulbs are turned on, the wall will disapear and the bulbs cannot be used anymore.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public bool status = false;
    public GameObject wall;
    public GameObject upObj;
    public GameObject downObj;
    public GameObject leftObj;
    public GameObject rightObj;

    private bool canBeUsed = true;

    void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = (status == true ? GameAssets.i.bulbON : GameAssets.i.bulbOFF);
    }

    public void useSwitch()
    {
        status ^= true;
        transform.GetComponent<SpriteRenderer>().sprite = (status == true ? GameAssets.i.bulbON : GameAssets.i.bulbOFF);
    }

    public void interactSwitch()
    {
        if(!canBeUsed) return;

        // use this, up, down, left, right light switches
        useSwitch();
        if(upObj != null)       upObj.GetComponent<Puzzle>().useSwitch();
        if(downObj != null)   downObj.GetComponent<Puzzle>().useSwitch();
        if(leftObj != null)   leftObj.GetComponent<Puzzle>().useSwitch();
        if(rightObj != null) rightObj.GetComponent<Puzzle>().useSwitch();

        if(checkFinished(true, true, true, true))
        {
            stopUsage(true, true, true, true);
            wall.SetActive(false);
        }
    }

    // access connected lights and check's them off. this function recursively checks all connected lights
    public bool checkFinished(bool canGoUp, bool canGoDown, bool canGoLeft, bool canGoRight)
    {
        // if this light is off, no need to check all lights, so return false
        if(status == false) return false;

        bool b1, b2, b3, b4;

        b1 = (canGoUp    ? (upObj    == null ? true :    upObj.GetComponent<Puzzle>().checkFinished(true,  false, true,  true))  : true);
        b2 = (canGoDown  ? (downObj  == null ? true :  downObj.GetComponent<Puzzle>().checkFinished(false, true,  true,  true))  : true);
        b3 = (canGoLeft  ? (leftObj  == null ? true :  leftObj.GetComponent<Puzzle>().checkFinished(false, false, true,  false)) : true);
        b4 = (canGoRight ? (rightObj == null ? true : rightObj.GetComponent<Puzzle>().checkFinished(false, false, false, true))  : true);

        // only if all the connected branches' lights are all turned on, it will return true
        return (b1 & b2 & b3 & b4);
    }

    // similar to checkFinished(). recursively disables player interaction of all connected lights
    public void stopUsage(bool canGoUp, bool canGoDown, bool canGoLeft, bool canGoRight)
    {
        canBeUsed = false;
        if(canGoUp    && upObj    != null)    upObj.GetComponent<Puzzle>().stopUsage(true,  false, true,  true);
        if(canGoDown  && downObj  != null)  downObj.GetComponent<Puzzle>().stopUsage(false, true,  true,  true);
        if(canGoLeft  && leftObj  != null)  leftObj.GetComponent<Puzzle>().stopUsage(false, false, true,  false);
        if(canGoRight && rightObj != null) rightObj.GetComponent<Puzzle>().stopUsage(false, false, false, true);
    }
}
