using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameB : GameSystem
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject startMenu;
    [SerializeField]
    private GameObject gameMenu;
    [SerializeField]
    private GameObject timer_object;
    [SerializeField]
    private TMP_Text timer_text;
    [SerializeField]
    private List<Power> power_Object = new List<Power>();
    [SerializeField]
    private TMP_Text score_text;
    [SerializeField]
    private TMP_Text displayScore_text;
    [SerializeField]
    private CountDown countDown;
    [SerializeField]
    private SaveGame saveGame;
    [SerializeField]
    private List<Vector3> powerArrage = new List<Vector3>();

    private int maxTimer = 30;
    private float maxSpeed = 20;


    private GameObject spineMid;
    private bool triggerOnce;
    private int powerIndex;
    private float highScore = 0;
    private float timer_value;
    private float distant_value;
    private float score_value;
    private Tween timer_Tween;
    private int avoidBombLine;

    private void OnEnable()
    {
        GameEventManager.OnMessageBroadcasted += HandleIncomingMessage;
        countDown.completeCallback += StartGame;
    }

    // Stop listening if the object is destroyed or disabled to prevent memory leaks
    private void OnDisable()
    {
        GameEventManager.OnMessageBroadcasted -= HandleIncomingMessage;
        countDown.completeCallback -= PreStart;
    }

    /// <summary>
    /// Received message from kinect body
    /// </summary>
    /// <param name="messageText"></param>
    private void HandleIncomingMessage(string messageText)
    {
        if (messageText == "Enter")
        {
            PreStart();
        }
    }

    /// <summary>
    /// Initiate the program
    /// </summary>
    public override void InitGame()
    {
        maxSpeed = gameData.GetRawData().speed;
        maxTimer = gameData.GetRawData().timer;

        player.completeRound += SpawnObject;
        player.lossRound += StopGame;
    }

    /// <summary>
    /// Prepare the game scene
    /// </summary>
    public void PreStart()
    {
        SpawnObject();
        timer_value = maxTimer;
        InitModelBody();
        startMenu.SetActive(false);
        gameMenu.SetActive(true);

        countDown.StartCountdown();
    }


    /// <summary>
    /// Start Game
    /// </summary>
    public override void StartGame()
    {
        startGame = true;
        player.Init(spineMid, maxSpeed, startGame, gameData.GetRawData().debug);
    }

    /// <summary>
    /// End Game
    /// </summary>
    public void StopGame()
    {
        startGame = false;

        startMenu.SetActive(true);
        gameMenu.SetActive(false);
        timer_value = maxTimer;
        timer_text.text = timer_value.ToString();
        distant_value = 0;
        avoidBombLine = 0;
        displayScore_text.text = "0";
        player.Stop();
        StartCoroutine(ShowResult());
    }

    /// <summary>
    /// Spawn Object for the scene
    /// </summary>
    public override void SpawnObject()
    {
        if (!triggerOnce)
        {
            for (int i = 0; i < power_Object.Count / 3; i++)
            {
                int index = Random.Range(0, powerArrage.Count);

                power_Object[(i * 3) + 0].Init((int)powerArrage[index].x);
                power_Object[(i * 3) + 1].Init((int)powerArrage[index].y);
                power_Object[(i * 3) + 2].Init((int)powerArrage[index].z);
            }
            triggerOnce = true;
        }
        else
        {
            for (int i = 0; i < power_Object.Count; i++)
            {
                if (i >= 9)
                {
                    int index = Random.Range(0, powerArrage.Count);
                    if (i % 3 != 0)
                        continue;

                    power_Object[i + 0].Init((int)powerArrage[index].x);
                    power_Object[i + 1].Init((int)powerArrage[index].y);
                    power_Object[i + 2].Init((int)powerArrage[index].z);

                }
                else
                {
                    powerIndex = power_Object[i + 9].powerIndex;
                    power_Object[i].Init(powerIndex);
                }
            }
        }


        //if (!triggerOnce)
        //{
        //    for (int i = 0; i < power_Object.Count; i++)
        //    {
        //        powerIndex = 0;
        //        power_Object[i].Init(powerIndex);
        //    }
        //    triggerOnce = true;
        //}
        //else
        //{
        //    // ensure the last 9 is move to the first 9
        //    for (int i = 0; i < power_Object.Count; i++)
        //    {
        //        if (i >= 9)
        //        {
        //            powerIndex = Random.Range(0, 3);

        //            #region Ensure line has 3 bomb in a row
        //            if (i % 3 == 0)
        //            {
        //                avoidBombLine = 0;
        //            }

        //            if (powerIndex == 1)
        //                avoidBombLine++;


        //            if (avoidBombLine >= 3)
        //            {
        //                powerIndex = 2;
        //                avoidBombLine = 0;
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            powerIndex = power_Object[i + 9].powerIndex;
        //        }

        //        power_Object[i].Init(powerIndex);
        //    }

        //}
    }

    public override void Update()
    {
        base.Update();

        if (startGame)
        {
            timer_text.text = Mathf.Round(timer_value).ToString();
            timer_value -= Time.deltaTime;
            distant_value += Time.deltaTime;

            score_value = Mathf.Round(distant_value + (player.TotalCollect() * 50));

            displayScore_text.text = score_value.ToString();

            TimerDisplay();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DOTween.KillAll(true);
        }
    }

    /// <summary>
    /// DIsplay time and shake the clock near end
    /// </summary>
    void TimerDisplay()
    {
        if (timer_value <= 0)
        {
            StopGame();
        }

        if (timer_Tween == null && timer_value <= 20)
        {
            timer_object.transform.DOShakePosition(20, new Vector3(3, 1, 0));
        }
    }

    /// <summary>
    /// Rest game and show result
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowResult()
    {
        yield return new WaitForEndOfFrame();
        DOTween.KillAll(true);
        triggerOnce = false;
        SpawnObject();

        //if (score_value > highScore)
        //    highScore = score_value;

        score_text.text = score_value.ToString();
        saveGame.SaveProgress(score_value);

        GameEventManager.RaiseMessage("Menu");
    }

    /// <summary>
    /// Get kniect Body
    /// </summary>
    void InitModelBody()
    {
        spineMid = GameObject.Find("SpineMid");
    }
}
