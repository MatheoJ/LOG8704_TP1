using UnityEngine;

public class BallScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrival")
        {
            collision.gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            Destroy(gameObject);
        }
    }
}
