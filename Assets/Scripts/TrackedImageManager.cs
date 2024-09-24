using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class TrackedImageManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject[] portalPrefabs;

    // Dictionary to track spawned portals
    private Dictionary<string, GameObject> activePortals = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdatePortalForImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdatePortalForImage(trackedImage);
        }
    }


    private void UpdatePortalForImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        // Check if the portal is already spawned
        if (!activePortals.ContainsKey(imageName))
        {
            GameObject portalPrefab = GetPortalPrefab(imageName);

            if (portalPrefab != null)
            {
                // Adjust rotation to face the correct way
                Quaternion adjustedRotation = trackedImage.transform.rotation * Quaternion.Euler(-90f, 0f, 0f); 

                // Move the portal slightly forward
                Vector3 forwardOffset = trackedImage.transform.up * 0.05f;

                // Instantiate the portal with the adjusted position and rotation
                GameObject spawnedPortal = Instantiate(portalPrefab, trackedImage.transform.position + forwardOffset, adjustedRotation);
                spawnedPortal.transform.parent = trackedImage.transform;

                // Track the spawned portal
                activePortals.Add(imageName, spawnedPortal);
                Debug.Log($"{imageName} detected!");
            }
        }
    }

    private GameObject GetPortalPrefab(string imageName)
    {
        switch (imageName)
        {
            case "one":
                return portalPrefabs[0];
            case "two":
                return portalPrefabs[1];
            case "three":
                return portalPrefabs[2];
            case "four":
                return portalPrefabs[3];
            default:
                return null;
        }
    }
}
