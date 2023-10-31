using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,   //Game in progress
        Paused,    //Game paused
        PlayerWon, //The player has won
        PlayerLost, //The player has lost
        Credits
    }

    //Denotes the current game state
    public GameState currentGameState { get; private set; }

    //Events that fire when game changes to specific states
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameResumed;
    public UnityEvent OnGameLost;
    public UnityEvent OnGameWon;

    public static GameManager _instance = null;

    public int lives = 3;


    private void Awake()
    {
        #region Singleton
         if (_instance == null)
         {
           _instance = this;

           DontDestroyOnLoad(gameObject);
         }
        else
         {
             Destroy(gameObject);
        }
         #endregion
}

    // Start is called before the first frame update
    void Start()
    {

    }

// Update is called once per frame
    void Update()
    {

    }   

    public void TogglePause()
    {
         if (currentGameState == GameState.Playing)
         {
             PauseGame();
         }
         else if (currentGameState == GameState.Paused)
         {
             ResumeGame();
         }
    }

    public void PauseGame()
    {
        if (currentGameState == GameState.Paused) return;

        currentGameState = GameState.Paused;

        Debug.Log("Game Paused");

        Time.timeScale = 0.0f;


        OnGamePaused.Invoke();
    }

    public void ResumeGame()
    {
        if (currentGameState == GameState.Playing) return;

        currentGameState = GameState.Playing;

        Debug.Log("Game Resumed");

        Time.timeScale = 1.0f;

        OnGameResumed.Invoke();
    }

    public void LoseGame()
    {
        if (currentGameState == GameState.PlayerLost) return;

        currentGameState = GameState.PlayerLost;

        Time.timeScale = 0.0f;

        OnGameLost.Invoke();
    }

    public void WinGame()
    {

        if (currentGameState == GameState.PlayerWon) return;
        Debug.Log("Win");

        currentGameState = GameState.PlayerWon;

        Time.timeScale = 0.0f;

        OnGameWon.Invoke();
    }

    public void Credits()
    {

        if (currentGameState == GameState.Credits) return;

        Debug.Log("Credits");
        currentGameState = GameState.Credits;

        Time.timeScale = 0.0f;
    }
}
