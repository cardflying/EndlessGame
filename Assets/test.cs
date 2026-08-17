using UnityEngine;

public class test : MonoBehaviour
{
    public float a, b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward);

        if (transform.position.z < -60f)
        {
            transform.position = new Vector3(0, 0.5f, 20);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(0,0.5f, a);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector3(0, 0.5f, b);
        }
    }
}
