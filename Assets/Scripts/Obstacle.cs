using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float speed = 50;

    private float maxSpeed;
    public Vector3 endPoint;
    public Action<Obstacle> completeHide_Callback;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxSpeed = speed * Time.deltaTime;

        transform.Translate(Vector3.back * maxSpeed);

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        Debug.Log(transform.position.z +" " + endPoint.z);
        if (transform.position.z > endPoint.z)
        {
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);

            if (completeHide_Callback != null)
            {
                completeHide_Callback(this);
            }

            completeHide_Callback = null;
        }
    }
}
