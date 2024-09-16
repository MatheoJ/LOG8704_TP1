using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForce = 500f;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Référence à l'InputAction configurée pour la position du touch
    public InputActionReference touchPositionActionReference;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // S'assurer que l'InputAction est activée
        if (touchPositionActionReference != null)
        {
            touchPositionActionReference.action.Enable();

            // Abonner à l'événement "performed" pour capturer le toucher complet
            touchPositionActionReference.action.performed += OnTouchPerformed;
        }
    }

    void OnDestroy()
    {
        if (touchPositionActionReference != null)
        {
            // Désabonner l'événement pour éviter des erreurs à la destruction du script
            touchPositionActionReference.action.performed -= OnTouchPerformed;
            touchPositionActionReference.action.Disable();
        }
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        // Récupérer la position du toucher
        Vector2 touchPosition = touchPositionActionReference.action.ReadValue<Vector2>();

        Debug.Log("Touch détecté via Input System à la position : " + touchPosition);

        // Effectuer un raycast pour vérifier si un plan est touché
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinBounds))
        {
            Debug.Log("Plan détecté !");
            Pose hitPose = hits[0].pose;

            // Créer la balle à l'endroit du toucher
            GameObject ball = Instantiate(ballPrefab, hitPose.position, hitPose.rotation);
            Debug.Log("Balle instanciée à : " + hitPose.position);

            // Appliquer une force pour simuler un lancer
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Camera.main.transform.forward * throwForce);
                Debug.Log("Force appliquée à la balle.");
            }
            else
            {
                Debug.LogError("Le Rigidbody n'est pas trouvé sur la balle.");
            }
        }
        else
        {
            Debug.Log("Aucun plan détecté.");
        }
    }
}
