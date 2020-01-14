using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollideWitPunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Deathzone")
        {
            FindObjectOfType<GameManager>().EndGame(); // Restarts game when opponent has collided with Deathzone
            Destroy(gameObject); // Destroys opponent
            
        }
    }
}
