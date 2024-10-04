using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponInteraction : MonoBehaviour
{
    //attached to weapon Object.
    [SerializeField] private weaponSpawner spawner;
    [SerializeField] private weaponCollector collector;

    private void Start()
    {
        //Refrence to other scripts, to use variables.
        spawner = FindObjectOfType<weaponSpawner>();
        collector = FindObjectOfType<weaponCollector>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!spawner.childToggle)
            {
                collector.CollectWeapon(gameObject);
            }
            else
            {
                collector.CollectWeapon(gameObject);
            }
        }
        else
        {
            //null check.
            Debug.LogError("Refrence not found.");
        }
    }
}
