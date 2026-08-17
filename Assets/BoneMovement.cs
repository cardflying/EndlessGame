using System.Collections;
using UnityEngine;

public class BoneMovement : MonoBehaviour
{
    [Header("Joint Transforms (Assign from Kinect Rig)")]
    public Transform leftAnkle;
    public Transform rightAnkle;

    [Header("Running Settings")]
    [Tooltip("Velocity threshold in meters per second to trigger running.")]
    public float runThresholdSpeed = 2.5f;

    private Vector3 lastLeftAnklePos;
    private Vector3 lastRightAnklePos;

    public GameObject targetObj;
    GameObject al;
    GameObject ar;
    bool missingAnkle;

    public float LeftAnkleSpeed { get; private set; }
    public float RightAnkleSpeed { get; private set; }
    public bool IsRunning { get; private set; }

    void Start()
    {
        // Initialize positions to avoid a massive speed spike on frame one
        if (leftAnkle) lastLeftAnklePos = leftAnkle.position;
        if (rightAnkle) lastRightAnklePos = rightAnkle.position;

        StartCoroutine(FindAnkle());
    }


    IEnumerator FindAnkle()
    {
        while (leftAnkle == null || rightAnkle == null)
        {
            al = GameObject.Find("AnkleLeft");
            ar = GameObject.Find("AnkleRight");

            yield return new WaitForEndOfFrame();


            if (al != null && ar != null)
            {
                Debug.Log(ar.name + " " + al.name);
                leftAnkle = al.transform;
                rightAnkle = ar.transform;
                lastLeftAnklePos = leftAnkle.position;
                lastRightAnklePos = rightAnkle.position;

                missingAnkle = false;
                break;
            }
        }

    }

    void Update()
    {
        if (leftAnkle == null || rightAnkle == null)
        {
            if (missingAnkle != true)
            {
                missingAnkle = true;
                StartCoroutine(FindAnkle());
            }
            return;
        }

        // 1. Calculate Velocity (Delta Distance / Delta Time) = Meters per Second
        float deltaTime = Time.deltaTime;
        if (deltaTime > 0)
        {
            // Left Ankle Speed
            float leftDist = Vector3.Distance(leftAnkle.position, lastLeftAnklePos);
            LeftAnkleSpeed = leftDist / deltaTime;

            // Right Ankle Speed
            float rightDist = Vector3.Distance(rightAnkle.position, lastRightAnklePos);
            RightAnkleSpeed = rightDist / deltaTime;
        }

        // 2. Evaluate State
        // If either lower joint exceeds the threshold speed, the user is running
        if (LeftAnkleSpeed > runThresholdSpeed || RightAnkleSpeed > runThresholdSpeed)
        {
            IsRunning = true;
            OnRunning();
        }
        else
        {
            IsRunning = false;
        }

        // 3. Save current positions for the next frame's calculation
        lastLeftAnklePos = leftAnkle.position;
        lastRightAnklePos = rightAnkle.position;
    }

    void OnRunning()
    {
        // Insert your game logic here
        Debug.Log($"Player is running! L: {LeftAnkleSpeed:F2} m/s, R: {RightAnkleSpeed:F2} m/s");

        SpeedRange(LeftAnkleSpeed);
        SpeedRange(RightAnkleSpeed);
    }

    private void SpeedRange(float ankleSpeed)
    {
        if (ankleSpeed >= 0f && ankleSpeed < 60f)
        {
            // The player is IDLE or walking slowly
            
            Debug.Log("State: Idle");

        }
        else if (ankleSpeed >= 60f && ankleSpeed <= 90f)
        {
            // The player is RUNNING
            targetObj.transform.Translate(Vector3.up);

            Debug.Log("State: Running!");
        }
    }
}
