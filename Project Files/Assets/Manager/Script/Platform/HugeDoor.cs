using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class HugeDoor : MonoBehaviour
{
    [SerializeField] private List<GameObject> doorDiamonds;
    [SerializeField] private float doorActiveTime = 1f;
    [SerializeField] private float diamondEmisiveAmount = .1f;

    private Mover _mover;
    private AudioSource _audio;
    private int diamondCount;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _audio = GetComponent<AudioSource>();
    }

    
    public void GateActive()
    {
        DoorDiamondActive();
        doorDiamonds[diamondCount - 1].GetComponent<MPB_Switch>().SetEmisiveAmount(diamondEmisiveAmount);
        if(diamondCount == doorDiamonds.Count)
        {
            Invoke(nameof(GateTimer), doorActiveTime);
        }
    }

    void GateTimer()
    {
        _mover.activate = true;
        _audio.Play();
    }

    void DoorDiamondActive()
    {
        diamondCount++;
    }


}
