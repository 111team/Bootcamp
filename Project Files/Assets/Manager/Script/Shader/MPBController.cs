using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBController : MonoBehaviour
{
    [Range(0f, 1f)]
    [HideInInspector] public float fresnelAmount; 

    private Renderer _renderer = null;
    private MaterialPropertyBlock _materialPropertyBlock = null;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        _materialPropertyBlock.SetFloat("Fresnel_Amount", fresnelAmount);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
