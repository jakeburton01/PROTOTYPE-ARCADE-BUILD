using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool heavyAtt;
    public bool attacking;
    //public Rigidbody rb;
    public float thrust;
    public GameObject enemy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent <Rigidbody>();
        attacking = false;
        heavyAtt = false;
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Enemy" && attacking) // If player collide's with opponent with left punch
        {
            Debug.Log("Collision Detected");
          
            enemy.GetComponent<Rigidbody>().AddForce(thrust, 0,0, ForceMode.Impulse); // Pushes opponent back
        }
    }
   
}
