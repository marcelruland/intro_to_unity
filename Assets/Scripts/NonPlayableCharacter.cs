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
using Random = System.Random;

public class NonPlayableCharacter : MonoBehaviour
{
    private Player _player;
    public Vector2[] walkingPoints;
    private int _numberOfWalkingPoints;
    private int _currentPointIndex = 0;
    private const int DETECTION_RADIUS = 5;
    private bool _pcFound;
    private Random _rand = new Random();


    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.speed = RandomFromDistribution.RandomNormalDistribution(4f, 1f);
    }

    private void Start()
    {
        _player.ActionInput.Jump = false;
        _player.MovementInput.RunX = 0f;
        _player.MovementInput.RunZ = 0f;
        _numberOfWalkingPoints = walkingPoints.Length;

        InvokeRepeating(nameof(ThrowToiletRoll), 1f, 1f);
    }


    private void Update()
    {
        Vector2 walkingDirection = DirectionToPoint(walkingPoints[_currentPointIndex]);
        _player.MovementInput.RunX = walkingDirection.x;
        _player.MovementInput.RunZ = walkingDirection.y;

        Collider[] pcArr = Physics.OverlapSphere(transform.position, DETECTION_RADIUS, 1 << 8);
        _pcFound = pcArr.Length != 0;
        if (_pcFound)
        {
            var pc = pcArr[0];
            var pcPosition = pc.transform.position;
            var directionToPc = DirectionToPoint(new Vector2(pcPosition.x, pcPosition.z));

            _player.MovementInput.LookX = directionToPc.x;
            _player.MovementInput.LookZ = directionToPc.y;
        }
        else
        {
            _player.MovementInput.LookX = walkingDirection.x;
            _player.MovementInput.LookZ = walkingDirection.y;
        }


        if (IsAtPoint(walkingPoints[_currentPointIndex]))
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _numberOfWalkingPoints)
                _currentPointIndex = 0;
        }
    }

    private void ThrowToiletRoll()
    {
        if (!_pcFound) return;
        string[] arsenal = new string[]
        {
            "Banana",
            "Flour",
            "Milk",
            "ToiletRoll",
            "Yeast",
        };

        int choice = _rand.Next(5);
        _player.ActionInput.ThrownCollectable = "NpcWeapons/" + arsenal[choice] + "NpcWeapon";
        _player.ActionInput.Throw = true;
        StartCoroutine(SetThrowToFalse());

        IEnumerator SetThrowToFalse()
        {
            yield return null;
            _player.ActionInput.Throw = false;
        }
    }


    private bool IsAtPoint(Vector2 point, float tolerance = 0.1f)
    {
        Vector2 position2d = new Vector2(transform.position.x, transform.position.z);
        return Vector2.Distance(position2d, point) <= tolerance;
    }

    private Vector2 DirectionToPoint(Vector2 point)
    {
        // awkward but fast(-er than before) access
        Vector3 position3d = transform.position;
        Vector2 position2d = new Vector2(position3d.x, position3d.z);
        return CalculateNormalizedDirection(point, position2d);
    }


    private Vector2 CalculateNormalizedDirection(Vector2 a, Vector2 b)
    {
        // calculates directional Vector2 FROM a TO b
        return (a - b).normalized;
    }
    
}