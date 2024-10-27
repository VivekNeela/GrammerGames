using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Common;
using Sirenix.OdinInspector;
using TMKOC.Grammer;
// using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
namespace TMKOC.Grammer
{

    public class FlashCardHandler : MonoBehaviour
    {
        //this flash card list wrapper is not being used anywhere can remove later...
        private FlashCardListWrapper flashCardListWrapper;
        public List<FlashCardData> flashCardDatas;
        public List<Collectable> flashCards;
        public List<Collectable> wordCards;

        public GameObject wordBasket;
        public GameObject progressBar;
        public GameObject levelConfetti;
        public static event Action<string, Sprite, GrammerType, int> SetFlashCardData;

        public static Action<string, Sprite, GrammerType, int> OnNextButtonClicked;
        public static event Action OnGameOver;
        public static event Action<string, int> ChangeTitle;
        // public static event Action<bool> EnableCardsDragging;
        public static event Action<bool, int, bool> ResetCollectablePos;
        public static event Action<bool> SetNextBtnState;
        public static event Action<Action> ScaleCardsDownToUp;   //crazy shit...
        // public static event Action<bool> EnableCollector;


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

            // Collector.OnRightAnswer += NextLevelQuiz;

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

            // Collector.OnRightAnswer -= NextLevelQuiz;

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
        private void SetActiveProgressBar(bool state) => progressBar.SetActive(state);

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
            if (TransitionHandler.Instance.inTransition) return;

            // if (GameManager.Instance.levelNumber == 6 && chunkIndex > 4)
            // {
            //     TestOver();
            // }

            if (GameManager.Instance.levelNumber >= 4 && GameManager.Instance.levelNumber <= 6 && chunkIndex > 4)
            {
                TestOver();
            }

            if (chunkIndex > 5)
            {
                TestOver();
            }

            void TestOver()
            {
                Debug.Log("Test is over");
                levelConfetti.SetActive(true);
                // EnableCardsDragging?.Invoke(false);   //game over so no dragging...
                
                if (GameManager.Instance.levelNumber != 6)
                    GameManager.Instance.LoadSelection();
                return;
            }


            //scale down all cards
            ScaleCardsDownToUp?.Invoke(() =>
            {

                int cardCount = 0;

                cardCount = GameManager.Instance.cardType == CardType.FlashCard ? 3 : 5;

                int startIndex = chunkIndex * cardCount;

                if (startIndex < flashCardDatas.Count)
                {

                    var chunk = flashCardDatas.GetRange(startIndex, Mathf.Min(cardCount, flashCardDatas.Count - startIndex));

                    Debug.Log("chunk data is :: " + chunk[0].word);

                    //assign data to the falsh cards...
                    for (int i = 0; i < cardCount; i++)
                    {
                        var data = chunk[i];
                        OnNextButtonClicked?.Invoke(data.word, data.image, data.grammerType, i);

                        //SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                    }

                    chunkIndex++;

                }
                else
                {
                    GameManager.Instance.currentLevel = LevelType.LevelQuiz;
                    //play confetti 
                    // levelConfetti.SetActive(true);

                    SetActiveWordBasket(true);
                    SetActiveProgressBar(true);

                    var pm = progressBar.GetComponent<ProgressManager>();
                    pm.EnableStars(LevelType.LevelQuiz);

                    // EnableCardsDragging?.Invoke(true);
                    ChangeTitle?.Invoke("Which Word is a Noun ?", 1200);
                    Debug.Log("<color=yellow> no more elements...Now we take test...</color>");

                    // EnableCollector?.Invoke(true);
                    SetNextBtnState?.Invoke(false);

                    //assign data to the falsh cards...

                    var chunk = GetLevelTestCards();

                    //assign data to the falsh cards...
                    for (int i = 0; i < cardCount; i++)
                    {
                        var data = chunk[i];
                        OnNextButtonClicked?.Invoke(data.word, data.image, data.grammerType, i);

                        //  SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                    }

                    chunkIndex++;
                }


            });  //should happen before the cards data has changed


        }


        // private void NextLevelQuiz(int index)   //this runs on the progress manager on right answer event
        // {
        //     if (GameManager.Instance.levelNumber == 6) return;
        //     // PlayCardTransition?.Invoke(0, .8f);
        //     ResetCollectablePos?.Invoke(true, index, true);
        //     NextBtnLoop();
        // }






        [SerializeField] private List<FlashCardData> previousChunk;
        private List<FlashCardData> GetLevelTestCards()
        {
            System.Random random = new System.Random();

            int cardCount = flashCardDatas.Count;

            var nouns = flashCardDatas;
            var incorrectWords = GameManager.Instance.grammerTypeDataSO.incorrectWords;

            var filtereedNouns = nouns.Except(previousChunk).ToList();
            var filteredIncorrectWords = incorrectWords.Except(previousChunk).ToList();

            var chunk = new List<FlashCardData>();

            var a = filtereedNouns[random.Next(filtereedNouns.Count)];
            chunk.Add(a);

            var b = filteredIncorrectWords.OrderBy(x => random.Next()).Take(2).ToList();
            chunk.AddRange(b);

            chunk = chunk.OrderBy(x => random.Next()).ToList();

            previousChunk = chunk;

            return chunk;
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
                OnGameOver?.Invoke();  //resets the quiz games played to zero...
                return;
            }
            NextBtnLoop();
        }











    }

}