using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchKey : MonoBehaviour
{
    [SerializeField] private GameObject keyObject;

    private GameManager _gameManager;
    private MPB_Switch mpbSwitch;
    private bool activeSwitcKey;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        mpbSwitch = GetComponentInChildren<MPB_Switch>();
    }

    public void ActiveSwitchKey()
    {

        if(_gameManager.keys.Count > 0 && !activeSwitcKey)
        {
            mpbSwitch.SetEmisiveAmount(10f);
            keyObject.SetActive(true);
            _gameManager.keys.Remove(_gameManager.keys[_gameManager.keys.Count - 1]);
            _gameManager.IncrementKey();
            activeSwitcKey = true;
        }
    }
}
