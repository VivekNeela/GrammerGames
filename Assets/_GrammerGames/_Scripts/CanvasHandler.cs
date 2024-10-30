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
        // public GameObject wordCardHandler;
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
                    SetActiveFlashCards(false);

                    EnableWordBasket(false);
                    // wordBasket.SetActive(false);

                    levelConfetti.SetActive(false);
                    break;

                case LevelType.FlashCards:
                    SetActiveCanvas(1);
                    // SetActiveFlashCards(true);
                    SetActiveFlashCards(true);
                    SetTitleTextAndWidth(GameManager.Instance.grammerType.ToString() + "s", 800);

                    //3 stars progressbar...
                    EnableProgressBar(LevelType.LevelQuiz, false);
                    // progressBar.SetActive(false);

                    levelConfetti.SetActive(false);
                    nextBtn.SetActive(true);

                    // if (GameManager.Instance.cardType != CardType.WordCard)
                    //     nextBtn.SetActive(true);
                    // else
                    //     nextBtn.SetActive(false);

                    EnableWordBasket(false);
                    // wordBasket.SetActive(false);

                    break;

                case LevelType.LevelQuiz:
                    EnableWordBasket(false);
                    break;

                case LevelType.Quiz:
                    SetActiveCanvas(1);
                    // SetActiveFlashCards(true);
                    SetActiveFlashCards(true);
                    SetTitleTextAndWidth("Choose the correct " + GameManager.Instance.grammerType.ToString(), 1200);

                    //5 stars progressbar...
                    EnableProgressBar(LevelType.Quiz, true);
                    // progressBar.SetActive(true);

                    nextBtn.SetActive(false);

                    EnableWordBasket(true);
                    // wordBasket.SetActive(true);

                    break;

                case LevelType.GameOver:
                    SetActiveCanvas(2);

                    EnableWordBasket(false);
                    // wordBasket.SetActive(false);

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

        private void SetActiveFlashCards(bool state)
        {
            flashCardHandler.SetActive(state);
        }


        private void SetTitleTextAndWidth(string titleText, int width)
        {
            if (titleBox.GetComponentInChildren<TextMeshProUGUI>().text == titleText) return;   //if text is already the same return...

            titleBox.transform.DOScale(0, .5f).OnComplete(() =>
            {
                titleBox.GetComponentInChildren<TextMeshProUGUI>().text = titleText;
                var size = new Vector2(width, titleBox.sizeDelta.y);
                titleBox.DOSizeDelta(size, 0);
                //scale up again... 
                titleBox.transform.DOScale(1, .5f);
            });
        }


        private void SetActiveNextbtn(bool state) => nextBtn.SetActive(state);

        private void EnableProgressBar(LevelType levelType, bool state)
        {
            var pm = progressBar.GetComponent<ProgressManager>();
            pm.EnableStars(levelType);
            progressBar.SetActive(state);

        }


        private void EnableWordBasket(bool state)
        {
            wordBasket.SetActive(state);
            // wordBasket.transform.DOScale(0, .5f).OnComplete(() =>
            // {
            //     wordBasket.transform.DOScale(1, .5f);
            // });   
        }


    }
}
