using UnityEngine;
using System.Collections.Generic;


public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] int zombiePoolSize = 20;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletPoolSize = 100;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int explosionPoolSize = 100;
    [SerializeField] GameObject skeletonPrefab;
    [SerializeField, Min(0)] int skeletonPoolSize = 10;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField, Min(0)] int arrowPoolSize = 40;


    List<GameObject> zombiePoolList = new List<GameObject>();
    List<GameObject> bulletPoolList = new List<GameObject>();
    List<GameObject> explosionPoolList = new List<GameObject>();
    readonly List<GameObject> skeletonPoolList = new List<GameObject>();
    readonly List<GameObject> arrowPoolList = new List<GameObject>();
    Transform inactiveRoot;

    void Awake()
    {
        var root = new GameObject("Inactive Pool Creation");
        root.SetActive(false);
        root.transform.SetParent(transform, false);
        inactiveRoot = root.transform;
    }


    void Start()
    {
        SimpleSetting(zombiePrefab, zombiePoolSize, zombiePoolList);
        SimpleSetting(bulletPrefab, bulletPoolSize, bulletPoolList);
        SimpleSetting(explosionPrefab, explosionPoolSize, explosionPoolList);
        SimpleSetting(skeletonPrefab, skeletonPoolSize, skeletonPoolList);
        SimpleSetting(arrowPrefab, arrowPoolSize, arrowPoolList);
    }


    void SimpleSetting(GameObject prefab, int PoolSize, List<GameObject> objList)
    {
        if (prefab == null) return;
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject obj = CreatePooled(prefab);
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

        GameObject newBullet = CreatePooled(bulletPrefab);
        bulletPoolList.Add(newBullet);
        return newBullet;
    }

    public void PlayExplosion(Vector3 position)
    {
        if (explosionPrefab == null) return;
        GameObject effect = null;
        foreach (GameObject candidate in explosionPoolList)
        {
            if (!candidate.activeInHierarchy)
            {
                effect = candidate;
                break;
            }
        }
        if (effect == null)
        {
            effect = CreatePooled(explosionPrefab);
            explosionPoolList.Add(effect);
        }
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.identity;
        effect.SetActive(true);
    }

    GameObject CreatePooled(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, inactiveRoot);
        obj.SetActive(false);
        obj.transform.SetParent(transform, false);
        return obj;
    }

    GameObject GetAvailable(GameObject prefab, List<GameObject> list)
    {
        if (prefab == null) return null;
        foreach (GameObject obj in list) if (!obj.activeSelf) return obj;
        GameObject created = CreatePooled(prefab);
        list.Add(created);
        return created;
    }

    public GameObject GetSkeleton()
    {
        GameObject obj = GetAvailable(skeletonPrefab, skeletonPoolList);
        if (obj != null) obj.GetComponent<Skeleton>().SetPoolManager(this);
        return obj;
    }

    public GameObject GetArrow() => GetAvailable(arrowPrefab, arrowPoolList);
}
