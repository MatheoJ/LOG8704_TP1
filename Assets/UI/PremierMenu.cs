using UnityEngine;

public class PremierMenu : MonoBehaviour
{
    bool closing = false;//float t = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (closing)
        {
            //transform.position.y = Mathf.Lerp(0,)
            //transform.position.y = ;
            //transform.position.Set(transform.position.x, transform.position.y+5, transform.position.z);
            //((RectTransform)transform).localPosition.Set(transform.localPosition.x, transform.localPosition.y + 5, transform.localPosition.z);
            Debug.Log("closing");
            ((RectTransform)transform).anchoredPosition = ((RectTransform)transform).anchoredPosition + new Vector2(0, 10);
        }
        if (((RectTransform)transform).anchoredPosition.y > 10000)
        {
            closing = false;
            //this.isActiveAndEnabled = true;
            gameObject.SetActive(false);
        }
    }

    public void close()
    {
        closing = true;
        //gameObject.SetActive(false);
    }
}
