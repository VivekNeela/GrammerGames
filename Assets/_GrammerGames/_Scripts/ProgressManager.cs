using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Slider progressSlider;
    public List<GameObject> stars;
    public int score;

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
        progressSlider = GetComponent<Slider>();
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
        }
    }

    private void ScaleUpStar(int index) => stars[index].transform.DOScale(1, .5f);

}
