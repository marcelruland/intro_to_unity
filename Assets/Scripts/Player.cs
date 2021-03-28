using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // declare player fields
    private bool isAirborn;
    private bool jumpKeyPressed;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpKeyPressed = true;
    }


    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        if (jumpKeyPressed)
        {
            rigidbodyComponent.AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            jumpKeyPressed = false;
        }

        rigidbodyComponent.velocity = new Vector3(rigidbodyComponent.velocity.x, rigidbodyComponent.velocity.y, verticalInput);
    }
}
