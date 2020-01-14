using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPunch : MonoBehaviour
{
    public PlayerAttack attackScr;

    // Start is called before the first frame update

    void Start()
    {
        attackScr = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is pressed down...
        if (Input.GetMouseButtonDown(0) == true) 
        {
            attackScr.attacking = true;
        
        }
        // If the left mouse button is released...
        if (Input.GetMouseButtonUp(0) == true)
        {
            attackScr.attacking = false;
           
         
        }
    }
}
