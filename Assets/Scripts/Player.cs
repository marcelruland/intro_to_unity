/*
 * This script can be applied to both PC and NPC.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * This input struct contains all information necessary for character
     * movement. The input can either be provided by the user or by some other
     * game logic (for NPCs).
     */
    [HideInInspector] public InputStr Input;

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

    protected Rigidbody Rigidbody;
    protected Quaternion LookRotation;
    private Animator _animator;

    // How fast should a player be and how high should they jump?
    private const float _SPEED = 3f;
    private const float _JUMP_FORCE = 4f;
    private const float _THROW_FORCE = 8f;
    private const float _DETECTION_RADIUS = 2f;

    // Collectable and actions related
    private string _carriedCollectable;
    private string _secondaryAction;
    private string _tertiaryAction;

    // all the things we can collect (definitely not the ideal way but idc)
    private readonly string[] _collectables = new string[]
    {
        "Banana",
        //"Disinfectant",
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
    private readonly Dictionary<string, string[]> _actionsWithCollectable = new Dictionary<string, string[]>
    {
        {"Banana", new string[] {"", ""}},
        {"Disinfectant", new string[] {"hoard", "drink"}},
        {"Flour", new string[] {"hoard", ""}},
        {"Milk", new string[] {"hoard", "drink"}},
        {"ToiletRoll", new string[] {"hoard", ""}},
        {"Yeast", new string[] {"hoard", ""}},
    };
    
    private Dictionary<string, float> collectableValues = new Dictionary<string, float>
    {
        {"Banana", 0.40f},
        {"Disinfectant", 1.99f},
        {"Flour", 0.79f},
        {"Milk", 1.09f},
        {"ToiletRoll", 0.25f},
        {"Yeast", 0.49f},
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
        _animator = GetComponentInChildren<Animator>();
    }


    // Start is called before the first frame update
    private void Start()
    {
        ResetCollectableValues();
    }

    // Update is called once per frame
    private void Update()
    {
        Jump();

        bool carriesNothing = _carriedCollectable == "";
        if (carriesNothing)
            PickUpCollectable();
        else
            PerformActionWithCollectable();
    }


    // FixedUpdate is called once every physics update
    private void FixedUpdate()
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
        if (!Input.JumpButton) return;
        /*
         * The character's pivot is on the ground (y=0). We cast a ray
         * downwards by 0.1. The Raycast function returns true if the ray
         * collides with a collider, false otherwise
         */
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, 1);
        if (isGrounded)
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, _JUMP_FORCE, Rigidbody.velocity.z);
    }


    private void UpdatePosition()
    {
        /*
         * ClampMagnitude sets the value of a vector to 1, IF it is < 1.
         * This is necessary because the actual speed should never be higher
         * than the value stored in the Speed variable.
         */
        if (Input.RunX == 0 && Input.RunZ == 0)
        {
            _animator.SetBool("isMoving", false);
            return;
        }
        _animator.SetBool("isMoving", true);
        Vector3 inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        Rigidbody.velocity = new Vector3(inputRun.x * _SPEED, Rigidbody.velocity.y, inputRun.z * _SPEED);
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
        Vector3 inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);
        if (inputLook.magnitude > 0.01f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up);
            LookRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        transform.rotation = LookRotation;
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
        Collider[] objectsInRadius = Physics.OverlapSphere(Rigidbody.position, _DETECTION_RADIUS);
        // foreach (Collider objectInRadius in objectsInRadius)
        // {
        //     Debug.DrawLine(transform.position, objectInRadius.transform.position, Color.red);
        // }

        //iterate over found objects
        foreach (Collider objectInRadius in objectsInRadius)
        {
            // if one of the objects within radius is a collectable

            bool isCollectable = Array.Exists(_collectables, element => element == objectInRadius.tag);
            if (isCollectable)
            {
                // write tag to carriedObject variable and destroy gameObject
                _carriedCollectable = objectInRadius.tag;
                GameManager.Instance.CarriedCollectable = _carriedCollectable;
                Destroy(objectInRadius.gameObject);
                _secondaryAction = _actionsWithCollectable[_carriedCollectable][0];
                _tertiaryAction = _actionsWithCollectable[_carriedCollectable][1];
                break;
            }

            ;
        }
    }


    private void PerformActionWithCollectable()
    {
        if (Input.PrimaryActionButton)
        {
            ThrowCollectable();
            ResetCollectableValues();
        }
        else if (Input.SecondaryActionButton)
        {
            PerformSecondaryAction();
            ResetCollectableValues();
        }
        else if (Input.TertiaryActionButton)
        {
            PerformTertiaryAction();
            ResetCollectableValues();
        }
    }

    private void PerformSecondaryAction()
    {
        if (_secondaryAction == "hoard")
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
        _carriedCollectable = "";
        _secondaryAction = "";
        _tertiaryAction = "";
    }

    private void HoardCollectable()
    {
        GameManager.Instance.MoneySpent += collectableValues[_carriedCollectable];
        _carriedCollectable = "";
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
    }


    private void ThrowCollectable()
    {
        /*
         * INSTANTIATION
         * instantiate carried collectable
         */
        var pathToPrefab = "Prefabs/Collectables/" + _carriedCollectable;
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
        _carriedCollectable = "";
        GameManager.Instance.CarriedCollectable = _carriedCollectable;

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

        thrownCollectable.GetComponent<Rigidbody>().velocity = currentPlayerVelocity + transform.forward * _THROW_FORCE;
    }
}