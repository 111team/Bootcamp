using UnityEngine;

public abstract class MaterialProperty : MonoBehaviour
{
    protected Renderer _renderer = null;
    protected MaterialPropertyBlock _materialPropertyBlock = null;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    
}
