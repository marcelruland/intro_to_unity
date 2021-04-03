/*
 * GAME MANAGER
 * Central script that manages the game (who would've thought?).
 * It manages the UI, keeps track of metrics like the player score, hoarded
 * objects etc, and manages the state of the game.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     * VARIABLES
     */
    public static GameManager Instance { get; private set; }
    public GameObject[] levels;
    GameObject _currentLevel;

    // Prefabs
    public GameObject prefabPlayer;

    public GameObject prefabBanana;
    //public GameObject prefabDisinfectant;  // not yet implemented
    public GameObject prefabFlour;
    public GameObject prefabMilk;
    public GameObject prefabToiletRoll;
    public GameObject prefabYeast;

    // User Interface
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    // get and set stuff
    private int _level;
    public int Level
    {
        get { return _level; }
        set { _level = value;
            //levelText.text = "LEVEL: " + _level;
        }
    }
    private int _score;
    public int Score
    {
        get { return _score; }
        set { _score = value;
            //scoreText.text = "SCORE: " + _score;
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
        INITIALIZE,      // initialise game, remove leftovers from previous
                         // levels, set cursor visibility, etc
        LOADLEVEL,       // load a level
        PLAY,            // level is being played
        LEVELCOMPLETED,  // level is completed, summary screen, then switch to
                         // load next level
        GAMEOVER         // you're done
    }

    private State _state;
    private bool _isSwitchingState;

    public void SwitchState(State newState, float delay = 0)
    {
        Debug.Log("Old " + _state.ToString() + "   New: " + newState.ToString());
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
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
    void Start()
    {
        // make accessing this script easier
        Instance = this;
        // begin in the (main) menu
        SwitchState(State.MENU);
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMenu.SetActive(true);
                SwitchState(State.LOADLEVEL, 3f);
                break;
            case State.INITIALIZE:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                _currentLevel = Instantiate(levels[Level]);
                SwitchState(State.PLAY);
                break;
            case State.GAMEOVER:
                break;
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INITIALIZE:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INITIALIZE:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                break;
        }
    }

}
