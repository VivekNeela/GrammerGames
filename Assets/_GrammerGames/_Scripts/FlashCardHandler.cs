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
        public List<Collectable> wordCards;

        public GameObject wordBasket;
        public static event Action<string, Sprite, GrammerType, int> SetFlashCardData;
        public static event Action OnGameOver;


        private void OnEnable()
        {
            Collectable.OnSelected += SelectedCard;
            Collectable.OnDeselected += DeselectedCard;
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
            Debug.Log("selected ::" + index);

            if (GameManager.Instance.cardType == CardType.FlashCard)
                SetCardsProperties(flashCards);
            else
                SetCardsProperties(wordCards);


            void SetCardsProperties(List<Collectable> cardsList)
            {
                foreach (var item in cardsList)
                {
                    item.BoxCollider.isTrigger = true;
                    item.SortingGroup.sortingOrder = 0;
                    item.TextCanvas.sortingOrder = 0;
                }
                cardsList[index].BoxCollider.isTrigger = false;
                cardsList[index].SortingGroup.sortingOrder = 1;
                cardsList[index].TextCanvas.sortingOrder = 1;
            }
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

            if (GameManager.Instance.cardType == CardType.FlashCard)
            {
                for (int i = 0; i < flashCards.Count; i++)
                {
                    var data = flashCardDatas[i];
                    SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                }
            }
            else
            {
                Debug.Log("word cards selected...");
                for (int i = 0; i < wordCards.Count; i++)
                {
                    var data = flashCardDatas[i];
                    SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                }
            }
        }




        public int chunkIndex = 1;
        public void NextBtnLoop()
        {
            int cardCount = 0;

            if (GameManager.Instance.cardType == CardType.FlashCard)
                cardCount = 3;
            else
                cardCount = 5;


            int startIndex = chunkIndex * cardCount;

            if (startIndex < flashCardDatas.Count)
            {

                var chunk = flashCardDatas.GetRange(startIndex, Mathf.Min(cardCount, flashCardDatas.Count - startIndex));

                Debug.Log("chunk data is :: " + chunk[0].word);

                //assign data to the falsh cards...
                for (int i = 0; i < cardCount; i++)
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