using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] public static ObjectPooler SharedInstance;
    [SerializeField] public List<GameObject> pooledMissiles;
    [SerializeField] public GameObject missileToPool;
    [SerializeField] public int amountToPool;

    private GameObject missile;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeObjects();
    }

    void InitializeObjects()
    {
        // Loop through list of pooled objects, deactivating them and adding them to the list 
        pooledMissiles = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            missile = (GameObject)Instantiate(missileToPool);
            missile.SetActive(false);
            pooledMissiles.Add(missile);
            missile.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject()
    {
        // For as many objects that are in the pooledMissiles list
        for (int i = 0; i < pooledMissiles.Count; i++)
        {
            // if the pooled object is NOT active, return that object 
            if (!pooledMissiles[i].activeInHierarchy)
            {
                return pooledMissiles[i];
            }
        }
        // otherwise, return null   
        return null;
    }

}
