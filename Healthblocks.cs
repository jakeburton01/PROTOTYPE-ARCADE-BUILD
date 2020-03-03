﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthblocks : MonoBehaviour
{
    public List<RawImage> segments = new List<RawImage>();
    public RawImage healthPrefab, damagedHealthPrefab;
    public Color defaultColor;
    public bool blockFlashing;
    public bool neutral;
    public bool speedPowerup;
    public bool hitByDamagePowerup;
    public float regenTimer;
    int i;
    public float chargedTimer;
    public Text gameOverText;
    public Vector3[] positions;
    float originalSpeed;
    public float powerupNewSpeed;
    public float powerupLength;
    RawImage FlashingBlock;
    RawImage SelectedBlock;
    //array of positions that looks at how many blocks of health left through segment list 
    //damage powerup boolean turns true the player is given an increased damage punch which can only be used once and will have to be the next attack 
    public bool dizzyIsPlaying;
    public bool dizzyState = false;
    public float idleTimer = 5f;
    private CharacterAnimation _anim;
    private JoystickMovement _movement;


    void Start()
    {
        positions = new Vector3[6];
        i = segments.Count - 1;
        blockFlashing = false;
        regenTimer = 0f;
        gameOverText.enabled = false;
        neutral = true;
        positions[0] = segments[0].transform.position;
        positions[1] = segments[1].transform.position;
        positions[2] = segments[2].transform.position;
        positions[3] = segments[3].transform.position;
        positions[4] = segments[4].transform.position;
        positions[5] = segments[5].transform.position;
        segments.RemoveAt(i);
        FlashingBlock = segments[4];
        hitByDamagePowerup = false;
        speedPowerup = false;
        _movement = GetComponent<JoystickMovement>();
    }

    void Awake()
    {
        _anim = GetComponentInChildren<CharacterAnimation>();
    }


    //if just pressed then normal hit - make one segment flash and then if hit again destroy flashing bar
    //if button held for 2 seconds remove 1 bar straight away 
    //if button held for 3 seconds remove 1 bar and make next bar flash 
    //if button held for 4 or more seconds then remove two bars straight away 

    void Update()
    {
        Knockout();
        i = segments.Count - 1;
        regenTimer += Time.deltaTime;
        //print(FlashingBlock);

        if (blockFlashing)
        {
            FlashingBlock = segments[i];
        }

        //if healthbar is empty then bring up game over text 
        if (segments.Count == 0)
        {
            //GameOver();
            dizzyState = true;
        }

        if (regenTimer >= 11f)
        {
            regenTimer = 0f;
        }

        //if 5 seconds have passed since health block began flashing then stop health block flashing(regenerating that health block)
        if (regenTimer >= 10f)
        {
            HealthRegen();
        }


        //ignore from HERE 
        if (Input.GetKeyDown("h"))
        {
            HealthPickup();

        }
        //n key == normal attack - everytime n key pushed down counter increases by 1
        if (Input.GetKeyDown("n"))
        {
            NormalDamage();
        }

        //c will represent the charged attack - when pressed the chargedtimer will start  
        if (Input.GetKey("c"))
        {
            ChargedTimer();
        }

        //when the c key is lifted up depending on the length of the charged timer health blocks will be removed or flash  
        if (Input.GetKeyUp("c"))
        {
            ChargedDamage();
        }

        if (Input.GetKeyDown("d"))
        {
            DamagePickup();
        }
        //to HERE just here for testing purposes
    }



    //coroutine to make the currently selected block flash 
    public IEnumerator Flashingbar()
    {
        //while blockflashing is true the last health block in the array will be enabled and disabled to give the illusion of it flashing
        while (blockFlashing)
        {
            FlashingBlock.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
            FlashingBlock.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            FlashingBlock.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
            FlashingBlock.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
        yield return FlashingBlock.color = defaultColor;
    }

    //brings up game over text
    public void GameOver()
    {
        gameOverText.enabled = true;
    }


    //basic take damage function 
    public void NormalDamage()
    {
        regenTimer = 0f;
        StopAllCoroutines();

        if (neutral)
        {
            //coroutine that makes block flash begins and sets the blockflashing boolean to be true 
            blockFlashing = true;
            StartCoroutine(Flashingbar());
            neutral = false;
            _anim.BeenHit();
        }
        else
        {
            //couroutine stopped so block stops flashing - meaning the blockfalshing boolean is now false - and the block that was flashing gets destroyed 
            blockFlashing = false;
            StopCoroutine(Flashingbar());
            neutral = true;
            SelectedBlock = segments[i];
            segments.RemoveAt(i);
            SelectedBlock.enabled = false;
            i = segments.Count - 1;
            FlashingBlock = segments[i];
            _anim.BeenHit();
        }
    }

    //function to start the charged timer to determine how long the attack has been charged for 
    public void ChargedTimer()
    {
        chargedTimer += Time.deltaTime;
    }

    //fucntion for the damage from the charged attack 
    public void ChargedDamage()
    {
        //timer is between 2 and 3 seconds so destroys 1 block of health 
        if (chargedTimer >= 2f && chargedTimer < 3f)
        {
            //if no blocks are flashing then destroy one health block 
            if (neutral)
            {
                segments[i].enabled = false;
                segments.RemoveAt(i);
                chargedTimer = 0f;

            }
            else
            //if a block is flashing then destroy the flashing block and make the next block flash 
            {
                SelectedBlock = segments[i];
                segments.RemoveAt(i);
                SelectedBlock.enabled = false;
                blockFlashing = true;
                StartCoroutine(Flashingbar());
                chargedTimer = 0f;
                regenTimer = 0f;
                neutral = false;
            }


        }

        //timer is between 3 and 4 seconds so destroy 1 1/2 health blocks  
        if (chargedTimer >= 3f && chargedTimer < 4f)
        {
            //if no blocks are flashing then remove one block and make the next health block flash 
            if (neutral)
            {
                SelectedBlock = segments[i];
                segments.RemoveAt(i);
                SelectedBlock.enabled = false;
                blockFlashing = true;
                StartCoroutine(Flashingbar());
                chargedTimer = 0f;
                regenTimer = 0f;
                neutral = false;
            }
            else
            //if a block is flashing then destroy the flashing block as well as one health block 
            {
                SelectedBlock = segments[i];
                segments.RemoveAt(i);
                SelectedBlock.enabled = false;
                int e = segments.Count - 1;
                RawImage SelectedBlock2 = segments[e];
                segments.RemoveAt(e);
                SelectedBlock2.enabled = false;
                chargedTimer = 0f;
                neutral = true;
            }

        }

        //timer is more than 4 seconds so the maximum amount of damage will be dealt meaning 2 health blocks will be removed 
        if (chargedTimer >= 4f)
        {
            //if no blocks are flashing then remove two health blocks 
            if (neutral)
            {
                SelectedBlock = segments[i];
                segments.RemoveAt(i);
                SelectedBlock.enabled = false;
                int e = segments.Count - 1;
                RawImage SelectedBlock2 = segments[e];
                segments.RemoveAt(e);
                SelectedBlock2.enabled = false; ;
                chargedTimer = 0f;
                neutral = true;
            }
            else
            //if a block is flashing then remove the flashing block and a health block and then make a health block flash 
            {
                SelectedBlock = segments[i];
                segments.RemoveAt(i);
                SelectedBlock.enabled = false;
                int e = segments.Count - 1;
                RawImage SelectedBlock2 = segments[e];
                segments.RemoveAt(e);
                SelectedBlock2.enabled = false;
                blockFlashing = true;
                StartCoroutine(Flashingbar());
                chargedTimer = 0f;
                regenTimer = 0f;
                neutral = false;
            }

        }
    }

    //function to stop block flashing essentially regenerating the players health
    public void HealthRegen()
    {
        if (blockFlashing == true)
        {
            blockFlashing = false;
            StopCoroutine(Flashingbar());
            neutral = true;
        }
    }

    // coroutine to determine the length the speed powerup lasts for 
    IEnumerator PowerUpTimer()
    {
        //current speed of player variable = powerupNewSpeed
        yield return new WaitForSeconds(powerupLength);
        StopCoroutine(PowerUpTimer());

        yield return speedPowerup = false;

    }

    // function called when the player picks up the speed powerup 
    void SpeedPickup()
    {
        //originalSpeed = current speed of player variable
        speedPowerup = true;
        StartCoroutine(PowerUpTimer());
    }

    // function called when the player picks up the damage powerup 
    void DamagePickup()
    {
        hitByDamagePowerup = true;
    }

    // function called when the player picks up the health powerup 
    void HealthPickup()
    {

        if (segments.Count < 6)
        {
            if (neutral)
            {
                var newHealth1 = (RawImage)Instantiate(healthPrefab) as RawImage;
                newHealth1.transform.SetParent(transform, false);
                newHealth1.enabled = true;
                segments.Add(newHealth1);
                i = segments.Count - 1;
                FlashingBlock = segments[i];


                if (segments.Count == 6)
                {
                    newHealth1.transform.position = positions[5];


                }

                if (segments.Count == 5)
                {
                    newHealth1.transform.position = positions[4];

                }

                if (segments.Count == 4)
                {
                    newHealth1.transform.position = positions[3];

                }

                if (segments.Count == 3)
                {
                    newHealth1.transform.position = positions[2];

                }

                if (segments.Count == 2)
                {
                    newHealth1.transform.position = positions[1];

                }

                if (segments.Count == 1)
                {
                    newHealth1.transform.position = positions[0];

                }
            }
            else
            {
                blockFlashing = false;
                StopCoroutine(Flashingbar());
                neutral = true;
                var newHealth = (RawImage)Instantiate(damagedHealthPrefab) as RawImage;
                newHealth.transform.SetParent(transform, false);
                newHealth.enabled = true;
                segments.Add(newHealth);
                i = segments.Count - 1;

                if (segments.Count == 6)
                {
                    newHealth.transform.position = positions[5];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;

                }

                if (segments.Count == 5)
                {
                    newHealth.transform.position = positions[4];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;
                }

                if (segments.Count == 4)
                {
                    newHealth.transform.position = positions[3];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;
                }

                if (segments.Count == 3)
                {
                    newHealth.transform.position = positions[2];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;
                }

                if (segments.Count == 2)
                {
                    newHealth.transform.position = positions[1];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;
                }

                if (segments.Count == 1)
                {
                    newHealth.transform.position = positions[0];
                    regenTimer = 0f;
                    StartCoroutine(Flashingbar());
                    blockFlashing = true;
                    neutral = false;
                }
            }

        }

    }


    public void Knockout()
    {
        if (dizzyState == true && idleTimer > 0)
        {
            if (!dizzyIsPlaying)
            {
                _anim.Dizzy_State();
                dizzyIsPlaying = true;
            }

            idleTimer -= Time.deltaTime;
        }
        else
        {
            dizzyIsPlaying = false;
            dizzyState = false;
            _anim.Idle(true);
        }


    }




    //function for taking damage from a player who has a damage powerup active 
    public void TakeDoubleDamage()
    {
        if (neutral)
        {
            SelectedBlock = segments[i];
            segments.RemoveAt(i);
            SelectedBlock.enabled = false;
            int e = segments.Count - 1;
            RawImage SelectedBlock2 = segments[e];
            segments.RemoveAt(e);
            SelectedBlock2.enabled = false; ;
            chargedTimer = 0f;
            neutral = true;
            hitByDamagePowerup = false;

        }
        else
        {
            SelectedBlock = segments[i];
            segments.RemoveAt(i);
            SelectedBlock.enabled = false;
            int e = segments.Count - 1;
            RawImage SelectedBlock2 = segments[e];
            segments.RemoveAt(e);
            SelectedBlock2.enabled = false;
            blockFlashing = true;
            StartCoroutine(Flashingbar());
            chargedTimer = 0f;
            regenTimer = 0f;
            neutral = false;
            hitByDamagePowerup = false;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        //calls the speed pickup function when the player picks up the speed powerup
        if (collider.gameObject.tag == "SpeedPowerup")
        {
            SpeedPickup();
        }

        //calls the health pickup function when the player picks up the health powerup
        if (collider.gameObject.tag == "HealthPowerup")
        {
            HealthPickup();
        }

    }
}
