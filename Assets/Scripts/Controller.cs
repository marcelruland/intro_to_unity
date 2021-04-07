/*
 * This script only applies to the playable character
 * Do NOT use for NPCs.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // input
    protected Player Player;
    private float horizontalKeyboardInput;
    private float verticalKeyboardInput;
    private bool jumpButton;
    private bool primaryActionButton;
    private bool secondaryActionButton;
    private bool tertiaryActionButton;

    // movement
    // TODO: rotationSpeed doesn't do anything, no idea why
    [SerializeField] private float rotationSpeed = 0.7f;
    protected float inputRotationX; // ]0, 360]
    protected float inputRotationY; // ]-80, 80[

    // camera control
    public Vector3 cameraPivot = new Vector3(0f, 1.42f, 0f);
    public float cameraDistance = 2.2f;  // 0 for first, 3 for third person


    /*
     * Awake is called when
     * (a) the script is loaded OR (b) when an object it is attached to is
     * instantiated. See wonderful diagram at 
     * https://gamedevbeginner.com/start-vs-awake-in-unity/
     */
    private void Awake()
    {
        // returns the first active loaded object of type Player
        Player = GetComponent<Player>();
    }


    private void Update()
    {
        /*
         * Get input for jumping and throwing and pass on.
         */
        jumpButton = Input.GetKeyDown(KeyCode.Space);
        primaryActionButton = Input.GetKeyDown(KeyCode.E);
        secondaryActionButton = Input.GetKeyDown(KeyCode.R);
        tertiaryActionButton = Input.GetKeyDown(KeyCode.T);

        Player.Input.JumpButton = jumpButton;
        Player.Input.PrimaryActionButton = primaryActionButton;
        Player.Input.SecondaryActionButton = secondaryActionButton;
        Player.Input.TertiaryActionButton = tertiaryActionButton;
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        /* 
         * INPUT
         * - keyboard input for movement
         * - mousePosition for view rotation (Probably not the best way in the
         *   world but Siriusly who cares if it works?)
         */
        horizontalKeyboardInput = Input.GetAxis("Horizontal");
        verticalKeyboardInput = Input.GetAxis("Vertical");
        Vector3 mousePos = Input.mousePosition;

        // rotation for camera position
        inputRotationX = (mousePos.x * rotationSpeed) % 360f;
        inputRotationY = Mathf.Clamp(-(mousePos.y - 300) * rotationSpeed, -80f, 80f);

        // forward and left relative to the player
        // useful for calculating run and look direction
        var playerForward = Quaternion.AngleAxis(inputRotationX, Vector3.up) * Vector3.forward;
        var playerLeft = Quaternion.AngleAxis(inputRotationX + 90, Vector3.up) * Vector3.forward;

        // run and look direction
        var runDirection = playerForward * verticalKeyboardInput + playerLeft * horizontalKeyboardInput;
        var lookDirection = Quaternion.AngleAxis(inputRotationY, playerLeft) * playerForward;

        // feed calculated values to player object
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;

        // camera position
        var characterPivot = Quaternion.AngleAxis(inputRotationX, Vector3.up) * cameraPivot;
        StartCoroutine(SetCamera(lookDirection, characterPivot));
    }


    private IEnumerator SetCamera(Vector3 lookDirection, Vector3 characterPivot)
    {
        yield return new WaitForFixedUpdate();

        Camera.main.transform.position = (transform.position + characterPivot) - lookDirection * cameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
