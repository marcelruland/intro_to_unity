/*
 * Properties that only the playable character should have but not the NPCs.
 * - showing health, carried collectable, etc in UI
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class PlayableCharacter : MonoBehaviour
{
    Random random = new System.Random();
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
    private const float _MINDESTABSTAND = 1.5f;
    private float _health = 1.0f;

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
        GameManager.Instance.Health = _health;
        TakeDamage();
    }

    void Update()
    {
        bool carriesNothing = _carriedCollectable == "";
        if (carriesNothing)
            PickUpCollectable();
        else
            PerformActionWithCollectable();
        CheckSafetyDistance();
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

        // see https://docs.unity3d.com/Manual/Layers.html section "Casting
        // Rays Selectively" for an explanation of how this works
        int colliderLayerMask = 1 << 6;
        // check for objects within radius
        Collider[] collectablesInRadius= Physics.OverlapSphere(Rigidbody.position, _DETECTION_RADIUS, colliderLayerMask);

        Collider chosenCollectable = collectablesInRadius[random.Next(collectablesInRadius.Length)];
        _carriedCollectable = chosenCollectable.tag;
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
        Destroy(chosenCollectable.gameObject);
        _secondaryAction = _actionsWithCollectable[_carriedCollectable][0];
        _tertiaryAction = _actionsWithCollectable[_carriedCollectable][1];
        
        //iterate over found objects
        // foreach (Collider objectInRadius in objectsInRadius)
        // {
        //     // if one of the objects within radius is a collectable
        //
        //     bool isCollectable = Array.Exists(_collectables, element => element == objectInRadius.tag);
        //     if (isCollectable)
        //     {
        //         // write tag to carriedObject variable and destroy gameObject
        //         _carriedCollectable = objectInRadius.tag;
        //         GameManager.Instance.CarriedCollectable = _carriedCollectable;
        //         Destroy(objectInRadius.gameObject);
        //         _secondaryAction = _actionsWithCollectable[_carriedCollectable][0];
        //         _tertiaryAction = _actionsWithCollectable[_carriedCollectable][1];
        //         break;
        //     }
        //
        //     ;
        // }
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


    private void CheckSafetyDistance()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            bool isOtherPlayer = false;
            if (isOtherPlayer)
                TakeDamage();
        }
    }

    private void TakeDamage(float amount = 0.1f)
    {
        _health -= amount;
        GameManager.Instance.Health = _health;
    }
}