using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{


    public Canvas canvas;
    float roundTimerMins = 1f;
    float roundTimerSeconds = 0f;
    public Text timerText;
    public Text roundOverText;
    bool finalSeconds;


    void Start()
    {
        roundTimerSeconds = 20f;
        roundTimerMins = 0f;
        roundOverText.enabled = false;
        finalSeconds = false;



    }

    // Update is called once per frame
    void Update()
    {
        roundTimerSeconds -= Time.deltaTime;
        timerText.text = string.Format("{0:00}:{1:00}", roundTimerMins, roundTimerSeconds);

        if (roundTimerMins > 1f && roundTimerSeconds < 0f)
        {
            roundTimerMins--;
            roundTimerSeconds = 59f;
            

            timerText.text = string.Format("{0:00}:{1:00}", roundTimerMins, roundTimerSeconds);
        }


        if (roundTimerMins < 1f && roundTimerSeconds < 15f)
        {
            timerText.color = Color.red;
            finalSeconds = true;
        }

        if(roundTimerMins < 1f && roundTimerSeconds < 10f)
        { 
            timerText.text = roundTimerSeconds.ToString("f0") + "!";
            timerText.fontSize = 70;
        }


        if(roundTimerMins < 0f && roundTimerSeconds < 0f)
        {
            roundOverText.enabled = true;
        }

    }





}
