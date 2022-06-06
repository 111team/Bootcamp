using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour
{
    [SerializeField] private float animAmount = 2f;
    [SerializeField] private float animTime = 2f;

    private GameManager _gameManager;
    

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();    
    }

    private void OnEnable()
    {
        //transform.DOMoveY(transform.position.y + animAmount, animTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") )
        {
            _gameManager.keys.Add(gameObject);
            gameObject.SetActive(false);
        }
    }


}
