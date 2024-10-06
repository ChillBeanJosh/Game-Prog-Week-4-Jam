using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionTrigger : MonoBehaviour
{
    public GameObject explosionObject;
    public LayerMask targetLayer;

    public Vector3 spawnOffset = Vector3.zero;


    private void OnTriggerEnter(Collider other)
    {
        // Check if the object's layer matches the target layer
        if ((targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Instantiate(explosionObject, transform.position + spawnOffset, Quaternion.identity);
        }
    }
}
