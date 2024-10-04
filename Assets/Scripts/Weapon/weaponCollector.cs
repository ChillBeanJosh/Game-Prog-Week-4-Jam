using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCollector : MonoBehaviour
{

    private const int maxWeapons = 2;
    private int currentWeapons = 0;

    //Weapon Position.
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

            //Ensure its at weapon position.
            weapon.transform.position = targetPosition + offset; 
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = Vector3.zero;

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
        }
    }

    public int GetWeaponCount()
    {
        return currentWeapons;
    }

}
