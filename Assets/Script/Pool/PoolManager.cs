using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefab;
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefab.Length];

        for(int i = 0; i < pools.Length; i++) 
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(select == null) 
        {
            select = Instantiate(prefab[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
