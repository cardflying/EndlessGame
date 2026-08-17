using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private ParticleSystem particleRing;
    [SerializeField]
    private BoxCollider particleCollider;
    [SerializeField]
    private List<Texture> timerSprite = new List<Texture>();
    [SerializeField]
    private MeshRenderer bilboard;

    public int powerIndex;

    private ParticleSystem.MainModule mainModule; 

    /// <summary>
    /// Define type of power based on powerIndex
    /// </summary>
    /// <param name="_powerIndex"></param>
    public void Init(int _powerIndex)
    {
        powerIndex = _powerIndex;

        switch (powerIndex)
        {
            case 0: // add timer
                ChangeColorImmediately(particle, Color.green);
                ChangeColorImmediately(particleRing, Color.green);
                particle.Play();
                bilboard.material.mainTexture = timerSprite[0];
                bilboard.gameObject.SetActive(true);
                particleCollider.enabled = true;
                break;
            case 1: // minus timer
                ChangeColorImmediately(particle, Color.red);
                ChangeColorImmediately(particleRing, Color.red);
                particle.Play();
                bilboard.material.mainTexture = timerSprite[1];
                bilboard.gameObject.SetActive(true);
                particleCollider.enabled = true;
                break;
            case 2: // none
                Hide();
                break;
        }
    }
    /// <summary>
    /// Change the particle color
    /// </summary>
    /// <param name="_particle"></param>
    /// <param name="_newColor"></param>
    public void ChangeColorImmediately(ParticleSystem _particle, Color _newColor)
    {
        // 1. Access the main module
        mainModule = _particle.main;

        // 2. Assign the new color to future emissions
        mainModule.startColor = _newColor;

        // 3. Force-change already living particles instantly
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particle.main.maxParticles];
        int aliveCount = _particle.GetParticles(particles);

        for (int i = 0; i < aliveCount; i++)
        {
            particles[i].startColor = _newColor;
        }

        // 4. Apply data back to the system
        _particle.SetParticles(particles, aliveCount);
    }

    /// <summary>
    /// Hide the power point
    /// </summary>
    public void Hide()
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleRing.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        bilboard.gameObject.SetActive(false);
        particleCollider.enabled = false;
    }
}
