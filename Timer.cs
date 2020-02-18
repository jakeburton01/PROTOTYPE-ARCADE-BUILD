using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{


    public Canvas canvas;
    public float roundTimerMins = 1f;
    float roundTimerSeconds = 0f;
    public Text timerText;
    public Text roundOverText;
    bool finalSeconds;
    bool timerStop;
    public GameObject manager;
    PowerupSpawn pwrups;
    bool spawnStarted;


    void Start()
    {
        roundTimerSeconds = 60f;
        roundOverText.enabled = false;
        finalSeconds = false;
        timerStop = false;
        pwrups = manager.GetComponent<PowerupSpawn>();
        spawnStarted = false;

    }

    // Update is called once per frame
    void Update()
    {

        if(roundTimerSeconds <= 40f)
        {
            if (spawnStarted == false)
            {
                pwrups.StartSpawning();
                spawnStarted = true;
            } else
            {

            }
        }


        if (timerStop)
        {
            roundTimerSeconds = 0;
        }
        else
        {
            roundTimerSeconds -= Time.deltaTime;
        } 

        timerText.text = string.Format("{0:00}:{1:00}", roundTimerMins, roundTimerSeconds);

        if (roundTimerMins >= 1f && roundTimerSeconds <= 0f)
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


        if(roundTimerMins <= 0f && roundTimerSeconds <= 0f)
        {
            timerStop = true;
            roundOverText.enabled = true;
        }

    }





}
