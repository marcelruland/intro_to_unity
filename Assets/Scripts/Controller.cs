/*
 * This script only applies to the playable character.
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

    // movement
    [SerializeField] private float RotationSpeed = 0.7f;
    protected float InputRotationX; // ]0, 360]
    protected float InputRotationY; // ]-80, 80[

    // camera control
    public Vector3 CameraPivot;
    public float CameraDistance;  // 0 for first, 3 for third person


    /*
     * Awake is called when
     * (a) the script is loaded OR (b) when an object it is attached to is
     * instantiated. See wonderful diagram at 
     * https://gamedevbeginner.com/start-vs-awake-in-unity/
     */
    private void Awake()
    {
        // returns the first active loaded object of type Player
        Player = FindObjectOfType<Player>();
    }


    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        /* 
         * INPUT
         * - keyboard input for movement
         * - jump button (Space) for jumping (duh...)
         * - mousePosition for view rotation (Probably not the best way in the
         *   world but Siriusly who cares if it works?)
         */
        horizontalKeyboardInput = Input.GetAxis("Horizontal");
        verticalKeyboardInput = Input.GetAxis("Vertical");
        jumpButton = Input.GetKeyDown(KeyCode.Space);
        Vector3 mousePos = Input.mousePosition;

        // rotation for camera position
        InputRotationX = (mousePos.x * RotationSpeed) % 360f;
        InputRotationY = Mathf.Clamp(-mousePos.y / 2 * RotationSpeed, -80f, 80f);

        // forward and left relative to the player
        // useful for calculating run and look direction
        var playerForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
        var playerLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        // run and look direction
        var runDirection = playerForward * verticalKeyboardInput + playerLeft * horizontalKeyboardInput;
        var lookDirection = Quaternion.AngleAxis(InputRotationY, playerLeft) * playerForward;

        // set values
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;
        Player.Input.Jump = jumpButton;

        // camera position
        var characterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * CameraPivot;
        StartCoroutine(SetCamera(lookDirection, characterPivot));
    }


    private IEnumerator SetCamera(Vector3 lookDirection, Vector3 characterPivot)
    {
        yield return new WaitForFixedUpdate();

        Camera.main.transform.position = (transform.position + characterPivot) - lookDirection * CameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
