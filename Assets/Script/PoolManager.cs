using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PoolPrefab
{
    public PoolType type;
    public GameObject prefab;
    public int startPrefab;
    public List<GameObject> list;
}
public class PoolManager : MonoBehaviour
{
    public PoolPrefab[] poolPrefabs;

    public static PoolManager instance
    {
        get { return m_instance; }
    }
    private static PoolManager m_instance;

    private void Awake()
    {
        if (m_instance == null) m_instance = this;
        else Destroy(gameObject);

        foreach (PoolPrefab poolPrefab in poolPrefabs)
        {
            for (int i = 0; i < poolPrefab.startPrefab; i++)
            {
                GameObject obj = Instantiate(poolPrefab.prefab);
                //obj.transform.parent = transform;
                obj.transform.SetParent(transform, true);
                obj.SetActive(false);
                poolPrefab.list.Add(obj);
            }
        }
    }

    public GameObject GetObj(PoolType type)
    {
        GameObject obj = null;
        foreach (PoolPrefab poolPrefab in poolPrefabs)
        {
            if (poolPrefab.type != type) continue;

            foreach (GameObject g in poolPrefab.list)
            {
                if (!g.activeSelf)
                {
                    obj = g;
                    break;
                }
            }
            break;
        }
        if (obj == null)
        {
            obj = MakeObj(type);
        }
        return obj;
    }
    GameObject MakeObj(PoolType type)
    {
        GameObject obj = null;
        foreach (PoolPrefab poolPrefab in poolPrefabs)
        {
            if (poolPrefab.type == type)
            {
                obj = Instantiate(poolPrefab.prefab);
                //obj.transform.parent = transform;
                obj.transform.SetParent(transform, true);
                obj.SetActive(false);
                poolPrefab.list.Add(obj);
                break;
            }
        }
        return obj;
    }
}
