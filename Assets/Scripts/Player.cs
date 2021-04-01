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
     * This input struct contains all information necessary for character
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
        public bool TertiaryActionButton;
    }

    // How fast should a player be and how high should they jump?
    private readonly float speed = 3f;
    private readonly float jumpForce = 4f;
    private readonly float throwForce = 8f;
    private readonly float detectObjectsInRadius = 2f;

    // Collectable and actions related
    [SerializeField] public string carriedCollectable;
    //[SerializeField] public string primaryAction;
    [SerializeField] public string secondaryAction;
    [SerializeField] public string tertiaryAction;

    protected Rigidbody Rigidbody;
    protected Quaternion lookRotation;

    // all the things we can collect (definitely not the ideal way but idc)
    private readonly string[] collectables = new string[] {
        "Banana",
        "Disinfectant",
        "Flour",
        "Milk",
        "ToiletRoll",
        "Yeast",
    };

    /*
     * look up actions for Collectable
     * NOTE: primary action is ALWAYS throw so this stores only secondary and
     * tertiary actions
     */
    private readonly Dictionary<string, string[]> actionsWithCollectable = new Dictionary<string, string[]>
    {
        { "Banana", new string[] {"", "" } },
        { "Disinfectant",new string[] {"hoard", "drink" } },
        { "Flour", new string[] {"hoard", "" }  },
        { "Milk", new string[] {"hoard", "drink" }  },
        { "ToiletRoll", new string[] {"hoard", "" }  },
        { "Yeast", new string[] {"hoard", "" }  },
    };


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
        Jump();
        
        var carriesNothing = carriedCollectable == "";
        if (carriesNothing)
            PickUpCollectable();
        else
            PerformActionWithCollectable();
    }
    

    // FixedUpdate is called once every physics update
    void FixedUpdate()
    {
        UpdatePosition();
        UpdateRotation();
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
            lookRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        transform.rotation = lookRotation;
    }


    private void PickUpCollectable()
    {
        /*
         * 1. find closest Collectable
         * 2. destroy it
         * 3. put value into carriedObject variable
         */
        if (!Input.PrimaryActionButton)
            return;

        // check for objects within radius
        Collider[] objectsInRadius = Physics.OverlapSphere(Rigidbody.position, detectObjectsInRadius);

        //iterate over found objects
        foreach (var objectInRadius in objectsInRadius){
            // if one of the objects within radius is a collectable

            var isCollectable = Array.Exists(collectables, element => element == objectInRadius.tag);
            if (isCollectable)
            {
                // write tag to carriedObject variable and destroy gameObject
                carriedCollectable = objectInRadius.tag;
                Destroy(objectInRadius.gameObject);
                secondaryAction = actionsWithCollectable[carriedCollectable][0];
                tertiaryAction = actionsWithCollectable[carriedCollectable][1];
                break;
            };
        }
    }


    private void PerformActionWithCollectable()
    {
        if (Input.PrimaryActionButton)
        {
            ThrowCollectable();
            ResetCollectableValues();
        } else if (Input.SecondaryActionButton)
        {
            PerformSecondaryAction();
            ResetCollectableValues();
        } else if (Input.TertiaryActionButton)
        {
            PerformTertiaryAction();
            ResetCollectableValues();
        }
    }

    private void PerformSecondaryAction()
    {
        if (secondaryAction == "hoard")
            HoardCollectable();
        else
            throw new NotImplementedException();
    }

    private void PerformTertiaryAction()
    {
        throw new NotImplementedException();
    }


    private void ResetCollectableValues()
    {
        carriedCollectable = "";
        secondaryAction = "";
        tertiaryAction = "";
    }

    private void HoardCollectable()
    {
        throw new NotImplementedException();
    }


    private void ThrowCollectable()
    {
        /*
         * INSTANTIATION
         * instantiate carried collectable
         */
        var pathToPrefab = "Prefabs/Collectables/" + carriedCollectable;
        var thrownCollectable =
            Instantiate(
                Resources.Load(
                    pathToPrefab,
                    typeof(GameObject)
                ),
                transform.position,
                Quaternion.identity
            )
            as GameObject;
        // can't carry what you threw away can ya?
        carriedCollectable = "";

        /*
        * POSITION
        * move instantiated collectable in front of player
        */
        var throwPosition = 
            transform.position
            + transform.forward * 0f
            + transform.right * 0.5f
            + Vector3.up;
        thrownCollectable.transform.position = throwPosition;

        // VELOCITY
        // same velocity as player
        var currentPlayerVelocity = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);

        thrownCollectable.GetComponent<Rigidbody>().velocity = currentPlayerVelocity + transform.forward * throwForce;
    }
}


