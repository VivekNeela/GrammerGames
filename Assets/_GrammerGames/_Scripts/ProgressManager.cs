using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Slider progressBar;
    public List<GameObject> stars;
    public int maxScore;
    [SerializeField] private float tempScore;
    public int score;
    public float sliderValue = 0;
    public static event Action ShowNextFlashCards;
    public static event Action<bool, int, bool> ResetCollectablePos;
    // public static event Action ScaleUpCards;


    private void OnEnable()
    {
        Collector.OnRightAnswer += GiveStar;
        Collector.AddPoints += AddScore;
        GameManager.OnResetQuiz += ResetProgress;

    }

    private void OnDisable()
    {
        Collector.OnRightAnswer -= GiveStar;
        Collector.AddPoints -= AddScore;
        GameManager.OnResetQuiz -= ResetProgress;
    }

    private void Start()
    {
        progressBar = GetComponent<Slider>();
        ScaleDownStars();
        maxScore = GameManager.Instance.cardType == CardType.FlashCard ? 5 : 10;
    }

    private void ScaleDownStars()
    {
        foreach (var item in stars)
        {
            item.transform.DOScale(0, 0);
        }
    }

    //suppose the max score is 10 we will add 2 points every time we call give star.
    private void GiveStar(int index)
    {
        if (score < maxScore)
        {
            score += maxScore / 5;
            int starIndex = GameManager.Instance.cardType == CardType.FlashCard ? score - 1 : (score / 2) - 1;

            ScaleUpStar(starIndex);
            if (score > maxScore / 5)
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


    private void ScaleUpStar(int index) => stars[index].transform.DOScale(1, .5f);

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
        score = 0;
        sliderValue = 0;
        ScaleDownStars();
    }

    private void AddScore(float _tempScore, int index)
    {
        tempScore += _tempScore;

        if (IsWholeNumber(tempScore))
        {
            Debug.Log("<color=green> tempscore is whole number we can give star !!!</color>");
            //dont need to invoke this event cuz it is already being invoked in give star() 
            // ResetCollectablePos?.Invoke(true, index);
            GiveStar(index);
            //show next cards also...
        }
        else
        {
            // StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
            // sliderValue += .125f;
            ResetCollectablePos?.Invoke(false, index, true);

            //dont show next cards...
            //need to scale down the card...
        }
    }

    private bool IsWholeNumber(float num)
    {
        return num % 1 == 0;
    }



}
