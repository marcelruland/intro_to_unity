/*
 * GAME MANAGER
 * Central script that manages the game (who would've thought?).
 * It manages the UI, keeps track of metrics like the player score, hoarded
 * objects etc, and manages the state of the game.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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
    [NonSerialized] public readonly float timePerRound = 50f;
    private float _timeRemaining;
    
    public Dictionary<string, int> Bounty = new Dictionary<string, int>()
    {
        {"Banana", 0},
        {"Flour", 0},
        {"Milk", 0},
        {"ToiletRoll", 0},
        {"Yeast", 0},
    };


    // properties
    private float _health = 1f;

    public float Health
    {
        get { return _health; }
        set
        {
            if (_health <= 0f)
                SwitchState(State.GAMEOVER);
            _health = value;
            textHealth.text = "Health: " + _health.ToString("0.00");
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }

    private float _moneySpent;

    public float MoneySpent
    {
        get { return _moneySpent; }
        set
        {
            _moneySpent = value;
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

    // User Interface
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;
    public GameObject panelScore;
    public GameObject panelManual;

    public Text textCountDown;
    public Text textCarriedCollectable;
    public Text textMoneySpent;
    public Text textLevelcompletedSummary;
    public Text textHealth;
    public Text textPrimaryAction;
    public Text textSecondaryAction;
    public Text textTertiaryAction;

    public Button buttonMenuPlay;
    public Button buttonGameOverPlayAgain;
    public Button buttonReplayLevel;
    public Button buttonHowto;
    public Button buttonBack;
    public Button buttonBackMenuHowTo;


    /*
     * GAME STATES
     * The "sections" of a game are represented by different game states.
     * Depending on the current state, and whenever one state ends and another
     * state begins, the GameManager performs certain actions..
     */
    public enum State
    {
        MENU,
        INITIALIZE, // prepare loading of new level
        LOADLEVEL, // load a level
        PLAY, // level is being played
        LEVELCOMPLETED, // level is completed, summary screen, then switch to

        // load next level
        GAMEOVER, // you're done
        END, // you made it
    }

    private State _state;
    private bool _isSwitchingState;
    private bool _countDownRunning = false;

    public void SwitchState(State newState, float delay = 0f)
    {
        Debug.Log(_state + " --> " + newState);
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
        // initialise buttons
        buttonMenuPlay.onClick.AddListener(delegate { SwitchState(State.INITIALIZE); });
        buttonGameOverPlayAgain.onClick.AddListener(delegate { SwitchState(State.INITIALIZE); });
        buttonReplayLevel.onClick.AddListener(delegate { SwitchState(State.INITIALIZE); });
        //buttonHowto.onClick.;
        buttonBack.onClick.AddListener(delegate { SwitchState(State.LEVELCOMPLETED); });
        buttonBackMenuHowTo.onClick.AddListener(delegate { SwitchState(State.MENU); });
        
        // make accessing this script easier
        Instance = this;
        SwitchState(State.MENU);
    }

    private void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMenu.SetActive(true);
                panelScore.SetActive(false);
                panelManual.SetActive(false);
                break;
            case State.INITIALIZE:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                textCountDown.text = "";
                if (_currentLevel != null)
                    Destroy(_currentLevel);
                SwitchState(State.LOADLEVEL);
                MoneySpent=0.00f;
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                textLevelcompletedSummary.text =
                    "Du Hast für " + _moneySpent.ToString() + "€ gehamstert.\n" +
                    "Gemessen am Bundesdruchschnitt hast du\n" +
                    "- Klopapier" + BountyToLifeTime((float) 134 / 365, Bounty["ToiletRoll"]) +
                    "- Mehl" + BountyToLifeTime((float) 70.6 / 365, Bounty["Yeast"]) +
                    "- Hefe" + BountyToLifeTime((float) 2 / 365, Bounty["Flour"]);
                    // + "- Desinfektionsmittel" + BountyToLifeTime((float)1.5, Bounty["Disinfectant"]);
                panelLevelCompleted.SetActive(true);
                panelScore.SetActive(false);
                break;
            case State.LOADLEVEL:
                _currentLevel = Instantiate(levels[Level]);
                SwitchState(State.PLAY);
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(true);
                break;
            case State.END:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
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
            case State.END:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
                panelLevelCompleted.SetActive(false);
                break;
            case State.PLAY:
                Destroy(_currentLevel);
                panelPlay.SetActive(false);
                Cursor.visible = true;
                _health = 1f;
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(true);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(false);
                break;
            case State.END:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void InitiateCountdown(float initialTime)
    {
        _timeRemaining = initialTime;
        _countDownRunning = true;
    }

    private void UpdateCountdown()
    {
        if (!_countDownRunning)
            return;
        // set timer to zero once time is up, else count down
        if (_timeRemaining == 0f)
            SwitchState(State.LEVELCOMPLETED);
        else if (_timeRemaining <= 0)
        {
            SwitchState(State.GAMEOVER);
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
    
    private string BountyToLifeTime(float avgDailyUse, int numCollected)
    {
        int daysOfUse = Convert.ToInt32(numCollected / avgDailyUse);
        int monthsOfUse = daysOfUse / 30;
        int weeksOfUse = daysOfUse % 30 / 7;
        daysOfUse = daysOfUse % 30 % 7;
        return " für " + monthsOfUse + " Monate, " + weeksOfUse + " Wochen und " + daysOfUse + " Tage\n";
    }
}
