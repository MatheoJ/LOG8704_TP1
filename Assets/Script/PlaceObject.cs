using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System.Reflection;



[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    [SerializeField]
    private BarreProgression barreProgression;

    [SerializeField]
    private Text debug;

    [SerializeField]
    private GameObject ballToSpawn;

    [SerializeField]
    private GameObject arrivalPrefab;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedBall;
    private GameObject spawnedArrival;

    private float touchStartTime=0f;
    private bool isTouching = false;

    //private BarreProgression barreProgression;

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
        //barreProgression = FindObjectOfType<BarreProgression>();
        
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

    private void Update()
    {
        if (isTouching && touchStartTime != 0f && spawnedArrival!=null)
        {
            barreProgression.SetValue(Mathf.Clamp(Time.time - touchStartTime, 0f, 3f) / 3f);
            debug.text = (Mathf.Clamp(Time.time - touchStartTime, 0f, 3f) / 3f).ToString();
        }
    }

    private void OnFingerDown(Finger finger)
    {
        

        if (finger.index != 0 || isTouching)
        {
            
            return;
        }

        touchStartTime = Time.time;  // Capture le temps au d�but du toucher
        isTouching = true;
        //barreProgression.SetValue(Mathf.Clamp(Time.time - touchStartTime, 0f, 3f) / 3f);
    }

    private void OnFingerUp(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        float touchDuration = Time.time - touchStartTime; // Calculate how long the screen was touched
        touchDuration = Mathf.Clamp(touchDuration, 0f, 3f); // Clamp the duration between 0 and 3 seconds
        var camera = Camera.main;


        // Get touch position
        var touchPosition = finger.screenPosition;

        if (spawnedArrival == null)
        {
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                Vector3 addedPosition = new Vector3(0, 0.1f, 0);
                spawnedArrival = Instantiate(arrivalPrefab, hitPose.position + addedPosition, hitPose.rotation);
                //toggle.isOn = true;
            }
            //toggle.enabled = true;
            
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
        touchStartTime = 0f;
        barreProgression.SetValue(0f);
        isTouching = false; // Reset touching flag
    }

    public void replaceArrival()
    {
        //spawnedArrival.gameObject.
        GameObject.Destroy(spawnedArrival);
        //spawnedArrival = null; 
    }

}