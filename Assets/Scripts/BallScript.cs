using UnityEngine;


public class BallScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem impactFx;

    [SerializeField]
    private AudioSource impactSound;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrival")
        {
            collision.gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            //call the onfinish function of the arrival script
            collision.gameObject.GetComponent<ArrivalScript>().OnFinish();                  

            Destroy(gameObject);
        }
        else
        {
           //Instantiate the impactFx at the collision point and rotation
           Instantiate(impactFx, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            //Apply scaling to the impactFx
            impactFx.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            //Play the impact sound
            impactSound.Play();

        }
    }
}
