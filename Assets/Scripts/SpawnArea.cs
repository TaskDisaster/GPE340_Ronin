using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private Bounds bounds;
    public Vector3 spawnArea;
    public Faction faction;

    // Awake is called before any other method
    void Awake()
    {
        bounds = new Bounds(transform.position, spawnArea);
    }

    private void OnDrawGizmos()
    {
        switch (faction)
        {
            case Faction.Friendly:
                Gizmos.color = Color.blue;
            break;

            case Faction.Hostile:
                Gizmos.color = Color.red;
            break;
        }

        Gizmos.DrawCube(transform.position, spawnArea);
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }

    public Vector3 GetRandomPointInArea()
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }
}
