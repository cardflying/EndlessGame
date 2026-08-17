using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 3..2..1.. Count down display
/// </summary>
public class CountDown : MonoBehaviour
{
    [SerializeField]
    private TMP_Text countdownText;
    [SerializeField]
    private int startValue = 3;
    [SerializeField]
    private SoundController soundController;

    public Action completeCallback;

    public void StartCountdown()
    {
        soundController.PlayEffect(2);
        RunCountdownStep(startValue);
    }

    private void RunCountdownStep(int number)
    {
        if (number <= 0)
        {
            countdownText.text = "GO!";
            TriggerGoEffect();

            return;
        }

        // 1. Reset text, scale, and alpha
        countdownText.text = number.ToString();
        countdownText.transform.localScale = Vector3.zero;
        countdownText.alpha = 0f;

        // 2. Create animation sequence
        Sequence numSequence = DOTween.Sequence();

        numSequence.Append(countdownText.transform.DOScale(1.8f, 0.4f).SetEase(Ease.OutBack));
        numSequence.Join(countdownText.DOFade(1f, 0.2f));

        // 3. Fade away and shrink slightly before next number
        numSequence.AppendInterval(0.3f);
        numSequence.Append(countdownText.DOFade(0f, 0.3f));
        numSequence.Join(countdownText.transform.DOScale(1.2f, 0.3f));

        // 4. Loop to next number
        numSequence.OnComplete(() => RunCountdownStep(number - 1));
    }

    private void TriggerGoEffect()
    {
        countdownText.transform.localScale = Vector3.zero;
        countdownText.alpha = 0f;

        Sequence goSequence = DOTween.Sequence();
        goSequence.Append(countdownText.transform.DOScale(2f, 0.5f).SetEase(Ease.OutElastic));
        goSequence.Join(countdownText.DOFade(1f, 0.1f));
        goSequence.AppendInterval(0.5f);
        goSequence.Append(countdownText.DOFade(0f, 0.5f));

        goSequence.OnComplete(() =>
        {
            if (completeCallback != null)
                completeCallback();
        });
    }
}
