using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    // Start is called before the first frame update

    
    public Canvas canvas;
    float roundTimerMins = 1f;
    float roundTimerSeconds = 0f;
    public TextMeshProUGUI timerText;


    void Start()
    {

        timerText = GetComponent<TextMeshProUGUI>();

        roundTimerSeconds = 20f;
        roundTimerMins--;

        

    }

    // Update is called once per frame
    void Update()
    {
        roundTimerSeconds -= Time.deltaTime;
        timerText.text = "00:" + roundTimerSeconds;
        //string.Format("{0:00}:{1:00}", roundTimerMins, roundTimerSeconds);

        if (roundTimerSeconds < 0f)
        {
            
            roundTimerSeconds = 20f;
           // roundTimerMins--;

            
            timerText.text = string.Format("{0:00}:{1:00}", roundTimerMins, roundTimerSeconds);
        }


        if(roundTimerMins < 1 && roundTimerSeconds < 15)
        {
            timerText.color = Color.red;
        }
    }
}
