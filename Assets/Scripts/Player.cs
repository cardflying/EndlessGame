using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;
    [SerializeField]
    private GameObject animatorPlayer;
    [SerializeField]
    private GameObject animatorModel;
    [SerializeField]
    private SoundController soundController;

    private GameObject spine;
    private int collectCount;
    private float playerSpeed;
    private bool start = false;
    private bool debug = false;
    public Action completeRound;
    public Action lossRound;

    public void Init(GameObject _spine, float _speed, bool _start, int _debug = 0)
    {
        spine = _spine;
        start = _start;
        playerSpeed = _speed;
        debug = _debug == 1;
        animatorPlayer.SetActive(true);
        collectCount = 0;
    }

    void Update()
    {
        if (start == false)
            return;

        if (debug == false)
        {
            if (spine != null)
            {
                animatorPlayer.transform.localPosition = new Vector3(spine.transform.position.x / 8, 0, 0);
            }
            else
            {
                soundController.PlayEffect(1);
                if (lossRound != null)
                    lossRound();
            }
        }

        if (animatorPlayer.transform.localPosition.x > 0.6f)
            animatorPlayer.transform.localPosition = new Vector3(0.6f, animatorPlayer.transform.localPosition.y, animatorPlayer.transform.localPosition.z);

        if (animatorPlayer.transform.localPosition.x < -0.6f)
            animatorPlayer.transform.localPosition = new Vector3(-0.6f, animatorPlayer.transform.localPosition.y, animatorPlayer.transform.localPosition.z);


        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);

        if (transform.position.z < endPos.position.z)
        {
            transform.position = startPos.position;

            if (completeRound != null)
                completeRound();
        }
    }

    public void Stop()
    {
        transform.position = startPos.position;
        animatorModel.transform.localPosition = new Vector3(0,-0.3f,1.5f);
        animatorModel.transform.localEulerAngles = animatorPlayer.transform.localPosition = Vector3.zero;
        animatorPlayer.SetActive(false);
        start = false;
    }

    public int TotalCollect()
    {
        return collectCount;
    }

    public void OnTriggerEnter(Collider other)
    {
        Power power = other.GetComponent<Power>();

        if (power != null)
        {
            power.Hide();

            if (power.powerIndex == 1)
            {
                soundController.PlayEffect(1);
                if (lossRound != null)
                    lossRound();
            }
            else
            {
                soundController.PlayEffect(0);
                collectCount++;
            }
        }
    }
}
