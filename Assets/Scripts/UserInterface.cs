/*
 * THIS SCRIPT IS STUPID AND USELESS
 * PLEASE DO NOT USE
 * (only not deleting it because of irrational fear of deleting code)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System;

public class UserInterface : MonoBehaviour
{
    private const float initialTime = 60f;
    private float timeRemaining = 0f;
    private GameObject playableCharacter;
    [SerializeField] private Text CountdownText;
    [SerializeField] private Text CarriedCollectable;
    [SerializeField] private Text PossibleActions;


    private void Start()
    {
        timeRemaining = initialTime;
        playableCharacter = GameObject.FindGameObjectsWithTag("GameController")[0];
    }


    private void Update()
    {
        updateCountdown();
        updateCarriedCollectable();
        updatePossibleActions();
    }


    private void updateCountdown()
    {
        // set timer to zero once time is up, else count down
        if (timeRemaining <= 0)
        {
            timeRemaining = 0f;
            // restart scene
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }

        CountdownText.text = timeRemaining.ToString("0.0");

        // COLOUR
        // this can definitely by optimised, but idc right now
        if (timeRemaining < 30)
            CountdownText.color = Color.yellow;
        if (timeRemaining < 20)
            CountdownText.color = new Color(1, 0.65f, 0);
        if (timeRemaining < 10)
            CountdownText.color = Color.red;
    }


    private void updateCarriedCollectable()
    {
        CarriedCollectable.text = "bla";//playableCharacter.GetComponent<Player>().carriedCollectable;
    }


    private void updatePossibleActions()
    {
        /*
         * TODO: only execute this Method when a Collectable is picked up or
         * hoarded/thrown/etc. Maybe via message sending?
         * I think the whole GetComponent stuff is quite costly.
         */

        var secondaryAction = "bla";//playableCharacter.GetComponent<Player>().secondaryAction;
        var tertiaryAction = "bla";//playableCharacter.GetComponent<Player>().tertiaryAction;

        if (secondaryAction == "" && tertiaryAction == "")
        {
            PossibleActions.text = "\n\n";
            return;
        }
        var outString = "E: throw\nR: " + secondaryAction + "\nT: " + tertiaryAction;
        PossibleActions.text = outString;
    }

}
