using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // declare player fields
    private bool isAirborn;
    private bool _jumpKeyPressed;
    private float _horizontalInput;
    private float _verticalInput;
    private Rigidbody _rigidbodyComponent;

    private float _rotation;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbodyComponent = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            _jumpKeyPressed = true;
    }


    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        if (_jumpKeyPressed)
        {
            _rigidbodyComponent.AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            _jumpKeyPressed = false;
        }

        _rigidbodyComponent.velocity = new Vector3(_rigidbodyComponent.velocity.x, _rigidbodyComponent.velocity.y, _verticalInput);
    }
}
