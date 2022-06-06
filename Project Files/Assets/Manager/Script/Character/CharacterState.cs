using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterState : MonoBehaviour
{
    public Transform rayPoint;
    public float rayDistance = 5f;
    UnityEvent OnDashControl;
    public Dash dashScript;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private RaycastHit rayHit;
    private bool dashControl = true;

    private void Start()
    {
        Transform startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        spawnPosition = startPoint.position;
        spawnRotation = startPoint.rotation;
        dashControl = true;

        if (OnDashControl == null)
            OnDashControl = new UnityEvent();

        OnDashControl.AddListener(dashScript.KillDOTween);
    }

    private void Update()
    {
        if(Physics.Raycast(rayPoint.position, transform.forward, out rayHit, rayDistance) && dashControl)
        {
            dashScript.isDash = false;
            dashScript.KillDOTween();
            dashControl = false;
        }
        else if(Physics.Raycast(rayPoint.position, transform.forward, out rayHit, rayDistance) == false)
        {
            dashControl = true;
            dashScript.isDash = true;
        }

        Debug.DrawRay(rayPoint.position, transform.forward * rayDistance, Color.red);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SpawnVolume"))
        {
            spawnPosition = other.transform.GetChild(0).position;
            spawnRotation = other.transform.GetChild(0).rotation;
        }

        if (other.CompareTag("DeadZone"))
        {
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
        }
    }
}
