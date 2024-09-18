using UnityEngine;

public class BallScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrival")
        {
            collision.gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            //call the onfinish function of the arrival script
            collision.gameObject.GetComponent<ArrivalScript>().OnFinish();                  

            Destroy(gameObject);
        }
    }
}
