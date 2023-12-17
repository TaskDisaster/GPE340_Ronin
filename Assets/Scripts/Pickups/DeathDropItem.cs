using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathDropItem : MonoBehaviour
{
    public DropTable dropTable;
    public List<float> densityArray;
    private float totalWeight = 0;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

        // Connect to Health Death
        GetComponent<Health>().OnDeath.AddListener(DropRandomItem);

        // Build our CDA
        BuildCDA();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildCDA()
    {
        foreach (DropEntry entry in dropTable.drops)
        {
            totalWeight += entry.weight;
            densityArray.Add(totalWeight);
        }

    }

    public GameObject GetRandomItem()
    {
        float randomNumber = Random.Range(0.0f, totalWeight);
        for (int i = 0; i < densityArray.Count; i++)
        {
            float weightMarker = densityArray[i];
            if (randomNumber < weightMarker)
            {
                return dropTable.drops[i].itemToDrop;
            }
        }
        return dropTable.drops[dropTable.drops.Count-1].itemToDrop;
    }

    public void DropRandomItem()
    {
        GameObject drop = GetRandomItem();
        if (drop != null)
        {
            Instantiate<GameObject>(drop, transform.position + offset, Quaternion.identity);
        }
    }
}

[System.Serializable]
public class DropTable
{
    public List<DropEntry> drops;
}

[System.Serializable]
public class DropEntry
{
    public GameObject itemToDrop;
    public int weight;
}
