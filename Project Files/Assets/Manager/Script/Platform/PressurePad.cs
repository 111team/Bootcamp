using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [SerializeField] private ParticleSystem pressurePadParticle;
    [SerializeField] private Color activeParticleColor = new Color();

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnPressurePad()
    {
        _animator.SetTrigger("active");
        pressurePadParticle.startColor = activeParticleColor;
        pressurePadParticle.startSize = 4f;
    }
}
