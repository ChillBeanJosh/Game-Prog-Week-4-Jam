using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabSpawner : MonoBehaviour
{
    public GameObject prefab;

    public float intervalTime;

    void Start()
    {
        InvokeRepeating("InstantiateObject", intervalTime, intervalTime);
    }

    void InstantiateObject()
    {
        Instantiate(prefab, transform.position, prefab.transform.rotation);
    }
}
