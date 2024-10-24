using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMKOC.Grammer;
using UnityEngine;

public class TransitionHandler : SerializedSingleton<TransitionHandler>
{
    public bool inTransition;
    public float targetScale;
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
        Sequence sequence = DOTween.Sequence();
        inTransition = true;
        foreach (var item in flashCards)
        {
            sequence.Append(item.transform.DOScale(targetScale, .5f).SetEase(Ease.InOutQuad)).OnComplete(() =>
            {
                Debug.Log("all are cards scaled::");
                callback?.Invoke();
                inTransition = false;
            });
        }

        // inTransition = true;
        // flashCards[0].transform.DOScale(targetScale, .5f).OnComplete(() =>
        // {
        //     Debug.Log("<color=yellow> card 1 scaled </color>");

        //     flashCards[1].transform.DOScale(targetScale, .5f).OnComplete(() =>
        //     {
        //         Debug.Log("<color=yellow> card 2 scaled </color>");

        //         flashCards[2].transform.DOScale(targetScale, .5f).OnComplete(() =>
        //         {
        //             Debug.Log("<color=yellow> card 3 scaled </color>");
        //             Debug.Log("all are cards scaled::");

        //             callback?.Invoke();
        //             inTransition = false;
        //         });
        //     });
        // });

    }






    private void ScaleDownCards(Action callback)
    {
        // inTransition = true;
        // flashCards[0].transform.DOScale(0, .5f).OnComplete(() =>
        // {
        //     flashCards[1].transform.DOScale(0, .5f).OnComplete(() =>
        //     {
        //         flashCards[2].transform.DOScale(0, .5f).OnComplete(() =>
        //         {
        //             // ScaleCardsOneByOne(() => { callback?.Invoke(); });
        //             callback?.Invoke();
        //             // inTransition = false;
        //         });
        //     });
        // });


        Sequence sequence = DOTween.Sequence();
        // inTransition = true;
        foreach (var item in flashCards)
        {
            sequence.Append(item.transform.DOScale(0, .5f).SetEase(Ease.InOutQuad)).OnComplete(() =>
            {
                // Debug.Log("all are cards scaled::");
                callback?.Invoke();
                // inTransition = false;
            });
        }


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
