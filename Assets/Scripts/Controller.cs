/*
 * This script only applies to the playable character.
 * Do NOT use for NPCs.
 */


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
    private float verticalKeyboardInput;
    //private float horizontalMouseMovement;
    //private float verticalMouseMovement;

    protected float RotationSpeed = 0.7f;

    private void Awake()
    {
        Player = FindObjectOfType<Player>();
    }


    private void FixedUpdate()
    {
        horizontalKeyboardInput = Input.GetAxis("Horizontal");
        verticalKeyboardInput = Input.GetAxis("Vertical");
        //horizontalMouseMovement = Input.GetAxis("Mouse X");
        //verticalMouseMovement = Input.GetAxis("Mouse Y");
        Vector3 mousePos = Input.mousePosition;

        InputRotationX = (mousePos.x * RotationSpeed) % 360f;
        InputRotationY = Mathf.Clamp(InputRotationY - mousePos.y * RotationSpeed * Time.deltaTime, -88f, 88f);

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
        //Player.Input.Jump = JumpButton.Pressed;
    }
}
