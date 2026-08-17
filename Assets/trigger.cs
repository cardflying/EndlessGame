using UnityEngine;

public class trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("LL");
    }
}
