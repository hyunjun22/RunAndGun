using UnityEngine;
using System.Collections.Generic;


public class ZombiePool : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] int poolSize = 20;

    List<GameObject> zombiePoolList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            zombiePoolList.Add(zombie);
        }
    }

    public GameObject GetZombie()
    {
        foreach (GameObject zombie in zombiePoolList)
        {
            if (!zombie.activeInHierarchy)
            {
                return zombie;
            }
        }

        return null;
    }
}
