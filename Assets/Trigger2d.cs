using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// This is detect 2d box collider
/// </summary>
public class Trigger2d : MonoBehaviour
{
    int count = 0;
    public Action triggerCallback;

    public void Update()
    {
        if (count > 50)
        {
            GameEventManager.RaiseMessage("Enter");
            count = 0;

            if (triggerCallback != null)
                triggerCallback();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        count++;
        // Check what entered the button's trigger zone
        Debug.Log("Triggered by: " + other.gameObject.name);
    }
}
