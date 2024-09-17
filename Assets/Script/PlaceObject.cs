using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;



[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{

    [SerializeField]
    private GameObject ballToSpawn;

    [SerializeField]
    private GameObject arrivalPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedBall;
    private GameObject spawnedArrival;

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
        if (spawnedArrival == null)
        {
            if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                Vector3 addedPosition = new Vector3(0, 0.1f, 0);
                spawnedArrival = Instantiate(arrivalPrefab, hitPose.position + addedPosition, hitPose.rotation);
            }
        }
        elseusing System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.InputSystem.EnhancedTouch;
        using UnityEngine.XR.ARFoundation;
        using UnityEngine.XR.ARSubsystems;
        using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;



        [RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
        public class PlaceObjectVariable : MonoBehaviour
    {

        [SerializeField]
        private GameObject ballToSpawn;

        [SerializeField]
        private GameObject arrivalPrefab;

        private ARRaycastManager raycastManager;
        private ARPlaneManager planeManager;

        private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

        private GameObject spawnedBall;
        private GameObject spawnedArrival;

        private float touchStartTime;
        private bool isTouching = false;

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
            EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            EnhancedTouch.TouchSimulation.Disable();
            EnhancedTouch.EnhancedTouchSupport.Disable();
            EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
            EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
        }

        private void OnFingerDown(Finger finger)
        {
            if (finger.index != 0 || isTouching)
                return;

            touchStartTime = Time.time;  // Capture le temps au début du toucher
            isTouching = true;
        }

        private void OnFingerUp(EnhancedTouch.Finger finger)
        {
            if (finger.index != 0)
                return;

            float touchDuration = Time.time - touchStartTime; // Calculate how long the screen was touched
            touchDuration = Mathf.Clamp(touchDuration, 0f, 3f); // Clamp the duration between 0 and 3 seconds
            var camera = Camera.main;

            if (spawnedArrival == null)
            {

                var screenCenter = camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

                if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;
                    Vector3 addedPosition = new Vector3(0, 0.1f, 0);
                    spawnedArrival = Instantiate(arrivalPrefab, hitPose.position + addedPosition, hitPose.rotation);
                }
            }
            else
            {
                if (spawnedBall != null)
                {
                    Destroy(spawnedBall);
                }
                spawnedBall = Instantiate(ballToSpawn, camera.transform.position, camera.transform.rotation);
                var rigidbody = spawnedBall.GetComponent<Rigidbody>();

                float forceMultiplier = Mathf.Lerp(0.5f, 8.0f, touchDuration / 3.0f); // Adjust force based on touch duration

                rigidbody.AddForce(camera.transform.forward * forceMultiplier, ForceMode.Impulse); // Apply force with adjusted strength
            }

            isTouching = false; // Reset touching flag
        }


    }

        {
            if (spawnedBall != null)
            {
                Destroy(spawnedBall);
            }
            spawnedBall = Instantiate(ballToSpawn, camera.transform.position, camera.transform.rotation);
            var rigidbody = spawnedBall.GetComponent<Rigidbody>();
            rigidbody.linearVelocity = camera.transform.forward * 5.0f;
        }
    }
}