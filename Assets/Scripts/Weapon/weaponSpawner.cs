using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSpawner : MonoBehaviour
{
    public KeyCode spawnWeapon = KeyCode.Q;
    public GameObject[] weapons;

    //for determining if the weapons are childs to the player.
    public bool childToggle = false;

    public float weaponSpacing = 2.0f;

    public float cooldown = 7.0f;
    private float cdTime;

    private weaponCollector collector;

    void Start()
    {
        collector = FindObjectOfType<weaponCollector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(spawnWeapon) && Time.time >= cdTime)
        {
            SpawnWeapons();
            Debug.Log("WEAPONS SPAWNED!");
            cdTime = Time.time + cooldown;
        }
    }

    void SpawnWeapons()
    {
        //null check.
        if (weapons.Length < 6)
        {
            Debug.LogError("Assign 6 weapons in the weapons array.");
            return;
        }

        //Spawning on the left.
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = transform.position + transform.right * -(i + 1) * weaponSpacing;
            GameObject newWeapon = Instantiate(weapons[i], spawnPosition, Quaternion.identity);

            if (childToggle)
            {
                newWeapon.transform.SetParent(transform);

                //collects weapons automatically.
                if (collector != null)
                {
                    collector.CollectWeapon(newWeapon);
                }
            }
        }

        //Spawning on the right.
        for (int i = 3; i < 6; i++)
        {
            Vector3 spawnPosition = transform.position + transform.right * (i - 2) * weaponSpacing;
            GameObject newWeapon = Instantiate(weapons[i], spawnPosition, Quaternion.identity);

            if (childToggle)
            {
                newWeapon.transform.SetParent(transform);

                //collects weapons automatically.
                if (collector != null)
                {
                    collector.CollectWeapon(newWeapon);
                }
            }
        }
    }
}
