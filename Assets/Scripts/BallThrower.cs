using UnityEngine;
using System.Collections;

public class BallThrower : MonoBehaviour
{
    public GameObject ballPrefab; // Reference to the ball prefab
    public float throwForce = 10f; // Speed of the thrown ball   
    public float throwInterval = 2f; // Time interval between throws.  /!\ throwInterval > PortalCooldown or teleportation won't work properly /!\

    private void Start()
    {
        // Start the ball throwing coroutine
        StartCoroutine(ThrowBalls());
    }

    private IEnumerator ThrowBalls()
    {
        Debug.Log("In ThrowBalls()");
        while (true) // keep throwing balls
        {
            ThrowBall(); 

            // Wait for the specified interval before throwing again
            yield return new WaitForSeconds(throwInterval);
        }
    }

    private void ThrowBall()
    {
        // Create the ball at the position of this object
        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        // Get the Rigidbody component and apply force in the specified direction
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.AddForce(transform.forward.normalized * throwForce, ForceMode.Impulse);
        }
    }
}
