

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    // input
    protected Player Player;
    // private float horizontalKeyboardInput;
    // private float verticalKeyboardInput;
    // private bool jumpButton;
    // private bool primaryActionButton;
    // private bool secondaryActionButton;
    // private bool tertiaryActionButton;

    // movement
    // [SerializeField] private float rotationSpeed = 0.7f;
    // protected float inputRotationX; // ]0, 360]
    // protected float inputRotationY; // ]-80, 80[


    private void Awake()
    {
        // returns the first active loaded object of type Player
        Player = GetComponent<Player>();
    }


    private void Update()
    {
        Player.Input.JumpButton = false;
        Player.Input.PrimaryActionButton = false;
        Player.Input.SecondaryActionButton = false;
        Player.Input.TertiaryActionButton = false;
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {

        // feed calculated values to player object
        Player.Input.RunX = 1f;
        Player.Input.RunZ = 0f; 
        Player.Input.LookX = 0f; 
        Player.Input.LookZ = 0f; 
    }

}
