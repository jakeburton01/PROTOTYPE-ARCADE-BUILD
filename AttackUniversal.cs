using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUniversal : MonoBehaviour {

    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 2f;



    public bool is_Player, is_Enemy;

    public GameObject hit_FX;

    
    private Healthblocks KO;

    public GameObject[] player;


    private void Start()
    {
        player = gameObject.GetComponents<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);
        
        if (hit.Length > 0)
        {
            
            if (is_Player)
            {
                print(gameObject.tag); 
                if (gameObject.CompareTag(Tags.LEFT_PUNCH_TAG) ||
                    gameObject.CompareTag(Tags.RIGHT_PUNCH_TAG)) {


                    //print("Player has been hit");
                    hit[0].GetComponent<Healthblocks>().NormalDamage();
                }


               if(gameObject.CompareTag(Tags.STRONG_PUNCH_TAG))
                {
                    hit[0].GetComponent<Healthblocks>().NormalDamage();
                   
                }
                
             if(player[0].GetComponent<Healthblocks>().dizzyState == true && gameObject.CompareTag(Tags.STRONG_PUNCH_TAG))
                {
                    Destroy(hit[0].gameObject);
                }
        }
            gameObject.SetActive(false);
        }

      

}

    /*public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == collisionLayer && collision.gameObject.GetComponent<Healthblocks>().dizzyState == true && gameObject.CompareTag(Tags.STRONG_PUNCH_TAG))
        {
            print("Collided with Player 2");
            Destroy(collision.gameObject);
        }
    }*/

}
