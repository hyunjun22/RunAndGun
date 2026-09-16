using UnityEngine;
using System.Collections.Generic;


public class ZombiePool : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] int poolSize = 20;

    List<GameObject> zombiePool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            zombiePool.Add(zombie);
        }
    }

    public GameObject GetZombie()
    {
        foreach (GameObject zombie in zombiePool)
        {
            if (!zombie.activeInHierarchy)
            {
                return zombie;
            }
        }
        
        return null;
    }
}
