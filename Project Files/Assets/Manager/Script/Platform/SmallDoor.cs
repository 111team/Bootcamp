using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDoor : MonoBehaviour
{
    private Mover _mover;
    private AudioSource _activeSound;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _activeSound = GetComponent<AudioSource>();
    }

    public void ActiveSmallDoor()
    {
        _mover.activate = true;
        if (_activeSound != null) _activeSound.Play();
    }
}
