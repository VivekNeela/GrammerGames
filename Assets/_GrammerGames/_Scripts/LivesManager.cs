using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMKOC.Grammer;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public List<SpriteRenderer> heartsList;
    public Sprite heartFull;
    public Sprite heartBg;
    public int currentLives;

    private void OnEnable()
    {
        Collector.OnWrongAnswer += ReduceLife;
    }

    private void OnDisable()
    {
        Collector.OnWrongAnswer += ReduceLife;
    }


    private void ReduceLife()
    {
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
    }



}
