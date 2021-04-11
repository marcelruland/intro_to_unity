/*
 * Properties that only the playable character should have but not the NPCs.
 * - showing health, carried collectable, etc in UI
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Random = System.Random;


public class PlayableCharacter : MonoBehaviour
{
    Random random = new System.Random();
    private SoundEffectsManager _soundEffectsManager;
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


    private readonly Dictionary<string, string[]> _actionsWithCollectable = new Dictionary<string, string[]>
    {
        {"Banana", new[] {"", ""}},
        {"Disinfectant", new[] {"hoard", "drink"}},
        {"Flour", new[] {"hoard", ""}},
        {"Milk", new[] {"hoard", "drink"}},
        {"ToiletRoll", new[] {"hoard", ""}},
        {"Yeast", new[] {"hoard", ""}},
    };

    private readonly Dictionary<string, float> _collectableValues = new Dictionary<string, float>
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
        _soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            TakeInstantDamage();
        }
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
        const int colliderLayerMask = 1 << 6;
        // check for objects within radius
        Collider[] collectablesInRadius= Physics.OverlapSphere(Rigidbody.position, _DETECTION_RADIUS, colliderLayerMask);
        if (collectablesInRadius.Length == 0)
            return;

        Collider chosenCollectable = collectablesInRadius[random.Next(collectablesInRadius.Length - 1)];
        _carriedCollectable = chosenCollectable.tag;
        _secondaryAction = _actionsWithCollectable[_carriedCollectable][0];
        _tertiaryAction = _actionsWithCollectable[_carriedCollectable][1];
        
        Destroy(chosenCollectable.gameObject);
        
        // update UI
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
        GameManager.Instance.textPrimaryAction.text = "E: throw";
        GameManager.Instance.textSecondaryAction.text = "R: " + _secondaryAction;
        GameManager.Instance.textTertiaryAction.text = "T: " + _tertiaryAction;
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
        if (_secondaryAction == "hoard")
            DrinkCollectable();
        else
            throw new NotImplementedException();
    }

    private void DrinkCollectable()
    {
        if (_carriedCollectable == "Disinfectant")
        {
            _health = 0.1f;
            GameManager.Instance.Health = _health;
        }
        else if (_carriedCollectable == "Milk")
        {
            _soundEffectsManager.PlaySoundEffect(_soundEffectsManager.sfxDrink);
            StartCoroutine(TemporarilyIncreaseSpeed(3f, 1f));
        }

        IEnumerator TemporarilyIncreaseSpeed(float durationInSeconds, float amount)
        {
            _player.speed += amount;
            yield return new WaitForSeconds(durationInSeconds);
            _player.speed -= amount;
        }
    }


    private void ResetCollectableValues()
    {
        _carriedCollectable = "";
        _secondaryAction = "";
        _tertiaryAction = "";
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
        GameManager.Instance.textPrimaryAction.text = "E:";
        GameManager.Instance.textSecondaryAction.text = "R:";
        GameManager.Instance.textTertiaryAction.text = "T:";
    }

    private void HoardCollectable()
    {
        GameManager.Instance.MoneySpent += _collectableValues[_carriedCollectable];
        GameManager.Instance.Bounty[_carriedCollectable]++;
        _carriedCollectable = "";
        GameManager.Instance.CarriedCollectable = _carriedCollectable;
    }


    // make ReSharper shut up already
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private void CheckSafetyDistance()
    {
        int NPCLayerMask = 1 << 7;
        Collider[] NPCsInRadius = Physics.OverlapSphere(transform.position, _MINDESTABSTAND, NPCLayerMask);
        if (NPCsInRadius.Length > 0)
            TakeDamage();
    }

    private void TakeDamage(float amount = 0.1f)
    {
        _health -= amount * Time.deltaTime;
        GameManager.Instance.Health = _health;
    }
    
    private void TakeInstantDamage(float amount = 0.3f)
    {
        _health -= amount;
        GameManager.Instance.Health = _health;
    }
}