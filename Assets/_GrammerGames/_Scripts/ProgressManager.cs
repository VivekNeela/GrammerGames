using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public GameObject Stars_5;
    public GameObject Stars_3;
    public Slider progressBar;
    // public Slider progressBar_3;
    public List<GameObject> stars_5_List;
    public List<GameObject> stars_3_List;
    public int maxScore;
    // [SerializeField] private float tempScore;
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            GameManager.Instance.currentScore = score;
        }
    }

    public float sliderValue = 0;
    public static event Action ShowNextFlashCards;
    public static event Action<bool, int, bool> ResetCollectablePos;
    // public static event Action ScaleUpCards;


    private void OnEnable()
    {
        Collector.OnRightAnswer += GiveStar_Quiz;
        Collector.OnRightAnswer += GiveStar_LevelQuiz;

        GameManager.OnResetQuiz += ResetProgress;

    }

    private void OnDisable()
    {
        Collector.OnRightAnswer -= GiveStar_Quiz;
        Collector.OnRightAnswer -= GiveStar_LevelQuiz;

        GameManager.OnResetQuiz -= ResetProgress;
    }

    private void Start()
    {
        ScaleDownStars();
        maxScore = GameManager.Instance.cardType == CardType.FlashCard ? 5 : 10;
    }

    private void ScaleDownStars()
    {
        foreach (var item in stars_5_List)
        {
            item.transform.DOScale(0, 0);
        }
        foreach (var item in stars_3_List)
        {
            item.transform.DOScale(0, 0);
        }
    }

    //suppose the max score is 10 we will add 2 points every time we call give star.
    private void GiveStar_Quiz(int index)
    {
        if (GameManager.Instance.currentLevel != LevelType.Quiz) return;
        if (Score < maxScore)
        {
            Score += maxScore / 5;
            int starIndex = GameManager.Instance.cardType == CardType.FlashCard ? Score - 1 : (Score / 2) - 1;

            ScaleUpStar(starIndex);
            if (Score > maxScore / 5)
            {
                StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
                sliderValue += .25f;
            }
            else
            {
                sliderValue += .25f;
            }
        }
        //need to invoke an event that sets new flash cards...
        ShowNextFlashCards?.Invoke();
        ResetCollectablePos?.Invoke(true, index, true);
    }



    private void GiveStar_LevelQuiz(int index)
    {
        if (GameManager.Instance.currentLevel != LevelType.LevelQuiz) return;
        if (Score < 3)
        {
            Score += 1;
            ScaleUpStar(Score - 1);

            if (Score > 1)
            {
                StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
                sliderValue += .5f;
            }
            else
            {
                sliderValue += .5f;
            }
        }

        ShowNextFlashCards?.Invoke();
        ResetCollectablePos?.Invoke(true, index, true);
    }


    private void ScaleUpStar(int index)
    {
        if (GameManager.Instance.currentLevel == LevelType.Quiz)
            stars_5_List[index].transform.DOScale(1, .5f);
        else
            stars_3_List[index].transform.DOScale(1, .5f);
    }

    IEnumerator IncreaseSliderValue(float targetValue, float duration)
    {
        float startValue = progressBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            progressBar.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }
        // Ensure slider reaches the target value exactly at the end
        progressBar.value = targetValue;
    }

    private void ResetProgress()
    {
        progressBar.value = 0;
        Score = 0;
        sliderValue = 0;
        ScaleDownStars();
    }

    // private void AddScore(float _tempScore, int index)
    // {
    //     tempScore += _tempScore;

    //     if (IsWholeNumber(tempScore))
    //     {
    //         Debug.Log("<color=green> tempscore is whole number we can give star !!!</color>");
    //         //dont need to invoke this event cuz it is already being invoked in give star() 
    //         // ResetCollectablePos?.Invoke(true, index);
    //         GiveStar_Quiz(index);
    //         //show next cards also...
    //     }
    //     else
    //     {
    //         // StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
    //         // sliderValue += .125f;
    //         ResetCollectablePos?.Invoke(false, index, true);

    //         //dont show next cards...
    //         //need to scale down the card...
    //     }
    // }

    private bool IsWholeNumber(float num)
    {
        return num % 1 == 0;
    }

    public void EnableStars(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.Quiz:
                Stars_5.SetActive(true);
                Stars_3.SetActive(false);
                break;

            case LevelType.LevelQuiz:
                Stars_3.SetActive(true);
                Stars_5.SetActive(false);
                break;

            default:
                break;
        }
    }

}

