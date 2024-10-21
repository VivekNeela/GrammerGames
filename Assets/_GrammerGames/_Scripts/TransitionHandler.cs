using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMKOC.Grammer;
using UnityEditorInternal;
using UnityEngine;

public class TransitionHandler : SerializedSingleton<TransitionHandler>
{
    public bool inTransition;
    public List<GameObject> flashCards;

    private void OnEnable()
    {
        GameManager.ScaleCardsOneByOne += ScaleCardsOneByOne;
        GameManager.ScaleDownCards += ScaleDownCards;
        FlashCardHandler.ScaleCardsDownToUp += ScaleCardsDownToUp;
    }

    private void OnDisable()
    {
        GameManager.ScaleCardsOneByOne -= ScaleCardsOneByOne;
        GameManager.ScaleDownCards -= ScaleDownCards;
        FlashCardHandler.ScaleCardsDownToUp -= ScaleCardsDownToUp;
    }


    private void ScaleCardsOneByOne(Action callback)
    {
        // Sequence sequence = DOTween.Sequence();
        // foreach (var item in flashCards)
        // {
        //     sequence.Append(item.transform.DOScale(.8f, .5f).SetEase(Ease.InOutQuad)).OnComplete(() =>
        //     {
        //         Debug.Log("all are cards scaled::");
        //         // callback?.Invoke();
        //     });
        // }
        inTransition = true;
        flashCards[0].transform.DOScale(.8f, .5f).OnComplete(() =>
        {
            flashCards[1].transform.DOScale(.8f, .5f).OnComplete(() =>
            {
                flashCards[2].transform.DOScale(.8f, .5f).OnComplete(() =>
                {
                    Debug.Log("all are cards scaled::");

                    callback?.Invoke();
                    inTransition = false;
                });

            });

        });
    }


    private void ScaleDownCards(Action callback)
    {
        // inTransition = true;
        flashCards[0].transform.DOScale(0, .5f).OnComplete(() =>
        {
            flashCards[1].transform.DOScale(0, .5f).OnComplete(() =>
            {
                flashCards[2].transform.DOScale(0, .5f).OnComplete(() =>
                {
                    // ScaleCardsOneByOne(() => { callback?.Invoke(); });
                    callback?.Invoke();
                    // inTransition = false;
                });

            });

        });

    }


    private void ScaleCardsDownToUp(Action setCardsCallback)
    {
        inTransition = true;
        ScaleDownCards(() =>
        {
            setCardsCallback?.Invoke();
            ScaleCardsOneByOne(() => { inTransition = false; });
        });

    }


}
