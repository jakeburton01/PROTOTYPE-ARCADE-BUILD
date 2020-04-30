using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public float destroyTimer = 10f;
    GameObject powerup;

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
}
