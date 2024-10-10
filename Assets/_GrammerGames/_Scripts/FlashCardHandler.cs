using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using TMKOC.Grammer;
using UnityEngine;

public class FlashCardHandler : MonoBehaviour
{
    public List<Collectable> flashCards;

    private void OnEnable()
    {
        Collectable.OnSelected += SelectedCard;
    }

    private void OnDisable()
    {
        Collectable.OnSelected -= SelectedCard;
    }

    private void SelectedCard(int index)
    {
        Debug.Log("selected ::" + index);

        foreach (var item in flashCards)
        {
            item.BoxCollider.isTrigger = true;
            item.SpriteRenderer.sortingOrder = 0;
        }
        flashCards[index].BoxCollider.isTrigger = false;
        flashCards[index].SpriteRenderer.sortingOrder = 1;
    }

    private void SetGrammerType(GrammerType grammerType)
    {
        foreach (var item in flashCards)
        {
            item.grammerType = grammerType;
        }
    }






}
