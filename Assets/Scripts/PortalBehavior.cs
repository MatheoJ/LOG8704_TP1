using UnityEngine;
using System.Collections;

public class PortalBehavior : MonoBehaviour
{
    private GameObject exitPortal; // Reference to the other portal
    public GameObject spawnPosition; // position at which the object will spawn when teleporting
    private bool isTeleporting = false; // Prevent teleport loop
    public float portalCooldown = 0.5f; //
    
    private void OnTriggerEnter(Collider other)
    {
        if (spawnPosition == null)
        {
            Debug.LogError("SpawnPosition is not set on " + gameObject.name);
            return;
        }

        // Ensure that the object is teleportable and isn't already teleporting
        if (!isTeleporting && other.CompareTag("Teleportable"))
        {
            // Get my tag
            string myTag = gameObject.tag;

            // Get all objects with same tag
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag(myTag);
            foreach (GameObject obj in allObjects)
            {
                if (obj != gameObject)
                {
                    exitPortal = obj;
                    break;
                }
            }
            if (exitPortal != null)
            {

                Rigidbody otherRb = other.GetComponent<Rigidbody>();
                if (otherRb != null)
                {
                    StartCoroutine(TeleportObject(other, otherRb)); // Using coroutine to avoid the immediate re-triggering of teleportation
                }
            }
        }        
    }

    private IEnumerator TeleportObject(Collider other, Rigidbody otherRb)
    {        
        // Disabling the two portals teleporting ability
        isTeleporting = true;

        exitPortal.GetComponent<PortalBehavior>().isTeleporting = true;
        
        Debug.Log("BEFORE teleportation : Object " + other.name + " at position : " + other.transform.position);

        // Position
        Vector3 localPos = transform.InverseTransformPoint(other.transform.position); // Local position relative to the entry portal
        other.transform.position = exitPortal.GetComponent<PortalBehavior>().spawnPosition.transform.position; // Teleport object to the exit portal

        // Velocity
        Vector3 localVelocity = transform.InverseTransformDirection(otherRb.linearVelocity); // Get the object's velocity in the local space of the entry portal
        Vector3 newVelocity = exitPortal.transform.TransformDirection(-localVelocity); // "-localVelocity" because the object exits the portal through its front, not its back
        otherRb.linearVelocity = newVelocity; // Set the new velocity of the ball

        TrailRenderer trail = other.GetComponentInChildren<TrailRenderer>();
        trail.Clear(); // Clear the trail to avoid a trail between the two portals

        yield return new WaitForSeconds(portalCooldown); // Small delay to prevent immediate re-teleportation (the other way around) -> temporarily stops the coroutine

        // Enabling the two portals teleporting ability
        isTeleporting = false; 
        exitPortal.GetComponent<PortalBehavior>().isTeleporting = false;
    }
}