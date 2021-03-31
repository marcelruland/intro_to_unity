/*
 * This script can be applied to both PC and NPC.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * This input string contains all information necessary for character
     * movement. The input can either be provided by the user or by some other
     * game logic (for NPCs).
     */
    [HideInInspector]
    public InputStr Input;
    public struct InputStr
    {
        public float LookX;
        public float LookZ;
        public float RunX;
        public float RunZ;
        public bool JumpButton;
        public bool PrimaryActionButton;
        public bool SecondaryActionButton;
    }

    // How fast should a player be and how high should they jump?
    private readonly float speed = 3f;
    private readonly float jumpForce = 4f;
    private readonly float detectObjectsInRadius = 1f;

    // carrying collectables
    [SerializeField] private GameObject carriedObject;

    protected Rigidbody Rigidbody;
    protected Quaternion LookRotation;


    /*
     * Awake is called when
     * (a) the script is loaded OR (b) when an object it is attached to is
     * instantiated. See wonderful diagram at 
     * https://gamedevbeginner.com/start-vs-awake-in-unity/
     */
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Throw();
        Jump();
        PickUpCollectable();
    }
    

    // FixedUpdate is called once every physics update
     void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
    }




    private void PickUpCollectable()
    {
        /*
         * 1. find closest Collectable
         * 2. destroy it
         * 3. put value into carriedObject variable
         */

        // do nothing if already carrying something
        if (carriedObject != null)
            return;

        // all the things we can collect (definitely not the ideal way but idc)
        string[] collectables = new string[] {
            "Banana",
            "Disinfectant",
            "Flour",
            "ToiletRoll",
            "Yeast",
        };

        // check for objects within radius
        Collider[] objectsInRadius = Physics.OverlapSphere(Rigidbody.position, detectObjectsInRadius);

        //iterate over found objects
        foreach (var objectInRadius in objectsInRadius){
            // if one of the objects within radius is a collectable
            var isCollectable = Array.Exists(collectables, element => element == objectInRadius.tag);
            if (isCollectable && Input.PrimaryActionButton)
            {
                Destroy(objectInRadius);
            };
        }
    }



    private void Jump()
    {
        /*
         * JUMPING
         * if jump input is true (key pressed etc) then
         * check if the character is on the ground (can't jump if airborne)
         * set y-velocity to jumpForce, leave other axes be
         */
        if (Input.JumpButton)
        {
            /*
             * The character's pivot is on the ground (y=0). We cast a ray
             * downwards by 0.1. The Raycast function returns true if the ray
             * collides with a collider, false otherwise
             */
            var grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, 1);
            if (grounded)
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, jumpForce, Rigidbody.velocity.z);
        }
    }


    private void UpdatePosition()
    {
        /*
         * ClampMagnitude sets the value of a vector to 1, IF it is < 1.
         * This is necessary because the actual speed should never be higher
         * than the value stored in the Speed variable.
         */
        var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        Rigidbody.velocity = new Vector3(inputRun.x * speed, Rigidbody.velocity.y, inputRun.z * speed);
    }


    private void UpdateRotation()
    {
        /*
         * If we get rotational input, then:
         * We calculate the angle of the current input and move the LookRotation
         * to it.
         * Or something like this, I swear I understood it at some point in the
         * middle of the night...
         */
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);
        if (inputLook.magnitude > 0.01f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up);
            LookRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        transform.rotation = LookRotation;
    }


    private void Throw()
    {
        if (Input.PrimaryActionButton)
        {
            //Instantiate(carriedObject, transform.position, Quaternion.identity);
        }
    }
}


