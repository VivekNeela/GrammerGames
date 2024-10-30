using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public List<Image> heartsList;
    public Sprite heartFull;
    public Sprite heartBg;
    public int currentLives;

    public static event Action ShowNextFlashCards;
    public static event Action<bool, int, bool> ResetCollectablePos;



    private void OnEnable()
    {
        Collector.OnWrongAnswer += ReduceLife;
        GameManager.OnResetQuiz += ResetLives;

    }

    private void OnDisable()
    {
        Collector.OnWrongAnswer -= ReduceLife;
        GameManager.OnResetQuiz -= ResetLives;
    }

    private void Start() => currentLives = 5;



    private void ReduceLife(int index)
    {
        Debug.Log("reduce one life...");
        if (currentLives > 0)
            currentLives -= 1;

        foreach (var item in heartsList)
        {
            item.sprite = heartBg;
        }
        for (int i = 0; i < currentLives; i++)
        {
            heartsList[i].sprite = heartFull;
        }
        //event to go next ...
        ShowNextFlashCards?.Invoke();
        ResetCollectablePos?.Invoke(true, index, true);
    }

    private void ResetLives()
    {
        currentLives = 5;
        foreach (var item in heartsList)
        {
            item.sprite = heartFull;
        }
    }


    private void ShowLevelQuizStars(bool state)
    {
        for (int i = 0; i < 2; i++)
        {
            heartsList[i].gameObject.SetActive(false);
        }
    }



}
