/*
 * Properties that only the playable character should have but not the NPCs.
 * - showing health, carried collectable, etc in UI
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayableCharacter : MonoBehaviour
{
    private Player _player;
    protected Rigidbody Rigidbody;
    public InputStr Input;
    public struct InputStr
    {
        public bool PrimaryActionButton;
        public bool SecondaryActionButton;
        public bool TertiaryActionButton;
    }
    
    // Collectable and actions related
    private string _carriedCollectable;
    private string _secondaryAction;
    private string _tertiaryAction;
    private const float _DETECTION_RADIUS = 2f;

    private readonly string[] _collectables = new string[]
    {
        "Banana",
        //"Disinfectant",
        "Flour",
        "Milk",
        "ToiletRoll",
        "Yeast",
    };

    private readonly Dictionary<string, string[]> _actionsWithCollectable = new Dictionary<string, string[]>
    {
        {"Banana", new string[] {"", ""}},
        {"Disinfectant", new string[] {"hoard", "drink"}},
        {"Flour", new string[] {"hoard", ""}},
        {"Milk", new string[] {"hoard", "drink"}},
        {"ToiletRoll", new string[] {"hoard", ""}},
        {"Yeast", new string[] {"hoard", ""}},
    };

    private Dictionary<string, float> collectableValues = new Dictionary<string, float>
    {
        {"Banana", 0.40f},
        {"Disinfectant", 1.99f},
        {"Flour", 0.79f},
        {"Milk", 1.09f},
        {"ToiletRoll", 0.25f},
        {"Yeast", 0.49f},
    };

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }
    
    void Start()
    {
        ResetCollectableValues();
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
    }

    void Update()
    {
        bool carriesNothing = _carriedCollectable == "";
        if (carriesNothing)
            PickUpCollectable();
        else
            PerformActionWithCollectable();
    }


    private void PickUpCollectable()
    {
        /*
         * 1. find closest Collectable
         * 2. destroy it
         * 3. put value into carriedObject variable
         */
        if (!Input.PrimaryActionButton)
            return;

        // check for objects within radius
        Collider[] objectsInRadius = Physics.OverlapSphere(Rigidbody.position, _DETECTION_RADIUS);

        //iterate over found objects
        foreach (Collider objectInRadius in objectsInRadius)
        {
            // if one of the objects within radius is a collectable

            bool isCollectable = Array.Exists(_collectables, element => element == objectInRadius.tag);
            if (isCollectable)
            {
                // write tag to carriedObject variable and destroy gameObject
                _carriedCollectable = objectInRadius.tag;
                GameManager.Instance.CarriedCollectable = _carriedCollectable;
                Destroy(objectInRadius.gameObject);
                _secondaryAction = _actionsWithCollectable[_carriedCollectable][0];
                _tertiaryAction = _actionsWithCollectable[_carriedCollectable][1];
                break;
            }

            ;
        }
    }


    private void PerformActionWithCollectable()
    {
        if (Input.PrimaryActionButton)
        {
            ThrowCollectable();
            ResetCollectableValues();
        }
        else if (Input.SecondaryActionButton)
        {
            PerformSecondaryAction();
            ResetCollectableValues();
        }
        else if (Input.TertiaryActionButton)
        {
            PerformTertiaryAction();
            ResetCollectableValues();
        }
    }

    private void ThrowCollectable()
    {
        _player.ActionInput.ThrownCollectable = _carriedCollectable;
        _player.ActionInput.Throw = true;
        StartCoroutine(SetThrowToFalse());
        ResetCollectableValues();
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
        
        IEnumerator SetThrowToFalse()
        {
            yield return null;
            _player.ActionInput.Throw = false;
            
        }
    }


    private void PerformSecondaryAction()
    {
        if (_secondaryAction == "hoard")
            HoardCollectable();
        else
            throw new NotImplementedException();
    }

    private void PerformTertiaryAction()
    {
        throw new NotImplementedException();
    }


    private void ResetCollectableValues()
    {
        _carriedCollectable = "";
        _secondaryAction = "";
        _tertiaryAction = "";
    }

    private void HoardCollectable()
    {
        GameManager.Instance.MoneySpent += collectableValues[_carriedCollectable];
        _carriedCollectable = "";
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
    }


}