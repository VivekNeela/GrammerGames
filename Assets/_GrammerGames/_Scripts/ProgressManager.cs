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
    public int score;
    public float sliderValue = 0;

    private void OnEnable()
    {
        Collector.OnRightAnswer += GiveStar;
    }

    private void OnDisable()
    {
        Collector.OnRightAnswer -= GiveStar;
    }

    private void Start()
    {
        progressBar = GetComponent<Slider>();
        ScaleDownStars();
    }

    private void ScaleDownStars()
    {
        foreach (var item in stars)
        {
            item.transform.DOScale(0, 0);
        }
    }

    private void GiveStar()
    {
        if (score < 5)
        {
            score += 1;
            ScaleUpStar(score - 1);
            if (score > 1)
            {
                StartCoroutine(IncreaseSliderValue(sliderValue, .5f));
                sliderValue += .25f;
            }
            else
            {
                sliderValue += .25f;
            }
        }
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

}
