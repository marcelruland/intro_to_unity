/*
 * Properties shared by both PC and NPC.
 * - movment (position and rotation)
 * - jumping
 * - throwing collectables
 */

using UnityEngine;

public class Player : MonoBehaviour
{
    // information for movement (position and rotation)
    public MovementInputStr MovementInput;
    public struct MovementInputStr
    {
        public float LookX;
        public float LookZ;
        public float RunX;
        public float RunZ;
    }

    // information for jumping and throwing
    public ActionInputStr ActionInput;
    public struct ActionInputStr
    {
        public bool Jump;
        public bool Throw;
        public string ThrownCollectable;
    }

    protected Rigidbody Rigidbody;
    protected Quaternion LookRotation;
    private Animator _animator;

    // How fast should a player be and how high should they jump?
    private const float _SPEED = 3f;
    private const float _JUMP_FORCE = 4f;
    private const float _THROW_FORCE = 8f;

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

    // Update is called once per frame
    private void Update()
    {
        Jump();
        ThrowCollectable();
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
         * if jump input is true (key pressed etc) then
         * check if the character is on the ground (can't jump if airborne)
         * set y-velocity to jumpForce, leave other axes be
         */
        if (!ActionInput.Jump) return;
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
        if (MovementInput.RunX == 0 && MovementInput.RunZ == 0)
        {
            _animator.SetBool("isMoving", false);
            return;
        }

        _animator.SetBool("isMoving", true);
        Vector3 inputRun = Vector3.ClampMagnitude(new Vector3(MovementInput.RunX, 0, MovementInput.RunZ), 1);
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
        Vector3 inputLook = Vector3.ClampMagnitude(new Vector3(MovementInput.LookX, 0, MovementInput.LookZ), 1);
        if (inputLook.magnitude > 0.01f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up);
            LookRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        transform.rotation = LookRotation;
    }

    private void ThrowCollectable()
    {
        if (!ActionInput.Throw)
            return;
        /*
         * INSTANTIATION
         * instantiate carried collectable
         */
        string pathToPrefab = "Prefabs/Collectables/" + ActionInput.ThrownCollectable;
        GameObject thrownCollectable =
            Instantiate(
                    Resources.Load(
                        pathToPrefab,
                        typeof(GameObject)
                    ),
                    transform.position,
                    Quaternion.identity
                )
                as GameObject;

        /*
        * POSITION
        * move instantiated collectable in front of player
        */
        Transform transform1 = transform;
        Vector3 throwPosition =
            transform1.position
            + transform1.forward * 0f
            + transform1.right * 0.5f
            + Vector3.up;
        thrownCollectable.transform.position = throwPosition;

        // VELOCITY
        // same velocity as player
        Vector3 currentPlayerVelocity = Vector3.ClampMagnitude(new Vector3(MovementInput.RunX, 0, MovementInput.RunZ), 1);

        Vector3 forward = transform1.forward;
        Vector3 throwDirection = new Vector3(forward.x, 0.2f, forward.z).normalized;
        thrownCollectable.GetComponent<Rigidbody>().velocity = currentPlayerVelocity + throwDirection * _THROW_FORCE;
    }
}