using UnityEngine;
using System.Collections.Generic;


public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] int zombiePoolSize = 20;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize = 100;


    List<GameObject> zombiePoolList = new List<GameObject>();
    List<GameObject> bulletPoolList = new List<GameObject>();


    void Start()
    {
        SimpleSetting(zombiePrefab, zombiePoolSize, zombiePoolList);
        SimpleSetting(bulletPrefab, bulletPoolSize, bulletPoolList);
    }


    // 기초 세팅 간편화
    void SimpleSetting(GameObject prefab, int PoolSize, List<GameObject> objList)
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // 오브젝트 생성할 때 자식으로 설정함
            obj.SetActive(false);
            objList.Add(obj);
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

    public GameObject GetBullet()
    {
        foreach(GameObject bullet in bulletPoolList)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }
}
