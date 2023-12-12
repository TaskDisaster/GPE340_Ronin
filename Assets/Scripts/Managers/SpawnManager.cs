using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class SpawnManager : MonoBehaviour
{
    public List<SpawnArea> playerSpawn = new List<SpawnArea>();
    public List<SpawnArea> enemySpawn = new List<SpawnArea>();

    private void Start()
    {
        FindAllEnemySpawnAreas();
    }

    /// <summary>
    /// Finds all player spawns and adds it to the list
    /// </summary>
    public void FindAllPlayerSpawnAreas()
    {
        SpawnArea[] spawnAreas = FindObjectsByType<SpawnArea>(FindObjectsSortMode.None);
        foreach (SpawnArea playerArea in spawnAreas)
        {
            // Sort for the correct spawn area
            if (playerArea.faction == Faction.Friendly)
            {
                playerSpawn.Add(playerArea);
            }
        }
    }

    /// <summary>
    /// Finds all enemy spawns and adds it to the list
    /// </summary>
    public void FindAllEnemySpawnAreas()
    {
        SpawnArea[] spawnAreas = FindObjectsByType<SpawnArea>(FindObjectsSortMode.None);
        foreach (SpawnArea enemyArea in spawnAreas)
        {
            // Sort for the correct spawn area
            if (enemyArea.faction == Faction.Hostile)
            {
                enemySpawn.Add(enemyArea);
            }
        }
    }
}
