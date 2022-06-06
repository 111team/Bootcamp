using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Trigger : MonoBehaviour
{
    public enum TriggerType { Single, Multiple };

    [SerializeField] private TriggerType triggerType;
    [SerializeField] private GameObject targetObject;
    [Space(10f)]
    public UnityEvent OnEnter;

    private BoxCollider _collider;
    private Mover targetMover;
    private bool singleTrigger;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        switch (triggerType)
        {
            case TriggerType.Single:
                singleTrigger = true;
                break;

            case TriggerType.Multiple:
                singleTrigger = false;
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerType == TriggerType.Single && singleTrigger)
            {
                OnEnter.Invoke();
                singleTrigger = false;

            }
            else if (triggerType == TriggerType.Multiple)
            {
                OnEnter.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (targetObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetObject.transform.position);
        }
    }
}
