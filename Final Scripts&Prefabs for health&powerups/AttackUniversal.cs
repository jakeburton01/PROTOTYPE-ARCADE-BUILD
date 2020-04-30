﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUniversal : MonoBehaviour
{

    public LayerMask collisionLayer;
    public float radius = 1f;
    //public float damage = 2f;

    private CharacterAnimation isHit;

    public bool is_Player, Is_Collided;

    public GameObject hit_FX;



    //private Healthblocks KO;

    public GameObject[] player;


    private void Start()
    {
        player = gameObject.GetComponents<GameObject>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!Is_Collided)
        {
            DetectCollision();
        }
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);

        if (hit.Length > 0)
        {

            if (is_Player)
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y = 1.3f;

                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }

                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);


                //print(gameObject.tag); 
                if (gameObject.CompareTag(Tags.LEFT_PUNCH_TAG) ||
                    gameObject.CompareTag(Tags.RIGHT_PUNCH_TAG))
                {

                    Is_Collided = true;
                    print("ObjectIsHit");

                  //  if (gameObject.GetComponentInParent<Healthblocks>().damagePowerup == true)
                  //  {
                   //     hit[0].GetComponent<Healthblocks>().TakeDoubleDamage();
                  //      gameObject.GetComponentInParent<Healthblocks>().damagePowerup = false;
                   // }
                   // else
                    //{
                        hit[0].GetComponent<Healthblocks>().NormalDamage();
                        //hit[0].gameObject.GetComponent<CharacterAnimation>().BeenHit();
                   // }
                }




                if (gameObject.CompareTag(Tags.STRONG_PUNCH_TAG))
                {

                   // if (gameObject.GetComponentInParent<Healthblocks>().damagePowerup == true)
                   // {
                   //     hit[0].GetComponent<Healthblocks>().TakeDoubleDamage();
                    //    gameObject.GetComponentInParent<Healthblocks>().damagePowerup = false;
                   //     
                    //} else
                    //{
                        hit[0].GetComponent<Healthblocks>().NormalDamage();
                   // }

                }

                if (player[0].GetComponent<Healthblocks>().dizzyState == true && gameObject.CompareTag(Tags.STRONG_PUNCH_TAG))
                {
                    hit[0].gameObject.SetActive(false);
                    hit[0].GetComponent<ExplosionFX>().Explosion();
                }
            }
            gameObject.SetActive(false);
        }



    }



}
