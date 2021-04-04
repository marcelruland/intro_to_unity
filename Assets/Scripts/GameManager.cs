/*
 * GAME MANAGER
 * Central script that manages the game (who would've thought?).
 * It manages the UI, keeps track of metrics like the player score, hoarded
 * objects etc, and manages the state of the game.
 */


using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
     * VARIABLES
     */
    public static GameManager Instance { get; private set; }
    public GameObject[] levels;
    private GameObject _currentLevel;
    private float _timeRemaining;

    // Prefabs
    public GameObject prefabPlayer;

    public GameObject prefabBanana;
    //public GameObject prefabDisinfectant;  // TODO not yet implemented
    public GameObject prefabFlour;
    public GameObject prefabMilk;
    public GameObject prefabToiletRoll;
    public GameObject prefabYeast;

    // User Interface
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public Text textCountDown;
    public Text textCarriedCollectable;
    public Text textMoneySpent;

    // properties
    private int _level;
    public int Level
    {
        get { return _level; }
        set { _level = value;
        }
    }
    private int _moneySpent;
    public int MoneySpent
    {
        get { return _moneySpent; }
        set { _moneySpent = value;
            textMoneySpent.text = _moneySpent.ToString("0.00") + " €";
        }
    }

    private string _carriedCollectable;

    public string CarriedCollectable
    {
        get { return _carriedCollectable; }
        set
        {
            _carriedCollectable = value;
            textCarriedCollectable.text = _carriedCollectable;
        }
    }

    /*
     * GAME STATES
     * The "sections" of a game are represented by different game states.
     * Depending on the current state, and whenever one state ends and another
     * state begins, the GameManager performs certain actions.
     */
    public enum State
    {
        MENU,
        INITIALIZE,      // prepare loading of new level
        LOADLEVEL,       // load a level
        PLAY,            // level is being played
        LEVELCOMPLETED,  // level is completed, summary screen, then switch to
                         // load next level
        GAMEOVER,        // you're done
        END,             // you made it
    }

    private State _state;
    private bool _isSwitchingState;

    private void SwitchState(State newState, float delay = 0)
    {
        Debug.Log(_state.ToString() + " --> " + newState.ToString());
        StartCoroutine(SwitchDelay(newState, delay));
    }

    private IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }



    /*
     * METHODS
     */
    private void Start()
    {
        // make accessing this script easier
        Instance = this;
        // begin in the (main) menu
        SwitchState(State.MENU);
    }

    private void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMenu.SetActive(true);
                SwitchState(State.INITIALIZE, 3f);
                break;
            case State.INITIALIZE:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                if (_currentLevel != null)
                    Destroy(_currentLevel);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(true);
                break;
            case State.LOADLEVEL:
                _currentLevel = Instantiate(levels[Level]);
                InitiateCountdown(45f);
                SwitchState(State.PLAY);
                break;
            case State.GAMEOVER:
                break;
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INITIALIZE:
                break;
            case State.PLAY:
                UpdateCountdown();
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                break;
        }
    }

    private void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INITIALIZE:
                break;
            case State.PLAY:
                Destroy(_currentLevel);
                panelPlay.SetActive(false);
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                break;
        }
    }

    private void InitiateCountdown(float initialTime)
    {
        _timeRemaining = initialTime;
    }

    private void UpdateCountdown()
    {
        // set timer to zero once time is up, else count down
        if (_timeRemaining == 0f)
            SwitchState(State.LEVELCOMPLETED);
        else if (_timeRemaining <= 0)
        {
            _timeRemaining = 0f;
        }
        else
        {
            _timeRemaining -= Time.deltaTime;
        }

        textCountDown.text = _timeRemaining.ToString("0.0");

        // COLOUR
        // this can definitely by optimised, but idc right now
        if (_timeRemaining > 30)
            textCountDown.color = Color.black;
        else if (_timeRemaining > 20)
            textCountDown.color = Color.yellow;
        else if (_timeRemaining > 10)
            textCountDown.color = new Color(1, 0.65f, 0);
        else
            textCountDown.color = Color.red;
    }
}
