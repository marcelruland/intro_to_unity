/*
 * This script can be applied to both PC and NPC
 */

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
        public bool Jump;
    }

    // How fast should a player be and how high should they jump?
    public float Speed = 3f;
    public float JumpForce = 4f;

    protected Rigidbody Rigidbody;
    protected Quaternion LookRotation;


    /*
     * Awake is called when
     * (a) the script is loaded
     * OR
     * (b) when an object it is attached to is instantiated
     * See wonderful diagram at 
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
        
    }
    

    // FixedUpdate is called once every physics update
     void FixedUpdate()
    {

        /*
         * INPUT
         * inputRun and inputLook take the data from the InputStr and convert it
         * to Vector3's.
         * ClampMagnitude sets the value of a vector to 1, IF it is < 1.
         * This is necessary because the actual speed should never be higher
         * than the value stored in the Speed variable.
         */
        var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

        /*
         * POSITION
         * use the inputRun Vector3 to calculate the new velocity
         */
        Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

        /*
         * ROTATION
         * If we get rotational input, then:
         * TODO: assign angle to separate variable before calling AngleAxis
         */
        if (inputLook.magnitude > 0.01f)
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        transform.rotation = LookRotation;

        /*
         * JUMPING
         * if jump input is true (key pressed etc) then
         * check if the character is on the ground (can't jump if airborne)
         * set y-velocity to jumpForce, leave other axes be
         */
        if (Input.Jump)
        {
            /*
             * The character's pivot is on the ground (y=0). We cast a ray
             * downwards by 0.1. The Raycast function returns true if the ray
             * collides with a collider, false otherwise
             */
            var grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, 1);
            if (grounded)
            {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
            }
        }
    }
}
