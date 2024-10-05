using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCollector : MonoBehaviour
{

    private const int maxWeapons = 2;
    private int currentWeapons = 0;

    //weapon Position.
    public Transform weaponOnLeft;
    public Transform weaponOnRight;


    public Vector3 offset = new Vector3(0, 0, 0);


    public void CollectWeapon(GameObject weapon)
    {
        if (currentWeapons < maxWeapons)
        {
            Vector3 targetPosition;

            if (currentWeapons == 0)
            {
                targetPosition = weaponOnLeft.position;
            }
            else
            {
                targetPosition = weaponOnRight.position;
            }

            //Sets weapon as child, then sets it to target position.
            weapon.transform.SetParent(transform);
            weapon.transform.position = targetPosition + offset;

            // Optionally, reset rotation and scale if necessary.
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = Vector3.one;

            currentWeapons++;
        }
    }


    public void ThrowWeapon(GameObject weapon)
    {
        if (weapon.transform.IsChildOf(transform))
        {
            weapon.transform.SetParent(null);
            Rigidbody rb = weapon.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            }

            currentWeapons--;
            Debug.Log("YOU NOW HAVE: " + currentWeapons + "!");
        }
    }

    public int GetWeaponCount()
    {
        return currentWeapons;
    }

}
