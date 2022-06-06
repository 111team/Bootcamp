using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public GameObject selectionObject;
    public Button buttonObject;

    public void ButtonEnter()
    {
        selectionObject.SetActive(true);
        selectionObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .08f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo);
        buttonObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .08f).SetEase(Ease.Linear).SetLoops(3, LoopType.Yoyo);
    }

    public void ButtonExit()
    {
        selectionObject.transform.DOScale(new Vector3(1f, 1f, 1f), .1f).SetEase(Ease.Linear);
        buttonObject.transform.DOScale(new Vector3(1f, 1f, 1f), .1f).SetEase(Ease.Linear);
        selectionObject.SetActive(false);
    }
}
