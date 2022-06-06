using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using Cinemachine;

public class Dash : MonoBehaviour
{
    [SerializeField] private Ability abilityObject;

    [Header("Dash Properties")]
    [Range(2f, 15f)]
    [SerializeField] private float dashAmount = 5f;
    [Range(.1f, 1f)]
    [SerializeField] private float dashTime = .2f;

    [Header("Visuals")]
    [SerializeField] private List<MPBController> skinnedMesh = default;
    [SerializeField] private ParticleSystem dashParticle = default;
    [SerializeField] private Volume dashVolume = default;
    [SerializeField] private CinemachineFreeLook cinemachineCam;

    
    public bool isDash;

    private Sequence dashSequence;
    private Animator _animator;
    private MPBController _mpbController = null;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        isDash = true;
    }

    public void DashEvent(InputAction.CallbackContext context)
    {

        if (context.started && isDash)
        {
            //Dash animasyonu için animator güncellenir.
            _animator.SetTrigger("dash");
            //Fresnel efekti baþlatýlýr.
            FresnelAnimation(true);
            //Dash particle efekt oynatýlýr.
            if (dashParticle) dashParticle.Play();

            //Dash DOTween sequence
            dashSequence = DOTween.Sequence();
            dashSequence.Insert(0, transform.DOMove(transform.position + (transform.forward * dashAmount), dashTime))
            .AppendCallback(() => FresnelAnimation(false)).AppendCallback(() => dashParticle.Stop());

            //Dash volume efekt
            DOVirtual.Float(0, 1, .1f, SetDashVolume)
                .OnComplete(() => DOVirtual.Float(1, 0, .5f, SetDashVolume));

            //Dash kamera efekt
            DOVirtual.Float(40f, 50f, dashTime, SetCameraFOV)
                .OnComplete(() => DOVirtual.Float(50f, 40f, dashTime * 3, SetCameraFOV));
        }
    }

    #region Fresnel Animation

    void FresnelAnimation(bool transition)
    {
        if (transition)
        {
            foreach (MPBController item in skinnedMesh)
            {
                DOTween.To(x => item.fresnelAmount = x, 0f, 1f, dashTime * 2f);
            }
        }
        else if(!transition)
        {
            foreach (MPBController item in skinnedMesh)
            {
                DOTween.To(x => item.fresnelAmount = x, 1f, 0f, dashTime);
            }
        }

    }
    #endregion

    
    void SetFresnelAmount(float amount)
    {
        _mpbController.fresnelAmount = amount;
    }

    void SetDashVolume(float volumeWeight)
    {
        dashVolume.weight = volumeWeight;
    }

    void SetCameraFOV(float value)
    {
        cinemachineCam.m_Lens.FieldOfView = value;
    }

    public void KillDOTween()
    {
        DOTween.KillAll();
        SetDashVolume(0f);
        FresnelAnimation(false);
        SetCameraFOV(40f);
        dashParticle.Stop();
        
    }


}
