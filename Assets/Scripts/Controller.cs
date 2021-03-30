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
    protected float InputRotationX; // ]0, 360]
    protected float InputRotationY; // ]-90, 90[

    private float horizontalKeyboardInput;
    private bool jumpButton;


    // camera control
    public Vector3 CameraPivot;
    public float CameraDistance;  // 0 for first, 3 for third person

    protected float RotationSpeed = 0.7f;

    private void Awake()
    {
        // returns the first active loaded object of type Player
        Player = FindObjectOfType<Player>();
    }


    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        // input
        horizontalKeyboardInput = Input.GetAxis("Horizontal");
        jumpButton = Input.GetKeyDown(KeyCode.Space);
        Vector3 mousePos = Input.mousePosition;

        // rotation for camera position
        InputRotationX = (mousePos.x * RotationSpeed) % 360f;
        InputRotationY = Mathf.Clamp(-mousePos.y / 2 * RotationSpeed, -88f, 88f);

        // left and forward
        var characterForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        // run and look direction
        var runDirection = characterForward * Input.GetAxisRaw("Vertical") + characterLeft * horizontalKeyboardInput;
        var lookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterForward;

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
