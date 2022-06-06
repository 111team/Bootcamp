using UnityEngine;
using Cinemachine;

public class Platform : MonoBehaviour
{
    private CharacterController _characterController;
    private CinemachineBrain _cineBrain;

    private void Start()
    {
        _cineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController character = other.GetComponent<CharacterController>();

            if (character != null)
                _characterController = character;

            if (_cineBrain != null)
            {
                //_cineBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_characterController != null && other.gameObject == _characterController.gameObject)
                _characterController = null;

            if (_cineBrain != null)
            {
                //_cineBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
            }
        }
    }

    public void MoveCharacterController(Vector3 movePosition)
    {
        if (_characterController != null)
        {
            _characterController.Move(movePosition);
        }
    }
}
