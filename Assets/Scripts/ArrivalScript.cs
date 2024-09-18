using UnityEngine;

public class ArrivalScript : MonoBehaviour
{

    [SerializeField]
    private GameObject baseArea;

    [SerializeField]
    private GameObject finishedArea;

    //Get the finish explsion
    [SerializeField]
    private ParticleSystem finishExplosion;


    void Start()
    {
        // Make sure the particle system is not playing initially.
        GetComponent<ParticleSystem>().Stop();
    }


    public void OnFinish()
    {
        baseArea.SetActive(false);
        finishedArea.SetActive(true);
        finishExplosion.Play();
    }
}