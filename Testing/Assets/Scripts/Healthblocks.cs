using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthblocks : MonoBehaviour
{
    public List <Image> segments = new List<Image>();

    public bool blockFlashing;
    public int counter;
    public float regenTimer;
    public int i;
    public float chargedTimer;
    public Text gameOverText;
    public bool neutral;
    public bool regen;

    public GameObject Playerunit;
    public GameObject AIunit;
    public float AIHPChange;

    public Image BlockHP;
    public bool Set;
   

    void Start()
    {
        int i = segments.Count - 1;
        blockFlashing = false;
        regenTimer = 0f;
        gameOverText.enabled = false;
        MoveCube unitscript = Playerunit.GetComponent<MoveCube>();
        neutral = true;
        AIHPChange = 0;
        regen = false;
        Set = false;

    }

    //need code to count time that button held down for 
    //if just pressed then normal hit - make one segment flash and then if hit again destroy flashing bar
    //if button held for 2 seconds remove 1 bar straight away 
    //if button held for 3 seconds remove 1 bar and make next bar flash 
    //if button held for 4 or more seconds then remove two bars straight away 

    void Update()
    {
        int i = segments.Count - 1;
        

        if (AIunit != null)
        {
            AI AIunitscript;
            AIunitscript = AIunit.GetComponent<AI>();
			
			//########################WANT TO USE THIS UPDATED VERSION OF HP CHANGE BUT HAVEN'T TESTED YET######################
			/*if(AIunitscript.HPBarSet)
			{
				if(!blockFlashing)
				{
					switch(AIunitscript.HPBarHitValue)
					{
						case 10:
						{
							StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
                            break;
						}
						
						case 20:
						{
							StartCoroutine(Flashingbar());
                            Set = true;
							Destroy(segments[i]);
                            segments.RemoveAt(i);
                            AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
                            break;
						}
						
						case 30:
						{
							
                            Set = true;
							Destroy(segments[i]);
                            segments.RemoveAt(i);
							StartCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
                            break;
						}
					}
				}
				
				else
				{
					switch(AIunitscript.HPBarHitValue)
					{
						case 10:
						{
							regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[i]);
                            segments.RemoveAt(i);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
                            break;
						}
						
						case 20:
						{
							regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[i]);
                            segments.RemoveAt(i);

                            StopCoroutine(Flashingbar());
							StartCoroutine(Flashingbar());
							AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
							break;
						}
						
						case 30:
						{
							regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
							StopCoroutine(Flashingbar());
                            Destroy(segments[i]);
                            segments.RemoveAt(i);
							i = i-1;
							Destroy(segments[i]);
                            segments.RemoveAt(i);
                            
							
							AIunitscript.HPBarSet = false;
							AIunitscript.HPBarHitValue = 0;
							break;
						}
					}
				}
					
			}*/
			//#####################################################################################
            
            if (regen == true)
            {
                AIunitscript.Health += 10;
                print("regen"  + i);
                regen = false;
                StopCoroutine(Flashingbar());
                print("coroutinestopped");
                segments[i].enabled = true;
                segments[i].color = Color.white;
                neutral = true;
                


            }
            if (AIunitscript.HPBarSet == true)
            {

                if (AIunitscript.Health == 110 || AIunitscript.Health == 90 || AIunitscript.Health == 70 || AIunitscript.Health == 50 || AIunitscript.Health == 30 || AIunitscript.Health == 10)
                {
                    blockFlashing = true;
                }

                
            }
            if (blockFlashing && AIunitscript.HPBarSet == true)
            {
               
                switch (AIunitscript.Health)
                {
					
					case 110: 
						{
						StartCoroutine(Flashingbar());
                        Set = true;
                        AIunitscript.HPBarSet = false;
                        break;
						}
					
                    case 90:
                        {
                            
                            StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
                            break;
                        }

                    case 70:
                        {
                            
                            StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
                            break;
                        }

                    case 50:
                        {
                            
                            StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
                            break;
                        }

                    case 30:
                        {
                            
                            StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
                            break;
                        }

                    case 10:
                        {
                            
                            StartCoroutine(Flashingbar());
                            Set = true;
                            AIunitscript.HPBarSet = false;
                            break;
                        }
						case 100:
						{
						regenTimer = 0;
                        Set = false;
                        blockFlashing = false;
                        Destroy(segments[5]);
                        segments.RemoveAt(5);

                        StopCoroutine(Flashingbar());
                        AIunitscript.HPBarSet = false;
                        break;
					}
                    case 80:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[4]);
                            segments.RemoveAt(4);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 60:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[3]);
                            segments.RemoveAt(3);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 40:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[2]);
                            segments.RemoveAt(2);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 20:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[1]);
                            segments.RemoveAt(1);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 0:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[0]);
                            segments.RemoveAt(0);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
						}
                }
            }
           /*else if (AIunitscript.HPBarSet == true)
                {
                    switch (AIunitscript.Health)
                    {
					
					case 100:
						{
						regenTimer = 0;
                        Set = false;
                        blockFlashing = false;
                        Destroy(segments[5]);
                        segments.RemoveAt(5);

                        StopCoroutine(Flashingbar());
                        AIunitscript.HPBarSet = false;
                        break;
					}
                    case 80:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[4]);
                            segments.RemoveAt(4);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 60:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[3]);
                            segments.RemoveAt(3);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 40:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[2]);
                            segments.RemoveAt(2);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 20:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[1]);
                            segments.RemoveAt(1);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    case 0:
                        {
                            regenTimer = 0;
                            Set = false;
                            blockFlashing = false;
                            Destroy(segments[0]);
                            segments.RemoveAt(0);

                            StopCoroutine(Flashingbar());
                            AIunitscript.HPBarSet = false;
                            break;
                        }
                    }
                }*/
            



        }
        if (blockFlashing)
        {
            regenTimer += Time.deltaTime;
        }
        if(!blockFlashing)
        {
            regenTimer = 0;
        }


        //if healthbar is empty then bring up game over text 
        if (segments.Count == 0)
        {
            gameOverText.enabled = true;
        }

        
        if(regenTimer >=8f)
        {
            StopAllCoroutines();
        }
        if (regenTimer >= 8f)
        {
            
            regenTimer = 0f;
            if (blockFlashing == true)
            {
                //StopCoroutine(Flashingbar());
                //segments[i].enabled = true;
                
                blockFlashing = false;
                neutral =false;
                regen = true;

            }

        }

        //if 5 seconds have passed since health block began flashing then stop health block flashing(regenerating that health block)
        if (regenTimer >= 10f)
        {

          


        }



        //n key == normal attack - everytime n key pushed down counter increases by 1
        if (Input.GetKeyDown("n"))
        {
            regenTimer += Time.deltaTime;
            regenTimer = 0f;


            if (neutral)
            {
                //coroutine that makes block flash begins and sets the blockflashing boolean to be true 
                blockFlashing = true;
                StartCoroutine(Flashingbar());
                neutral = false;
            }
            else
            {
                //couroutine stopped so block stops flashing - meaning the blockfalshing boolean is now false - and the block that was flashing gets destroyed 
                blockFlashing = false;
                Destroy(segments[i]);
                segments.RemoveAt(i);
                StopCoroutine(Flashingbar());
                neutral = true;
            }
        }



        //c will represent the charged attack - when pressed the chargedtimer will start  
        if (Input.GetKey("c"))
        {
            chargedTimer += Time.deltaTime;
        }

        //when the c key is lifted up depending on the length of the charged timer health blocks will be removed or flash  
        if (Input.GetKeyUp("c"))
        {
            //timer is between 2 and 3 seconds so destroys 1 block of health 
            if (chargedTimer >= 2f && chargedTimer < 3f)
            {
                //if no blocks are flashing then destroy one health block 
                if (neutral)
                {
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
                    chargedTimer = 0f;
                    //neutral = true;
                }
                else
                //if a block is flashing then destroy the flashing block and make the next block flash 
                {
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
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
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
                    blockFlashing = true;
                    StartCoroutine(Flashingbar());
                    chargedTimer = 0f;
                    regenTimer = 0f;
                    neutral = false;
                }
                else
                //if a block is flashing then destroy the flashing block as well as one health block 
                {
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
                    int e = segments.Count - 1;
                    Destroy(segments[e]);
                    segments.RemoveAt(e);
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
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
                    int e = segments.Count - 1;
                    Destroy(segments[e]);
                    segments.RemoveAt(e);
                    chargedTimer = 0f;
                    neutral = true;
                }
                else
                //if a block is flashing then remove the flashing block and a health block and then make a health block flash 
                {
                    Destroy(segments[i]);
                    segments.RemoveAt(i);
                    int e = segments.Count - 1;
                    Destroy(segments[e]);
                    segments.RemoveAt(e);
                    blockFlashing = true;
                    StartCoroutine(Flashingbar());
                    chargedTimer = 0f;
                    regenTimer = 0f;
                    neutral = false;
                }

            }
        }


    }

    public IEnumerator Flashingbar()
    {
        print("turned off");
        //while blockflashing is true the last health block in the array will be enabled and disabled to give the illusion of it flashing
        while (blockFlashing)
        {
            print("set");
            int i = segments.Count - 1;
            segments[i].color = Color.red;
            segments[i].enabled = false;
            yield return new WaitForSeconds(0.5f);
            segments[i].enabled = true;
            yield return new WaitForSeconds(0.5f);
            segments[i].enabled = false;
            yield return new WaitForSeconds(0.5f);
            segments[i].enabled = true;

            
            
        }

    }


    public void FlashTrue()
    {
        blockFlashing = false;
        Destroy(segments[i]);
        segments.RemoveAt(i);
        StopCoroutine(Flashingbar());
        neutral = true;

    }


    public void FlashFalse()
    {
        blockFlashing = true;
        StartCoroutine(Flashingbar());
        neutral = false;
    }



}
