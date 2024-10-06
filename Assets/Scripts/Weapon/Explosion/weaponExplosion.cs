using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponExplosion : MonoBehaviour
{
    public float maxSize;
    public float growthSpeed;
    public float timeTillDestroy;

    private bool isGrowing = true;

    private void Update()
    {
        if (isGrowing)
        {
            transform.localScale += Vector3.one * growthSpeed * Time.deltaTime;

            if (transform.localScale.x  >= maxSize)
            {
                transform.localScale = new Vector3(maxSize, maxSize, maxSize);

                isGrowing = false;
                Invoke("DestroyObject", timeTillDestroy);
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

}
