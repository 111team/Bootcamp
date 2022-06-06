using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPB_Switch : MaterialProperty
{
    private float emisivePower;

    // Update is called once per frame
    void Update()
    {
        _materialPropertyBlock.SetFloat("Emisive_Power", emisivePower);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    public void SetEmisiveAmount(float amount)
    {
        emisivePower = amount;
    }
}
