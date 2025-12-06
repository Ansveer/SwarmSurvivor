using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoints)
        {
            GameObject item = propPrefabs[0];
            GameObject prop = Instantiate(item, sp.transform.position, Quaternion.identity);
            prop.transform.parent = sp.transform;
        }
    }
}
