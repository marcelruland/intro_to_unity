

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalNonPlayableCharacter : MonoBehaviour
{
    // input
    protected Player Player;
    public Vector2[] walkingPoints;
    private int _nextPointIndex;

    // private Dictionary<Vector2, Vector2[]> pathWays = new Dictionary<Vector2, Vector2[]>
    // {
    //     {new Vector2(6, 4), new[]{new Vector2(10, 4), new Vector2(2, 4), new Vector2(6, 2), new Vector2(6, 6)}},
    // };

    private void Awake()
    {
        // returns the first active loaded object of type Player
        Player = GetComponent<Player>();
    }

    private void Start()
    {
        Player.Input.RunX = 1f;
        Player.Input.RunZ = 0f;
        _nextPointIndex = 1;
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
        UpdateWalkInput();
        // feed calculated values to player object
        // Player.Input.RunX = 1f;
        // Player.Input.RunZ = 0f; 
        // Player.Input.LookX = 0f; 
        // Player.Input.LookZ = 0f; 
    }

    private void UpdateWalkInput()
    {
        var position = transform.position;
        bool isAtPoint = (new Vector2(position.x, position.z) == walkingPoints[_nextPointIndex]);
        if (isAtPoint)
        {
            WalkToPoint(_nextPointIndex);
            _nextPointIndex++;
            if (_nextPointIndex == 4)
                _nextPointIndex = 0;
        }
    }

    private void WalkToPoint(float nextPointIndex)
    {
        // this doesn't work because for some reason the Player's position isn't changing? Whatever.
        Vector2 nextPoint = walkingPoints[_nextPointIndex];
        if (transform.position.x < nextPoint.x)
            Player.Input.RunX = 1f;
        else if (transform.position.x > nextPoint.x)
            Player.Input.RunX = -1f;
        else
            Player.Input.RunX = 0f;
        
        if (transform.position.z < nextPoint.y)
            Player.Input.RunX = 1f;
        else if (transform.position.z > nextPoint.y)
            Player.Input.RunX = -1f;
        else
            Player.Input.RunX = 0f;
    }
}
