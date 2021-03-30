using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private const float initialTime = 60f;
    float timeRemaining = 0f;
    [SerializeField] Text CountdownText;


    private void Start()
    {
        timeRemaining = initialTime;
    }


    private void Update()
    {
        // set timer to zero once time is up, else count down
        if (timeRemaining <= 0)
            timeRemaining = 0f;
        else
            timeRemaining -= Time.deltaTime;

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
}
