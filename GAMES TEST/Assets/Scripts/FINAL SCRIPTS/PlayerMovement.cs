using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody myBody;
    private CharacterAnimation player_Anim;

    public float walk_Speed = 2f;
    public float z_Speed = 1.5f;

    private float rotation_Y = -90f;
    //private float rotation_x = -90f;
    //private float rotation_Speed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        player_Anim = GetComponentInChildren<CharacterAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        //RotatePlayer();
        AnimatePlayerWalk();
        Rotate();
    }

    void FixedUpdate()
    {
        DetectMovement();
    }

    void DetectMovement() {

        myBody.velocity = new Vector3(
            Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) * (-walk_Speed), myBody.velocity.y,
            Input.GetAxisRaw(Axis.VERTICAL_AXIS) * (-z_Speed));
    } // This move the character

    //void RotatePlayer()
    //{
    //    if (Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) > 0)
    //    {
    //        transform.rotation = Quaternion.Euler(0f, rotation_Y, 0f);
    //    }
    //    else if (Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) < 0)
    //    {
    //        transform.rotation = Quaternion.Euler(0f, Mathf.Abs(rotation_Y), 0f);
    //    }
    //} // This Rotates the character

    void Rotate()
    {

        //Vector3 rotation_direction = Vector3.zero;

        if (Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) < 0)
        {
            
            transform.rotation = Quaternion.Euler(0f, Mathf.Abs(rotation_Y), 0f);
        }

        if (Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) > 0)
        {
            
            transform.rotation = Quaternion.Euler(0f, rotation_Y, 0f);
        }

        //if (Input.GetAxisRaw(Axis.VERTICAL_AXIS) < 0)
        //{
        //    //rotation_direction = transform.TransformDirection(Vector3.left);
        //    transform.rotation = Quaternion.Euler(0f, Mathf.Abs(rotation_x), 0f);
        //}

        //if (Input.GetAxisRaw(Axis.VERTICAL_AXIS) > 0)
        //{
        //    //rotation_direction = transform.TransformDirection(Vector3.right);
        //    transform.rotation = Quaternion.Euler(0f, rotation_x, 0f);
        //}
    }



        void AnimatePlayerWalk()
    {
        if(Input.GetAxisRaw(Axis.HORIZONTAL_AXIS) !=0 || 
            Input.GetAxisRaw(Axis.VERTICAL_AXIS) != 0)
        {
            
            player_Anim.Walk(true);
        }
        else
        {
            player_Anim.Walk(false);
        }
    }
}
