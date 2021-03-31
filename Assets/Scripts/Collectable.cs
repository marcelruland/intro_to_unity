/*
 * Collectables are objects the player may pick up and then perform an action
 * with.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // create a list of actions that can be performed with this collectabl
    public List<string> actions = new List<string>();

    private void Awake()
    {
        // all collectables can be thrown
        actions.Add("throw");

        /*
         * for every action, create an array of the TAGS of tha collectables
         * that the action can be performed with
         */
        string[] hordables = new string[] {
            "Disinfectant",
            "Flour",
            "ToiletRoll",
            "Yeast",
        };
        string[] drinkables = new string[]
        {
            "Disinfectant",
        };

        /*
         * Slightly complicated looking, but it's fast. Essentially means
         * "Is this object in the array?"
         * Check this for every action and if the object is in the array,
         * then it may perform this action.
         */
        if (Array.Exists(hordables, element => element == gameObject.tag))
        {
            actions.Add("hoard");
        }
        if (Array.Exists(drinkables, element => element == gameObject.tag))
        {
            actions.Add("drink");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
