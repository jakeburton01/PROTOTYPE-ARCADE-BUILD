using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour {

    private CharacterController charController;

    public float movement_Speed = 3f;
    public float gravity = 9.8f;
    public float rotation_Speed = 0.15f;
    public float rotateDegreesPerSecond = 180f;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }


    void Move()
    {
        if(Input.GetAxis(Axis.VERTICAL_AXIS) > 0)
        {

            Vector3 moveDirection = transform.forward;
            moveDirection.y -= gravity * Time.deltaTime;

            charController.Move(moveDirection * movement_Speed * Time.deltaTime);

        } else if(Input.GetAxis(Axis.VERTICAL_AXIS) < 0) {


            Vector3 moveDirection = -transform.forward;
            moveDirection.y -= gravity * Time.deltaTime;

            charController.Move(moveDirection * movement_Speed * Time.deltaTime);

        }

    
    }

    void Rotate()
    {

        Vector3 rotation_direction = Vector3.zero;

        if(Input.GetAxis(Axis.HORIZONTAL_AXIS) < 0)
        {
            rotation_direction = transform.TransformDirection(Vector3.left);
        }

        if (Input.GetAxis(Axis.HORIZONTAL_AXIS) > 0)
        {
            rotation_direction = transform.TransformDirection(Vector3.right);
        }

        if(rotation_direction != Vector3.zero)
        {

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(rotation_direction), rotateDegreesPerSecond * Time.deltaTime);

        }
    }

}
