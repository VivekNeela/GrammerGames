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
using UnityEngine.UIElements;
namespace TMKOC.Grammer
{

    public class FlashCardHandler : MonoBehaviour
    {
        //this flash card list wrapper is not being used anywhere can remove later...
        // private FlashCardListWrapper flashCardListWrapper;
        public List<FlashCardData> flashCardDatas;
        public List<Collectable> flashCards;
        // public List<Collectable> wordCards;   //not using anymore

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



        private void SelectedCard(int index)
        {
            Debug.Log("selected ::" + index);

            SetCardsProperties(flashCards);
            // if (GameManager.Instance.cardType == CardType.FlashCard)
            //     SetCardsProperties(flashCards);
            // else
            //     SetCardsProperties(wordCards);


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

        private void SetActiveWordBasket(bool state)
        {
            wordBasket.SetActive(state);
            // wordBasket.transform.DOScale(1, .5f).OnComplete(() =>
            // {
            //     wordBasket.transform.DOScale(1, .5f);
            // });   //does not look good...
        }

        private void SetActiveProgressBar(bool state) => progressBar.SetActive(state);

        private void SetFlashCardDataList(FlashCardListWrapper flashCardListWrapper)
        {
            // this.flashCardListWrapper = flashCardListWrapper;
            flashCardDatas = flashCardListWrapper.listOfFlashCards;

            for (int i = 0; i < flashCards.Count; i++)
            {
                var data = flashCardDatas[i];
                SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                Debug.Log("data set::" + i + "::" + data.word);
            }

            // if (GameManager.Instance.cardType == CardType.FlashCard)
            // {
            //     for (int i = 0; i < flashCards.Count; i++)
            //     {
            //         var data = flashCardDatas[i];
            //         SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
            //     }
            // }
            // else
            // {
            //     Debug.Log("word cards selected...");
            //     for (int i = 0; i < wordCards.Count; i++)
            //     {
            //         var data = flashCardDatas[i];
            //         SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
            //     }
            // }
        }



        public int chunkIndex = 1;
        public void NextBtnLoop()
        {
            if (TransitionHandler.Instance.inTransition) return;

            if (GameManager.Instance.cardType == CardType.WordCard)
            {
                NextBtnWordCards();
                return;
            }


            if (GameManager.Instance.levelNumber >= 4 && GameManager.Instance.levelNumber <= 6 && chunkIndex > 4)
            {
                TestOver();
                return;
            }

            if (chunkIndex > 5)
            {
                TestOver();
                return;
            }

            void TestOver()
            {
                Debug.Log("Test is over");
                levelConfetti.SetActive(true);
                // EnableCardsDragging?.Invoke(false);   //game over so no dragging...

                if (GameManager.Instance.levelNumber != 6)
                {
                    GameManager.Instance.LoadSelection();
                    return;
                }
                return;
            }


            //scale down all cards
            ScaleCardsDownToUp?.Invoke(() =>
            {

                int cardCount = 0;

                cardCount = GameManager.Instance.cardType == CardType.FlashCard ? 3 : 5;

                int startIndex = chunkIndex * cardCount;

                if (startIndex < flashCardDatas.Count)   //quiz...
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
                else   //level quiz...
                {
                    GameManager.Instance.currentLevel = LevelType.LevelQuiz;
                    //play confetti 
                    // levelConfetti.SetActive(true);

                    SetActiveWordBasket(true);
                    SetActiveProgressBar(true);

                    var pm = progressBar.GetComponent<ProgressManager>();
                    pm.EnableStars(LevelType.LevelQuiz);

                    // EnableCardsDragging?.Invoke(true);
                    ChangeTitle?.Invoke("Which Word is a " + GameManager.Instance.grammerType.ToString() + "?", 1200);
                    Debug.Log("<color=yellow> no more elements...Now we take test...</color>");

                    // EnableCollector?.Invoke(true);
                    SetNextBtnState?.Invoke(false);

                    //assign data to the falsh cards...

                    var chunk = GetLevelTestCards(2);

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



        public void NextBtnWordCards()   //only runs on adjectives
        {
            if (TransitionHandler.Instance.inTransition) return;

            if (GameManager.Instance.levelNumber >= 4 && GameManager.Instance.levelNumber <= 6 && chunkIndex > 4)
            {
                TestOver();
                return;
            }

            if (chunkIndex > 3 && (GameManager.Instance.levelNumber > 0 && GameManager.Instance.levelNumber < 6 || chunkIndex > 4))
            {
                TestOver();
                return;
            }

            void TestOver()
            {
                Debug.Log("Test is over");
                levelConfetti.SetActive(true);
                // EnableCardsDragging?.Invoke(false);   //game over so no dragging...

                if (GameManager.Instance.levelNumber != 6)
                {
                    GameManager.Instance.LoadSelection();
                    return;
                }
                return;
            }

            ScaleCardsDownToUp?.Invoke(() =>
            {
                // Debug.Log("set cards now::");
                int cardCount = 5;

                int startIndex = chunkIndex * cardCount;

                if (startIndex < flashCardDatas.Count)
                    SetQuizCardsData(startIndex);
                else
                    SetLevelCardsData();

            });


            void SetLevelCardsData()
            {
                //gonna set cards data here...
                GameManager.Instance.currentLevel = LevelType.LevelQuiz;
                SetActiveWordBasket(true);
                SetActiveProgressBar(true);

                var pm = progressBar.GetComponent<ProgressManager>();
                pm.EnableStars(LevelType.LevelQuiz);

                ChangeTitle?.Invoke("Which Word is a " + GameManager.Instance.grammerType.ToString() + "?", 1200);
                SetNextBtnState?.Invoke(false);

                var chunk = GetLevelTestCards(4);

                //assign data to the falsh cards...
                for (int i = 0; i < 5; i++)
                {
                    var data = chunk[i];
                    OnNextButtonClicked?.Invoke(data.word, data.image, data.grammerType, i);
                }

                chunkIndex++;
            }

            void SetQuizCardsData(int startIndex)
            {
                var chunk = flashCardDatas.GetRange(startIndex, Mathf.Min(5, flashCardDatas.Count - startIndex));

                Debug.Log("chunk data is :: " + chunk[0].word);

                //assign data to the falsh cards...
                for (int i = 0; i < 5; i++)
                {
                    var data = chunk[i];
                    OnNextButtonClicked?.Invoke(data.word, data.image, data.grammerType, i);

                    //SetFlashCardData?.Invoke(data.word, data.image, data.grammerType, i);
                }

                chunkIndex++;
            }

        }



        [SerializeField] private List<FlashCardData> previousChunk;
        private List<FlashCardData> GetLevelTestCards(int incorrectSize)
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

            var b = filteredIncorrectWords.OrderBy(x => random.Next()).Take(incorrectSize).ToList();
            chunk.AddRange(b);

            chunk = chunk.OrderBy(x => random.Next()).ToList();

            previousChunk = chunk;

            return chunk;
        }




        private void ResetFlashCardsIndex()
        {
            chunkIndex = 1;
            for (int i = 0; i < flashCards.Count; i++)
            {
                flashCards[i].Index = i;
            }
        }


        private void ShowNext()
        {
            if (GameManager.Instance.currentLevel == LevelType.LevelQuiz && GameManager.Instance.QuizGamesPlayed > 1) return;

            GameManager.Instance.QuizGamesPlayed += 1;
            Debug.Log("<color=green>now we need to show next cards...,</color>");
            if (GameManager.Instance.QuizGamesPlayed > 5)
            {
                Debug.Log("Game Over ! ");
                OnGameOver?.Invoke();  //resets the quiz games played to zero...
                return;
            }
            if (GameManager.Instance.cardType == CardType.FlashCard)
                NextBtnLoop();
            else
                NextBtnWordCards();
        }




    }

}