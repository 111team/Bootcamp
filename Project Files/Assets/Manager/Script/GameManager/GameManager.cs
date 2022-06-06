using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> keys;
    public int keyNumber;
    public GameObject portal;

    private int keyCounter;

    private void Start()
    {

    }

    public void IncrementKey()
    {
        keyCounter++;
        if(keyCounter == keyNumber)
        {
            ActivePortal();
        }
        print("Key Count: " + keyCounter);
    }

    void ActivePortal()
    {
        portal.SetActive(true);
    }
}
