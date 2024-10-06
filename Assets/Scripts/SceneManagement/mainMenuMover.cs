using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuMover : MonoBehaviour
{
    public float moveSpeed;
    public float destroyAfterTime;

    void Start()
    {
        Destroy(gameObject, destroyAfterTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
    }
}
