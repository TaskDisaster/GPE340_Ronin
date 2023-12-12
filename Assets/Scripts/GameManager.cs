using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
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
    public GameObject currentPlayer;
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
    public bool beginWaves = false;
    public int wave;
    public float secondsBetween;
    public float timeTillWave; 
    #endregion

    [Header("Misc Variables")]
    #region Misc
    public CameraController camController;
    #endregion

    [Header("Unity Events")]
    #region Events
    public UnityEvent OnGameStart;
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
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MakeDecision();
    }

    public void MakeDecision()
    {
        // Check if waves have begun
        if (beginWaves != true)
        {            
            return;
        }

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
        currentPlayer = newPawnObj;

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

    public void SpawnWave()
    {
        Debug.Log("Wave: " + wave);
        for (int i = 0; i < wave; i++)
        {
            SpawnEnemy();
        }

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
}
