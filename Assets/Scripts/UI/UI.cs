/*
 * UI countdown
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System;

public class UI : MonoBehaviour
{
    private const float initialTime = 60f;
    float timeRemaining = 0f;
    GameObject playableCharacter;
    [SerializeField] Text CountdownText;
    [SerializeField] Text CarriedCollectable;
    [SerializeField] Text PossibleActions;


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
        CarriedCollectable.text = playableCharacter.GetComponent<Player>().carriedCollectable;
    }


    private void updatePossibleActions()
    {
        // bla
    }

}
