using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Main variable himself
    public static GameManager Instance;

    [Header("Player Variables")]
    #region Player
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;
    public Pawn currentPlayer;
    #endregion

    [Header("Enemy Variables")]
    #region Enemy
    public GameObject enemyControllerPrefab;
    public GameObject enemyPawnPrefab;
    public List<Pawn> currentEnemyPawns = new List<Pawn>();
    #endregion

    [Header("Spawn Variables")]
    #region Spawn
    public SpawnManager spawnManager;
    #endregion

    [Header("Wave Variables")]
    #region Wave
    public ButtonSpawner buttonSpawner;
    public bool beginWaves = false;
    public int wave;
    public float secondsBetween;
    public float timeTillWave;
    public PlayerUIManager uiManager;
    #endregion

    [Header("Game States")]
    #region GameStates
    public GameObject TitleStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverStateObject;
    public GameObject CreditsStateObject;
    public GameObject PauseStateObject;
    public enum GameState { Title, MainMenu, Options, Credits, Gameplay, GameOver, Pause, Idle, Restart };
    public GameState gameState;
    public GameState previousState;
    public KeyCode transitionKey;
    public KeyCode pauseKey;
    public bool playerSpawned;
    public bool pausedGame;
    public bool playerDied;
    #endregion

    [Header("Misc Variables")]
    #region Misc
    public CameraController camController;
    #endregion

    // Awake is called before another other method
    private void Awake()
    {
        // Check if GameManager exists
        if (Instance == null)
        {
            // Create one if it doesn't
            Instance = this;
            // Make sure it's persisting through scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy multiples
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the player immediately if there's no TitleStateObject
        if (TitleStateObject != null)
        {
            // Sets to title screen first
            gameState = GameState.Title;
        }
        else
        {
            SpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleStateObject != null)
        {
            MakeDecision();
        }
    }

    public void MakeDecision()
    {
        // Game State FSM
        switch (gameState)
        {
            case GameState.Title:

                ActivateTitleState();

                // Transition to Main Menu
                if (Input.GetKey(transitionKey))
                {
                    ChangeState(GameState.MainMenu);
                }
                break;

            case GameState.MainMenu:

                ActivateMainMenuState();

                break;

            case GameState.Options:

                ActivateOptionsState();

                break;

            case GameState.Gameplay:

                // Resume Game if time is paused
                if (pausedGame == true)
                {
                    Time.timeScale = 1;
                    pausedGame = false;
                }

                ActivateGameplayState();

                // Spawn the Player once
                if (playerSpawned != true)
                {
                    SpawnPlayer();
                    playerSpawned = true;
                }

                // Check for transitions to Pause Menu
                if (Input.GetKeyDown(pauseKey))
                {
                    ChangeState(GameState.Pause);
                }

                // Check if waves have begun
                if (beginWaves == true)
                {            
                    // Check if all enemies are dead
                    if (currentEnemyPawns.Count <= 0)
                    {
                        // Update the timer every frame
                        timeTillWave -= Time.deltaTime;

                        // Time down until the next wave
                        if (timeTillWave <= 0)
                        {
                            StartNewWave();
                        }
                    }
                }

                break;

            case GameState.GameOver:

                ActivateGameOverState();


                // Transition to Main Menu
                if (Input.GetKey(transitionKey))
                {
                    ChangeState(GameState.Restart);
                }
                break;

            case GameState.Credits:

                ActivateCreditsState();

                break;

            case GameState.Pause:

                ActivatePauseState();

                // Pause Game
                if (pausedGame == false)
                {
                    Time.timeScale = 0;
                    pausedGame = true;
                }

                // Check for transitions
                if (Input.GetKeyDown(pauseKey))
                {
                    ChangeState(GameState.Gameplay);
                }

                break;

            case GameState.Idle:
                // Wait for transitions
                break;

                // Case will only play once, then switch to MainMenu
            case GameState.Restart:
                // Reset Wave Variables
                wave = 1;
                UpdateUI();
                beginWaves = false;
                timeTillWave = 0;

                // Reset Player Variables and Destroy Player
                if (currentPlayer != null)
                {
                    Destroy(currentPlayer.gameObject);
                }

                if (currentPlayer.controller != null)
                {
                    Destroy(currentPlayer.controller.gameObject);
                }

                currentPlayer = null;
                playerSpawned = false;

                // Reset Paused Variable
                pausedGame = false;
                Time.timeScale = 1;

                // Create a list to store enemies to be destroyed
                List<Pawn> enemiesToDestroy = new List<Pawn>();

                // Reset enemy variables and add enemies to the destroy list
                foreach (Pawn enemy in currentEnemyPawns)
                {
                    if (enemy != null)
                    {
                        enemiesToDestroy.Add(enemy);
                    }
                }

                // Destroy all enemies in the destroy list
                foreach (Pawn enemy in enemiesToDestroy)
                {
                    Destroy(enemy.controller.gameObject);
                    Destroy(enemy.gameObject);

                    // Remove the enemy from the currentEnemyPawns list (if needed)
                    currentEnemyPawns.Remove(enemy);
                }

                // Clear the main list
                currentEnemyPawns.Clear();

                // Respawn the Start Button
                buttonSpawner.OnSpawnButton.Invoke();

                ChangeState(GameState.MainMenu);
                break;
        }




    }

    #region Spawning

    /// <summary>
    /// Function that spawns and links the player pawn and controller
    /// </summary>
    public void SpawnPlayer()
    {
        // Check if there is a spawn manager
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found!");
            return;
        }

        // Get the lucky spawn
        SpawnArea luckySpawn = spawnManager.playerSpawn[Random.Range(0, spawnManager.playerSpawn.Count)];

        // Then spawn the player
        GameObject newControllerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(playerPawnPrefab, luckySpawn.GetRandomPointInArea(), Quaternion.identity);

        // Connect Controllers and Pawns
        Controller newController = newControllerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.controller = newController;

        // Set the current player
        currentPlayer = newPawn;

        // Set the camera's target to the player
        camController.target = newPawn.transform;
    }

    public void SpawnEnemy()
    {
        // Check if there is a spawn manager
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found!");
            return;
        }

        // Get the lucky spawn
        SpawnArea luckySpawn = spawnManager.enemySpawn[Random.Range(0, spawnManager.playerSpawn.Count)];

        // Then spawn the enemy
        GameObject newControllerObj = Instantiate(enemyControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(enemyPawnPrefab, luckySpawn.GetRandomPointInArea(), Quaternion.identity);

        // Connect Controllers and Pawns
        Controller newController = newControllerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.controller = newController;

        // Add it to the list of EnemyPawns
        currentEnemyPawns.Add(newPawn);

        // Cast the newController as an AIController
        AIController aiController = newController as AIController;

        // Set the target transform
        if (aiController != null)
        {
            aiController.targetTransform = currentPlayer.transform;
        }
    }
    #endregion

    #region Waves

    public void UpdateUI()
    {
        uiManager.waveCounter.text = wave.ToString();
    }

    public void SpawnWave()
    {
        Debug.Log("Wave: " + wave);
        for (int i = 0; i < wave; i++)
        {
            SpawnEnemy();
        }

        UpdateUI();
        IncrementWave();
    }

    public void IncrementWave()
    {
        wave += 1;
    }

    public void StartNewWave()
    {
        // Spawn Wave
        SpawnWave();
        Debug.Log("Starting new wave!");

        // Reset the timer for the next wave
        timeTillWave = secondsBetween;
    }

    #endregion

    #region Game States

    public void SetPreviousState(GameState newState)
    {
        // Change the previous state;
        previousState = newState;
    }

    public void ChangeState(GameState newState)
    {
        // Set the previous state
        SetPreviousState(gameState);

        // Change the current state
        gameState = newState;
    }

    #region Deactivate States

    private void DeactivateAllStates()
    {
        TitleStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverStateObject.SetActive(false);
        CreditsStateObject.SetActive(false);
        PauseStateObject.SetActive(false);
    }

    public void DeactivateTitleState()
    {
        TitleStateObject.SetActive(false);
    }

    public void DeactivateMainMenuState()
    {
        MainMenuStateObject.SetActive(false);
    }

    public void DeactivateOptionsState() 
    {
        OptionsStateObject.SetActive(false);
    }

    public void DeactivateGameplayState()
    {
        GameplayStateObject.SetActive(false);
    }

    public void DeactivateGameOverState()
    {
        GameOverStateObject.SetActive(false);
    }

    public void DeactivateCreditsState()
    {
        CreditsStateObject.SetActive(false);
    }

    public void DeactivatePauseState()
    {
        PauseStateObject.SetActive(false);
    }

    #endregion

    #region Activate States

    private void ActivateAllStates()
    {
        TitleStateObject.SetActive(true);
        MainMenuStateObject.SetActive(true);
        OptionsStateObject.SetActive(true);
        GameplayStateObject.SetActive(true);
        GameOverStateObject.SetActive(true);
        CreditsStateObject.SetActive(true);
        PauseStateObject.SetActive(true);
    }

    public void ActivateTitleState()
    {
        DeactivateAllStates();

        gameState = GameState.Title;

        TitleStateObject.SetActive(true);
    }

    public void ActivateMainMenuState()
    {
        DeactivateAllStates();

        gameState = GameState.MainMenu;

        MainMenuStateObject.SetActive(true);
    }

    public void ActivateOptionsState()
    {
        DeactivateAllStates();

        gameState = GameState.Options;

        OptionsStateObject.SetActive(true);
    }

    public void ActivateGameplayState()
    {
        DeactivateAllStates();

        gameState = GameState.Gameplay;

        GameplayStateObject.SetActive(true);
    }

    public void ActivateGameOverState()
    {
        DeactivateAllStates();

        gameState = GameState.GameOver;

        GameOverStateObject.SetActive(true);
    }

    public void ActivateCreditsState()
    {
        DeactivateAllStates();

        gameState = GameState.Credits;

        CreditsStateObject.SetActive(true);
    }

    public void ActivatePauseState()
    {
        DeactivateAllStates();

        gameState = GameState.Pause;

        PauseStateObject.SetActive(true);
    }

    public void ActivateRestartState()
    {
        DeactivateAllStates();

        gameState = GameState.Restart;
    }

    #endregion

    #endregion
}
