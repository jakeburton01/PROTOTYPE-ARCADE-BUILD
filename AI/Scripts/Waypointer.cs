using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypointer : MonoBehaviour
{
	public bool Reset;
    // Start is called before the first frame update
    void Start()
    {
        Reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter(Collider collision)
	{
        print("Waypoint hit");
		if (collision.gameObject.tag == "Enemy")
		{
			Reset = true;
            print("Waypoint Reset");
		}
	}
}
