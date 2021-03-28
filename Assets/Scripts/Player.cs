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
    private Rigidbody _rigidBodyComponent;

    [SerializeField] private float _speed = 3;
    private float _rotation;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBodyComponent = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        ListenForKeys();
    }


    // FixedUpdate is called once every physics update
    // do physic-ish stuff here so that low fps don't slow physics down
     void FixedUpdate()
    {
        Jump();
        Move();
    }


    void ListenForKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _jumpKeyPressed = true;
    }


    void Jump()
    {
        if (_jumpKeyPressed)
        {
            _rigidBodyComponent.AddForce(Vector3.up * _speed, ForceMode.VelocityChange);
            _jumpKeyPressed = false;
        }
    }


    void Move()
    {
        _rigidBodyComponent.velocity = new Vector3(_rigidBodyComponent.velocity.x, _rigidBodyComponent.velocity.y, _verticalInput);
    }
}
