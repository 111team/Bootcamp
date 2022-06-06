using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private ParticleSystem activeParticle;

    private AudioSource switchAudio = null;
    private Animator _animator;

    private void Awake()
    {
        switchAudio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public void SwitchActive()
    {
        switchAudio.Play();
        _animator.SetTrigger("Active");

        if (activeParticle != null)
            activeParticle.Play();
    }
}
