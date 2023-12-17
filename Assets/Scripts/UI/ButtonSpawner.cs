using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSpawner : MonoBehaviour
{
    [Header("Button Variables")]
    public UnityEvent OnSpawnButton;
    public GameObject buttonPrefab;
    public GameObject currentButton;

    public void SpawnStartButton()
    {
        if (currentButton == null)
        {
            currentButton = Instantiate(buttonPrefab, transform.position, Quaternion.identity);
        }
    }
}
