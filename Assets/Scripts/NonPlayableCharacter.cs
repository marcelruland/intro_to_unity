

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : MonoBehaviour
{
    // input
    protected Player Player;
    public Vector2[] walkingPoints;

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
        // if (transform.position == walkingPoints[0])
        // {
        //     
        // }
    }

}
