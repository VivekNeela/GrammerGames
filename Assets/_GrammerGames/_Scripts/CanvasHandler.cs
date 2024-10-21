using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace TMKOC.Grammer
{
    public class CanvasHandler : MonoBehaviour
    {
        public List<GameObject> gameCanvasList;
        public GameObject flashCardHandler;
        public GameObject wordCardHandler;
        public RectTransform titleBox;
        public GameObject progressBar;
        public GameObject nextBtn;
        public GameObject wordBasket;
        public GameObject levelConfetti;



        void OnEnable()
        {
            GameManager.OnLoadFlashCards += SetCanvas;
            GameManager.OnLoadQuiz += SetCanvas;
            GameManager.OnLoadSelection += SetCanvas;
            GameManager.OnGameOver += SetCanvas;
            FlashCardHandler.ChangeTitle += SetTitleTextAndWidth;
            FlashCardHandler.SetNextBtnState += SetActiveNextbtn;
        }
        private void OnDisable()
        {
            GameManager.OnLoadFlashCards -= SetCanvas;
            GameManager.OnLoadQuiz -= SetCanvas;
            GameManager.OnLoadSelection -= SetCanvas;
            GameManager.OnGameOver -= SetCanvas;
            FlashCardHandler.ChangeTitle -= SetTitleTextAndWidth;
        }


        private void SetCanvas(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Selection:
                    SetActiveCanvas(0);
                    // SetActiveFlashCards(false);
                    SetActiveFlashCards(GameManager.Instance.cardType, false);
                    wordBasket.SetActive(false);
                    levelConfetti.SetActive(false);
                    break;

                case LevelType.FlashCards:
                    SetActiveCanvas(1);
                    // SetActiveFlashCards(true);
                    SetActiveFlashCards(GameManager.Instance.cardType, true);
                    SetTitleTextAndWidth(GameManager.Instance.grammerType.ToString() + "s", 800);
                    progressBar.SetActive(false);
                    levelConfetti.SetActive(false);
                    if (GameManager.Instance.cardType != CardType.WordCard)
                        nextBtn.SetActive(true);
                    else
                        nextBtn.SetActive(false);

                    wordBasket.SetActive(false);
                    break;

                case LevelType.Quiz:
                    SetActiveCanvas(1);
                    // SetActiveFlashCards(true);
                    SetActiveFlashCards(GameManager.Instance.cardType, true);
                    SetTitleTextAndWidth("Choose the correct " + GameManager.Instance.grammerType.ToString() + "s", 1200);
                    progressBar.SetActive(true);
                    nextBtn.SetActive(false);
                    wordBasket.SetActive(true);
                    break;

                case LevelType.GameOver:
                    SetActiveCanvas(2);
                    wordBasket.SetActive(false);
                    break;

                default:
                    break;
            }
        }


        public void SetActiveCanvas(int level)
        {
            foreach (var item in gameCanvasList)
            {
                item.SetActive(false);
            }
            gameCanvasList[level].SetActive(true);
        }

        // private void SetActiveFlashCards(bool state) => flashCardHandler.SetActive(state);

        private void SetActiveFlashCards(CardType cardType, bool state)
        {
            if (cardType == CardType.FlashCard)
                flashCardHandler.SetActive(state);
            else
                wordCardHandler.SetActive(state);
        }


        private void SetTitleTextAndWidth(string titleText, int width)
        {
            titleBox.GetComponentInChildren<TextMeshProUGUI>().text = titleText;
            var size = new Vector2(width, titleBox.sizeDelta.y);
            titleBox.DOSizeDelta(size, 0);
        }


        private void SetActiveNextbtn(bool state) => nextBtn.SetActive(state);



    }
}
