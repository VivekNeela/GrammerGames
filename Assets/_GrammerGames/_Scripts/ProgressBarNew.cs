using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarNew : MonoBehaviour
{
    public Slider progressBar;
    public List<GameObject> stars;
    public int score;
    public int maxScore;
    public float sliderValue;
    public static event Action<bool, int, bool> ResetCollectablePos;


    private void OnEnable()
    {
        Collector.OnRightAnswer += AddProgress;
        GameManager.OnResetQuiz += ResetProgress;
    }

    private void OnDisable()
    {
        Collector.OnRightAnswer -= AddProgress;
        GameManager.OnResetQuiz -= ResetProgress;
    }

    private void Start()
    {
        ScaleDownStars();
    }


    [Button]
    private void AddProgress(int index)
    {
        if (score < maxScore)
        {
            score += 1;

            if (score % 2 == 0)
            {
                Debug.Log("scale up the star...");
                index = score == 1 ? 0 : (score / 2) - 1;

                ScaleUpStar(index);
            }

            if (score > 1)
            {
                StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
                sliderValue += .125f;
            }
            else
            {
                sliderValue += .125f;
            }

            if (score % 2 != 0)
                ResetCollectablePos?.Invoke(false, index, true);
            else
                ResetCollectablePos?.Invoke(true, index, true);

        }
        //need to invoke an event that sets new flash cards...
        // ShowNextFlashCards?.Invoke();

        //if score is odd only the we can keep the first bool true...
        //do tmrw

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


    private void ScaleDownStars()
    {
        foreach (var item in stars)
        {
            item.transform.DOScale(0, 0);
        }
    }




}
