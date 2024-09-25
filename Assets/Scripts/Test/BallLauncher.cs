using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForce = 250f;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // R�f�rence � l'InputAction configur�e pour la position du touch
    public InputActionReference touchPositionActionReference;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // S'assurer que l'InputAction est activ�e
        if (touchPositionActionReference != null)
        {
            touchPositionActionReference.action.Enable();

            // Abonner � l'�v�nement "performed" pour capturer le toucher complet
            touchPositionActionReference.action.performed += OnTouchPerformed;
        }
    }

    void OnDestroy()
    {
        if (touchPositionActionReference != null)
        {
            // D�sabonner l'�v�nement pour �viter des erreurs � la destruction du script
            touchPositionActionReference.action.performed -= OnTouchPerformed;
            touchPositionActionReference.action.Disable();
        }
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        // R�cup�rer la position du toucher
        Vector2 touchPosition = touchPositionActionReference.action.ReadValue<Vector2>();

        Debug.Log("Touch d�tect� via Input System � la position : " + touchPosition);

        // Effectuer un raycast pour v�rifier si un plan est touch�
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinBounds))
        {
            Debug.Log("Plan d�tect� !");
            Pose hitPose = hits[0].pose;

            // Cr�er la balle � l'endroit du toucher
            GameObject ball = Instantiate(ballPrefab, hitPose.position, hitPose.rotation);
            Debug.Log("Balle instanci�e � : " + hitPose.position);

            // Appliquer une force pour simuler un lancer
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Camera.main.transform.forward * throwForce);
                Debug.Log("Force appliqu�e � la balle.");
            }
            else
            {
                Debug.LogError("Le Rigidbody n'est pas trouv� sur la balle.");
            }
        }
        else
        {
            Debug.Log("Aucun plan d�tect�.");
        }
    }
}
