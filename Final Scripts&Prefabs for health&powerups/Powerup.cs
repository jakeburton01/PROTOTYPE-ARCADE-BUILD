using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{

    public float destroyTimer = 10f;
    GameObject powerup;
    public bool health, speed, damage;

    // Start is called before the first frame update
    void Start()
    {
        powerup = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0f)
        {
            Destroy(powerup);
        }


    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            Destroy(powerup);
        }
    }
}
