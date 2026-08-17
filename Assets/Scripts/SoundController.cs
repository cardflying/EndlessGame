using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource Bgm;
    [SerializeField]
    private AudioSource effect;
    [SerializeField]
    private List<AudioClip> audioClipsList = new List<AudioClip>();

    public void PlayEffect(int index)
    {
        effect.PlayOneShot(audioClipsList[index]);
    }

}
