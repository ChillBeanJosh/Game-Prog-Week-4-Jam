using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSpawner : MonoBehaviour
{

    public KeyCode spawnWeapon = KeyCode.Q;
    public GameObject[] weapons;

    //Following weapons toggle.
    public bool childToggle = false;

    public float weaponSpacing = 2.0f;


    public float timeTillDestroy = 5.0f;

    public float cooldown = 7.0f;
    private float cdTime;

    void Update()
    {
        if (Input.GetKeyDown(spawnWeapon) && Time.time >= cdTime)
        {
            SpawnWeapons();
            cdTime = Time.time + cooldown;
        }
    }


    void SpawnWeapons()
    {
        //null check.
        if (weapons.Length < 6)
        {
            Debug.LogError("Assign 6 weapons in array");
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
            }

            Destroy(newWeapon, timeTillDestroy);
        }




        //Spawning on the right.
        for (int i = 3; i < 6; i++)
        {
            Vector3 spawnPosition = transform.position + transform.right * (i - 2) * weaponSpacing;
            GameObject newWeapon = Instantiate(weapons[i], spawnPosition, Quaternion.identity);

            if (childToggle)
            {
                newWeapon.transform.SetParent(transform);
            }

            Destroy(newWeapon, timeTillDestroy);
        }
    }
}
