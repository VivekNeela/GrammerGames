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
        public RectTransform titleBox;
        public GameObject progressBar;
        // public TextMeshProUGUI gameTitle;
        // private string gameTitle=""

        public static event Action<bool> EnableWordBasket;

        void OnEnable()
        {
            GameManager.OnLoadFlashCards += SetCanvas;
            GameManager.OnLoadQuiz += SetCanvas;
            GameManager.OnLoadSelection += SetCanvas;
        }
        private void OnDisable()
        {
            GameManager.OnLoadFlashCards -= SetCanvas;
            GameManager.OnLoadQuiz -= SetCanvas;
            GameManager.OnLoadSelection -= SetCanvas;
        }


        private void SetCanvas(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Selection:
                    SetActiveCanvas(0);
                    SetActiveFlashCards(false);
                    EnableWordBasket?.Invoke(false);
                    break;

                case LevelType.FlashCards:
                    SetActiveCanvas(1);
                    SetActiveFlashCards(true);
                    EnableWordBasket?.Invoke(false);
                    SetTitleTextAndWidth(GameManager.Instance.grammerType.ToString() + "s", 800);
                    progressBar.SetActive(false);
                    break;

                case LevelType.Quiz:
                    SetActiveCanvas(1);
                    SetActiveFlashCards(true);
                    EnableWordBasket?.Invoke(true);
                    SetTitleTextAndWidth("Choose the correct " + GameManager.Instance.grammerType.ToString(), 1200);
                    progressBar.SetActive(true);
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

        private void SetActiveFlashCards(bool state) => flashCardHandler.SetActive(state);


        private void SetTitleTextAndWidth(string titleText, int width)
        {
            titleBox.GetComponentInChildren<TextMeshProUGUI>().text = titleText;
            var size = new Vector2(width, titleBox.sizeDelta.y);
            titleBox.DOSizeDelta(size, 0);
        }



    }
}
