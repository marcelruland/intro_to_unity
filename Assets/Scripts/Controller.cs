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
    private float _rotationSpeed = 0.7f;
    private float _inputRotationX; // ]0, 360]
    private float _inputRotationY; // ]-80, 80[

    // camera control
    private Vector3 _cameraPivot = new Vector3(0f, 1.42f, 0f);
    private float _cameraDistance = 2.2f;  // 0 for first, 3 for third person


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
        _inputRotationX = (mousePos.x * _rotationSpeed) % 360f;
        _inputRotationY = Mathf.Clamp(-(mousePos.y - 300) * _rotationSpeed, -80f, 80f);

        // forward and left relative to the player
        // useful for calculating run and look direction
        var playerForward = Quaternion.AngleAxis(_inputRotationX, Vector3.up) * Vector3.forward;
        var playerLeft = Quaternion.AngleAxis(_inputRotationX + 90, Vector3.up) * Vector3.forward;

        // run and look direction
        var runDirection = playerForward * verticalKeyboardInput + playerLeft * horizontalKeyboardInput;
        var lookDirection = Quaternion.AngleAxis(_inputRotationY, playerLeft) * playerForward;

        // feed calculated values to player object
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;

        // camera position
        var characterPivot = Quaternion.AngleAxis(_inputRotationX, Vector3.up) * _cameraPivot;
        StartCoroutine(SetCamera(lookDirection, characterPivot));
    }


    private IEnumerator SetCamera(Vector3 lookDirection, Vector3 characterPivot)
    {
        yield return new WaitForFixedUpdate();

        Camera.main.transform.position = (transform.position + characterPivot) - lookDirection * _cameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
