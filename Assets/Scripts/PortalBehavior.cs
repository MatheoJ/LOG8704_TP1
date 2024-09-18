using UnityEngine;
using System.Collections;

public class PortalBehavior : MonoBehaviour
{
    public PortalBehavior exitPortal; // Reference to the other portal
    public GameObject spawnPosition; // position at which the object will spawn when teleporting
    private bool isTeleporting = false; // Prevent teleport loop
    public float portalCooldown = 0.5f; //
    public float exitOffsetDistance = 0.0f; // Offset distance to ensure ball exits in front of portal, depends on the portal thickness
    


    private void OnTriggerEnter(Collider other)
    {
        // Check if ExitPortal is assigned
        if (exitPortal == null)
        {
            Debug.LogError("ExitPortal is not set on " + gameObject.name);
            return;
        }

        if (spawnPosition == null)
        {
            Debug.LogError("SpawnPosition is not set on " + gameObject.name);
            return;
        }


        // Check if the object is teleportable and isn't already teleporting
        if (!isTeleporting && other.CompareTag("Teleportable"))
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                // using coroutine to avoid the immediate re-triggering of teleportation
                StartCoroutine(TeleportObject(other, otherRb)); 
            }
        }        
    }

    private IEnumerator TeleportObject(Collider other, Rigidbody otherRb)
    {
        // Disabling the two portals teleporting ability
        isTeleporting = true;
        exitPortal.isTeleporting = true;

        // Get the local position relative to EntryPortal
        Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

        // Teleport object to the exit portal & set its position
        other.transform.position = exitPortal.spawnPosition.transform.position;

        // Apply an offset to the ball's position so it appears just in front of the exit portal
        other.transform.position += exitPortal.transform.forward * exitOffsetDistance;

        // Calculate the velocity in the local space of the entry portal
        Vector3 localVelocity = transform.InverseTransformDirection(otherRb.linearVelocity);

        // Transform the velocity into the local space of the exit portal
        // "-localVelocity" because the object exits the portal through its front, not its back
        Vector3 newVelocity = exitPortal.transform.TransformDirection(-localVelocity); 

        // Set the new velocity of the ball after teleportation
        otherRb.linearVelocity = newVelocity;

        Debug.Log("Object " + other.name + " teleported from " + name + " to " + exitPortal.name);

        yield return new WaitForSeconds(portalCooldown); // Small delay to prevent immediate re-teleportation (the other way around) --> temporarily stops the coroutine

        // Enabling the two portals teleporting ability
        isTeleporting = false; 
        exitPortal.isTeleporting = false;
    }
}
