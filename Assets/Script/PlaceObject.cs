using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;



[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{

    [SerializeField]
    private GameObject ballToSpawn;

    
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    private GameObject spawnedBall;

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;


        var camera = Camera.main;
        var screenCenter = camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {

            if (spawnedBall != null)
            {
                Destroy(spawnedBall);
            }

            var hitPose = hits[0].pose;
            spawnedBall = Instantiate(ballToSpawn, camera.transform.position, hitPose.rotation);
            var rigidbody = spawnedBall.GetComponent<Rigidbody>();
            rigidbody.linearVelocity = camera.transform.forward * 5.0f;
        }
    }
}
