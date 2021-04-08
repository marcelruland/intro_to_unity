

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    // input
    protected Player Player;
    public Vector2[] walkingPoints;
    private int _numberOfWalkingPoints;
    private int _currentPointIndex = 0;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    private void Start()
    {
        Player.Input.JumpButton = false;
        Player.Input.PrimaryActionButton = false;
        Player.Input.SecondaryActionButton = false;
        Player.Input.TertiaryActionButton = false;
        Player.Input.RunX = 0f;
        Player.Input.RunZ = 0f;
        
        _numberOfWalkingPoints = walkingPoints.Length;
        WalkToPoint(walkingPoints[_currentPointIndex]);
    }


    private void Update()
    {
        if (IsAtPoint(walkingPoints[_currentPointIndex]))
            WalkToNextPoint();
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

    private void WalkToNextPoint()
    {
        _currentPointIndex++;
        if (_currentPointIndex >= _numberOfWalkingPoints)
            _currentPointIndex = 0;
        WalkToPoint(walkingPoints[_currentPointIndex]);
    }
}
