using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootAllWeapons : MonoBehaviour
{
    public KeyCode shootAll = KeyCode.F;

    [Header("Cooldown Settings")]
    public float shootAllCooldown = 5f;
    private float nextShootTime = 0f;

    [Header("Shooting Settings")]
    public float throwForce = 20f;
    private weaponCollector collector;

    public Camera playerCamera;
    public Transform reticleTransform;

    private void Start()
    {
        collector = GetComponent<weaponCollector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(shootAll) && Time.time >= nextShootTime)
        {
            StartCoroutine(ShootAll());

            Debug.Log("SHOOTING ALL WEAPONS!");

            nextShootTime = Time.time + shootAllCooldown;
        }
    }

    public float rotationSpeed; // Speed at which the weapon rotates
    public float destructionDelay = 5f; // Time before the weapon is destroyed after being thrown

    private System.Collections.IEnumerator ShootAll()
    {
        // Get all weapons in the scene with the "Weapon" tag.
        GameObject[] allWeapons = GameObject.FindGameObjectsWithTag("Weapon");

        // List to store coroutines for raising and rotating weapons
        List<System.Collections.IEnumerator> positionAndRotationCoroutines = new List<System.Collections.IEnumerator>();

        // Raise each weapon and update its rotation
        foreach (GameObject weapon in allWeapons)
        {
            // If the weapon is not a child of the player 
            if (!weapon.transform.IsChildOf(transform))
            {
                // Get Rigidbody reference to all those weapons
                Rigidbody rb = weapon.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    // Store the initial rotation
                    Quaternion initialRotation = weapon.transform.rotation;

                    // Raise the weapon by 5 units and calculate the shoot direction
                    Vector3 initialPosition = weapon.transform.position;
                    Vector3 targetPosition = initialPosition + new Vector3(0, 2,0);

                    // Start the position and rotation coroutine for this weapon
                    positionAndRotationCoroutines.Add(UpdateWeaponPositionAndRotation(weapon.transform, initialRotation, targetPosition, rb));
                }
            }
        }

        // Start all position and rotation coroutines simultaneously
        foreach (var rotationCoroutine in positionAndRotationCoroutines)
        {
            StartCoroutine(rotationCoroutine);
        }

        // Allow time for all weapons to finish raising
        yield return new WaitForSeconds(2.0f); // Adjust this if necessary for raising duration

        // Apply forces after all positions and rotations are set
        foreach (GameObject weapon in allWeapons)
        {
            // If the weapon is not a child of the player 
            if (!weapon.transform.IsChildOf(transform))
            {
                Rigidbody rb = weapon.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 shootDirection;

                    if (reticleTransform != null)
                    {
                        shootDirection = (reticleTransform.position - weapon.transform.position).normalized;
                    }
                    else
                    {
                        // Default direction if reticle is null
                        shootDirection = playerCamera.transform.forward;
                    }

                    // Finalize the rotation before applying force
                    weapon.transform.rotation = Quaternion.LookRotation(shootDirection) * Quaternion.AngleAxis(90, Vector3.right);

                    // Force for movement
                    rb.AddForce(shootDirection * throwForce, ForceMode.Impulse);
                    rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

                    // Freeze unwanted rotations
                    rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;

                    // Delay between scene weapon thrown
                    yield return new WaitForSeconds(0.001f); // You may adjust this delay if needed

                    // Start coroutine to destroy the weapon after the specified delay
                    StartCoroutine(DestroyWeaponAfterDelay(weapon, destructionDelay));
                }
            }
        }

        yield return null;
    }

    private System.Collections.IEnumerator UpdateWeaponPositionAndRotation(Transform weaponTransform, Quaternion initialRotation, Vector3 targetPosition, Rigidbody rb)
    {
        float duration = 1.0f; // Duration for the upward motion
        float elapsedTime = 0f;

        // This loop raises the weapon and updates its rotation
        while (elapsedTime < duration)
        {
            // Interpolate the weapon's position to move upwards
            weaponTransform.position = Vector3.Lerp(weaponTransform.position, targetPosition, elapsedTime / duration);

            // Update shoot direction based on reticle or camera
            Vector3 shootDirection;

            if (reticleTransform != null)
            {
                shootDirection = (reticleTransform.position - weaponTransform.position).normalized;
            }
            else
            {
                // Default direction if reticle is null
                shootDirection = playerCamera.transform.forward;
            }

            // Calculate the target rotation to look at the shoot direction
            Quaternion targetRotation = Quaternion.LookRotation(shootDirection) * Quaternion.AngleAxis(90, Vector3.right);

            // Use Slerp to smoothly rotate towards the target rotation
            weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set correctly
        weaponTransform.position = targetPosition; // Ensure final position

        // Keep updating direction until force is applied
        while (true) // Keep updating direction
        {
            Vector3 shootDirection;

            if (reticleTransform != null)
            {
                shootDirection = (reticleTransform.position - weaponTransform.position).normalized;
            }
            else
            {
                // Default direction if reticle is null
                shootDirection = playerCamera.transform.forward;
            }

            // Calculate the target rotation to look at the shoot direction
            Quaternion targetRotation = Quaternion.LookRotation(shootDirection) * Quaternion.AngleAxis(90, Vector3.right);

            // Use Slerp to smoothly rotate towards the target rotation
            weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            yield return null; // Wait for the next frame
        }
    }

    // Coroutine to destroy the weapon after a delay
    private System.Collections.IEnumerator DestroyWeaponAfterDelay(GameObject weapon, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(weapon); // Destroy the weapon object
    }
}
    

