

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    // input
    protected Player Player;
    public Vector2[] walkingPoints;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    private void Start()
    {
        Player.Input.RunX = 0f;
        Player.Input.RunZ = 0f;
        WalkToPoint(new Vector2(30, 5));
    }


    private void Update()
    {
        Player.Input.JumpButton = false;
        Player.Input.PrimaryActionButton = false;
        Player.Input.SecondaryActionButton = false;
        Player.Input.TertiaryActionButton = false;
        if (IsAtPoint(new Vector2(30, 5)))
        {
            Player.Input.RunX = 0f;
            Player.Input.RunZ = 0f;
        }
    }

    // FixedUpdate is called once every physics update
    private void FixedUpdate()
    {
        // feed calculated values to player object
        // Player.Input.RunX = 1f;
        // Player.Input.RunZ = 0f; 
        // Player.Input.LookX = 0f; 
        // Player.Input.LookZ = 0f; 
    }


    private bool IsAtPoint(Vector2 point)
    {
        float tolerance = 0.1f;
        var position = new Vector2(transform.position.x, transform.position.z);
        bool isAtX = position.x > point.x - tolerance && position.x < point.x + tolerance;
        bool isAtZ = position.y > point.y - tolerance && position.y < point.y + tolerance;
        if (isAtX && isAtZ)
            return true;
        return false;
    }

    private void WalkToPoint(Vector2 point)
    {
        var position = new Vector2(transform.position.x, transform.position.z);
        var walkingDirection = point - position;
        Player.Input.RunX = walkingDirection.x;
        Player.Input.RunZ = walkingDirection.y;
    }
}
