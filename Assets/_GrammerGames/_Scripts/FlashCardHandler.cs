using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using TMKOC.Grammer;
using Unity.Mathematics;
using UnityEngine;
namespace TMKOC.Grammer
{

    public class FlashCardHandler : MonoBehaviour
    {
        //this flash card list wrapper is not being used anywhere can remove later...
        private FlashCardListWrapper flashCardListWrapper;   //this will set all the flash cards data
        public List<FlashCardData> flashCardDatas;
        public List<Collectable> flashCards;
        public GameObject wordBasket;
        public static event Action<string, Sprite, GrammerType, int> SetFlashCardData;
        public static event Action OnGameOver;
        // public bool cardsMoving;
        // public bool cardsPosReset;


        private void OnEnable()
        {
            Collectable.OnSelected += SelectedCard;
            Collectable.OnDeselected += DeselectedCard;
            CanvasHandler.EnableWordBasket += SetActiveWordBasket;
            GameManager.SetFlashCardData += SetFlashCardDataList;
            GameManager.SetQuizCardsData += SetFlashCardDataList;
            GameManager.ResetFlashCardsIndex += ResetFlashCardsIndex;
            ProgressManager.ShowNextFlashCards += NextBtnLoop;
            LivesManager.ShowNextFlashCards += NextBtnLoop;

            Collectable.ShowNextCards += ShowNext;

        }

        private void OnDisable()
        {
            Collectable.OnSelected -= SelectedCard;
            Collectable.OnDeselected -= DeselectedCard;
            CanvasHandler.EnableWordBasket -= SetActiveWordBasket;
            GameManager.SetFlashCardData -= SetFlashCardDataList;
            GameManager.SetQuizCardsData -= SetFlashCardDataList;
            GameManager.ResetFlashCardsIndex -= ResetFlashCardsIndex;
            ProgressManager.ShowNextFlashCards -= NextBtnLoop;
            LivesManager.ShowNextFlashCards -= NextBtnLoop;

            Collectable.ShowNextCards -= ShowNext;
        }


        //useless...
        // private void SetCollectableFlashCards(int index)
        // {
        //     flashCards[index].word = flashCardDatas[index].word;
        //     flashCards[index].image = flashCardDatas[index].image;
        //     flashCards[index].grammerType = flashCardDatas[index].grammerType;
        // }


        private void SelectedCard(int index)
        {
            // cardsMoving = true;
            Debug.Log("selected ::" + index);

            foreach (var item in flashCards)
            {
                item.BoxCollider.isTrigger = true;
                item.SortingGroup.sortingOrder = 0;
                item.TextCanvas.sortingOrder = 0;
            }
            flashCards[index].BoxCollider.isTrigger = false;
            flashCards[index].SortingGroup.sortingOrder = 1;
            flashCards[index].TextCanvas.sortingOrder = 1;
        }

        private void DeselectedCard(int index)
        {
            // cardsMoving = false;
        }

        private void SetActiveWordBasket(bool state) => wordBasket.SetActive(state);

        private void SetFlashCardDataList(FlashCardListWrapper flashCardListWrapper)
        {
            this.flashCardListWrapper = flashCardListWrapper;
            flashCardDatas = flashCardListWrapper.listOfFlashCards;

            for (int i = 0; i < flashCards.Count; i++)
            {
                var data = flashCardDatas[i];
                SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
            }
        }


        // public void SetFlashCardDataLoop()
        // {
        //     if (flashCardDatas.Count < 6) return;
        //     for (int i = 3; i < GameManager.Instance.currentFlashCardData.listOfFlashCards.Count; i++)   //cuz only 3 flash cards at once...
        //     {
        //         var data = GameManager.Instance.currentFlashCardData.listOfFlashCards[i];
        //         flashCards[i - 3].Index = i;
        //         Debug.Log("name:: " + data.word);
        //         SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
        //     }
        // }


        public int chunkIndex = 1;
        public void NextBtnLoop()
        {
            int startIndex = chunkIndex * 3;

            if (startIndex < flashCardDatas.Count)
            {

                var chunk = flashCardDatas.GetRange(startIndex, Mathf.Min(3, flashCardDatas.Count - startIndex));

                Debug.Log("chunk data is :: " + chunk[0].word);

                //assign data to the falsh cards...
                for (int i = 0; i < 3; i++)
                {
                    var data = chunk[i];
                    SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                }

                chunkIndex++;

            }
            else
            {
                Debug.Log("no more elements...");
            }

        }



        private void SetQuizCardsDataList(FlashCardListWrapper flashCardListWrapper)   //same function but need to add randomness later on...
        {
            this.flashCardListWrapper = flashCardListWrapper;
            flashCardDatas = flashCardListWrapper.listOfFlashCards;

            for (int i = 0; i < flashCards.Count; i++)
            {
                var data = flashCardDatas[i];
                SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
            }
        }

        private void ResetFlashCardsIndex()
        {
            chunkIndex = 1;
            for (int i = 0; i < 3; i++)
            {
                flashCards[i].Index = i;
            }
        }


        private void ShowNext()
        {
            GameManager.Instance.QuizGamesPlayed += 1;
            Debug.Log("<color=green>now we need to show next cards...,</color>");
            if (GameManager.Instance.QuizGamesPlayed > 5)
            {
                Debug.Log("Game Over ! ");
                OnGameOver?.Invoke();
                return;
            }
            NextBtnLoop();
        }










    }

}