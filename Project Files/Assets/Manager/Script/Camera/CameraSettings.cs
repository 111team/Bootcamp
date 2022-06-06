using UnityEngine;
using Cinemachine;

public class CameraSettings : MonoBehaviour
{
    private CinemachineBrain _cineBrain;

    private void Awake()
    {
        _cineBrain = GetComponent<CinemachineBrain>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _cineBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
    }

    
}
