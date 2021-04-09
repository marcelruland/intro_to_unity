/*
 * Properties that only NPCs should have but not the playable character.
 * - directly spawning/throwing collectables without picking anything up
 * 
 * NOTE: This script combines both properties and movement input for NPC's.
 * The corresponding scripts for the playable character are PlayableCharacter.cs
 * (properties) and Controller.cs (movement) respectively.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    private Player _player;
    public Vector2[] walkingPoints;
    private int _numberOfWalkingPoints;
    private int _currentPointIndex = 0;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.ActionInput.Jump = false;
        _player.MovementInput.RunX = 0f;
        _player.MovementInput.RunZ = 0f;
        
        _numberOfWalkingPoints = walkingPoints.Length;
    }


    private void Update()
    {
        WalkToPoint(walkingPoints[_currentPointIndex]);
        if (IsAtPoint(walkingPoints[_currentPointIndex]))
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _numberOfWalkingPoints)
                _currentPointIndex = 0;
        }
    }


    private bool IsAtPoint(Vector2 point, float tolerance = 0.1f)
    {
        Vector2 position2d = new Vector2(transform.position.x, transform.position.z);
        return Vector2.Distance(position2d, point) <= tolerance;
    }

    private void WalkToPoint(Vector2 point)
    {
        // awkward but fast(-er than before) access
        Vector3 position3d = transform.position;
        Vector2 position2d = new Vector2(position3d.x, position3d.z);
        Vector2 walkingDirection = CalculateNormalizedDirection(point, position2d);
        
        _player.MovementInput.RunX = walkingDirection.x;
        _player.MovementInput.RunZ = walkingDirection.y;
        _player.MovementInput.LookX = walkingDirection.x;
        _player.MovementInput.LookZ = walkingDirection.y;
    }


    private Vector2 CalculateNormalizedDirection(Vector2 a, Vector2 b)
    {
        // calculates directional Vector2 FROM a TO b
        return (a - b).normalized;
    }

}
