using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private const float InitialTime = 60f;
    float TimeRemaining = 0f;
    [SerializeField] Text CountdownText;


    private void Start()
    {
        TimeRemaining = InitialTime;
    }


    private void Update()
    {
        // set timer to zero once time is up, else count down
        if (TimeRemaining <= 0)
            TimeRemaining = 0f;
        else
            // maybe we can increase the 1 at some point to really really mess with people
            TimeRemaining -= 1 * Time.deltaTime;

        CountdownText.text = TimeRemaining.ToString("0.0");

        // colour
        // this can definitely by optimsed, but idc right now
        if (TimeRemaining < 30)
            CountdownText.color = Color.yellow;
        if (TimeRemaining < 20)
            CountdownText.color = new Color(1, 0.65f, 0);
        if (TimeRemaining < 10)
            CountdownText.color = Color.red;
    }
}
