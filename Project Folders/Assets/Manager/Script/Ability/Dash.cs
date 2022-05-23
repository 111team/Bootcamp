using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [SerializeField] private Ability abilityObject;
    [Header("Visuals")]
    [SerializeField] private List<Renderer> skinnedMesh = default;
    [SerializeField] private ParticleSystem dashParticle = default;
    [SerializeField] private Volume dashVolume = default;

    private Sequence dashSequence;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DashEvent(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            _animator.SetTrigger("dash");
            FresnelAnimation(true);
            if (dashParticle) dashParticle.Play();

            dashSequence = DOTween.Sequence();
            dashSequence.Insert(0, transform.DOMove(transform.position + (transform.forward * 5), .2f))
            .AppendCallback(() => FresnelAnimation(false)).AppendCallback(() => dashParticle.Stop());

           /*.Insert(0, transform.DOMove(transform.position + (transform.forward * 5), .2f))
           .OnComplete(() => FresnelAnimation(true));//.OnComplete(() => FresnelAnimation(false));*/
           
           //.AppendCallback(() => FresnelAnimation(true)).OnComplete(() => FresnelAnimation(false));
           //.Insert(0, skinnedMesh. .DOFloat(1, "Fresnel_Amount", .1f));
           //.Append(skinnedMesh.material.DOFloat(0, "Fresnel_Amount", .35f));

        }


    }

    #region Fresnel Animation

    void FresnelAnimation(bool transition)
    {
        if(transition)
        {
            foreach (Renderer item in skinnedMesh)
            {
                 dashSequence.Append(item.material.DOFloat(1, "Fresnel_Amount", .1f));
            }
        }
        else
        {
            foreach (Renderer item in skinnedMesh)
            {
                dashSequence.Append(item.material.DOFloat(0, "Fresnel_Amount", .35f));         
            }
        }
        
    }
    #endregion
}
