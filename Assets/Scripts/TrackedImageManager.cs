using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImageManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject[] portalPrefabs;

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

        GameObject portalPrefab = null;

        // Instancier le bon portail selon l'image
        switch (imageName)
        {
            case "one":
                portalPrefab = portalPrefabs[0];
                Debug.Log("Image one detected!");
                break;
            case "two":
                portalPrefab = portalPrefabs[1];
                Debug.Log("Image two detected!");
                break;
            case "three":
                portalPrefab = portalPrefabs[2];
                Debug.Log("Image three detected!");
                break;
            case "four":
                portalPrefab = portalPrefabs[3];
                Debug.Log("Image four detected!");
                break;
        }

        if (portalPrefab != null)
        {
            // Adjust rotation to face the correct way
            Quaternion adjustedRotation = trackedImage.transform.rotation * Quaternion.Euler(-90f, 0f, 0f); // Rotate 180 degrees to face the camera

            // Move the portal slightly forward
            Vector3 forwardOffset = trackedImage.transform.up * 0.05f;

            // Instantiate the portal with the adjusted position and rotation
            GameObject spawnedPortal = Instantiate(portalPrefab, trackedImage.transform.position + forwardOffset, adjustedRotation);

            spawnedPortal.transform.parent = trackedImage.transform;
        }
    }
}
