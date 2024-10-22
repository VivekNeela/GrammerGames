using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


namespace TMKOC.Grammer
{
    public class Collectable : MonoBehaviour
    {
        #region Public Fields
        [SerializeField] private int index;
        public int Index { get => index; set => index = value; }
        public GrammerType grammerType;
        public CardType cardType;

        #endregion

        #region Privates and Properties

        private BoxCollider2D boxCollider;
        public BoxCollider2D BoxCollider { get => boxCollider; set => boxCollider = value; }

        private SortingGroup sortingGroup;
        public SortingGroup SortingGroup { get => sortingGroup; set => sortingGroup = value; }

        private Canvas textCanvas;
        public Canvas TextCanvas { get => textCanvas; set => textCanvas = value; }

        [SerializeField] private LeanDragTranslate leanDragTranslate;
        [SerializeField] private SpriteRenderer cardSpriteRenderer;   //image
        [SerializeField] private SpriteRenderer imageSpriteRenderer;   //image
        [SerializeField] private TextMeshProUGUI tmpText;   //word
        private Vector2 initialPos;
        public Vector2 InitialPos { get => initialPos; set => initialPos = value; }

        #endregion

        public static event Action<int> OnSelected;
        public static event Action<int> OnDeselected;
        public static event Action ShowNextCards;


        private void OnEnable()
        {
            GameManager.SetDraggingState += SetLeanDragState;
            FlashCardHandler.SetFlashCardData += SetFlashCardData;
            ProgressManager.ResetCollectablePos += ResetCollectablePosition;

            ProgressBarNew.ResetCollectablePos += ResetCollectablePosition;   //new progress bar handler

            LivesManager.ResetCollectablePos += ResetCollectablePosition;
            FlashCardHandler.EnableCardsDragging += SetLeanDragState;

            FlashCardHandler.ResetCollectablePos += ResetCollectablePosition;

            // GameManager.PlayCardTransition += PlayCardTransition;

            GameManager.ResetCardScale += ResetCardScale;

            FlashCardHandler.OnNextButtonClicked += OnNextButtonClicked;
        }


        private void OnDisable()
        {
            GameManager.SetDraggingState -= SetLeanDragState;
            FlashCardHandler.SetFlashCardData -= SetFlashCardData;
            ProgressManager.ResetCollectablePos -= ResetCollectablePosition;
            
            ProgressBarNew.ResetCollectablePos -= ResetCollectablePosition;   //new progress bar handler

            LivesManager.ResetCollectablePos -= ResetCollectablePosition;
            FlashCardHandler.EnableCardsDragging -= SetLeanDragState;

            FlashCardHandler.ResetCollectablePos -= ResetCollectablePosition;

            // GameManager.PlayCardTransition -= PlayCardTransition;

            GameManager.ResetCardScale -= ResetCardScale;

            FlashCardHandler.OnNextButtonClicked -= OnNextButtonClicked;
        }

        private void Awake()
        {
            BoxCollider = GetComponent<BoxCollider2D>();
            sortingGroup = GetComponent<SortingGroup>();
            textCanvas = GetComponentInChildren<Canvas>();
        }

        private void Start()
        {
            InitialPos = transform.position;
        }

        public void ResetCollectablePosition(bool showNext, int _index, bool destroy)   //this is also on the on deselected event on the lean selectable by finger...
        {
            SetLeanDragState(false);
            transform.DOMove(InitialPos, .5f).OnComplete(() =>
            {
                OnReset(showNext, _index, destroy);
            });
        }

        public void Selected()  //this function is on the On Selected event of Lean Selectable by finger.
        {
            OnSelected?.Invoke(Index);
        }

        public void Deselected()  //this function is on the On Deselected event of Lean Selectable by finger. 
        {
            OnDeselected?.Invoke(Index);
            ResetCollectablePosition(false, index, false);
        }

        private void SetLeanDragState(bool state) => leanDragTranslate.enabled = state;

        private void SetFlashCardData(string word, Sprite image, GrammerType grammer, int _index)
        {
            if (_index == Index)
            {
                tmpText.text = word;
                imageSpriteRenderer.sprite = image;
                grammerType = grammer;
            }
            else
                return;

        }


        private void OnNextButtonClicked(string word, Sprite image, GrammerType grammer, int _index)
        {
            // if (_index != index) return;
            // Vector3 defaultScale = transform.localScale;
            SetFlashCardData(word, image, grammer, _index);

            //lets not do the transition here...

            // transform.DOScale(0, 1).OnComplete(() =>
            // {
            //     transform.DOScale(defaultScale.x, 1);
            // });
        }


        private void OnReset(bool showNext, int _index, bool destroy)
        {
            // if (gameObject.activeSelf && showNext)
            //     PlayCardTransition(0, 0.8f);
            cardSpriteRenderer.color = Color.white;
            // Debug.Log("<color=yellow>card pos was reset...</color>");
            if (GameManager.Instance.currentLevel == LevelType.Quiz)
                SetLeanDragState(true);
            else
                SetLeanDragState(false);

            //OG CODE
            // if show next true invoke an event that tells flashcard handler to show next cards...
            if (_index == index && showNext && destroy)
            {
                Debug.Log("<color=yellow> Destroy card and show Next!!! </color>");
                ShowNextCards?.Invoke();

            }
            if (_index == index && destroy)
            {
                Debug.Log("<color=yellow> Destroy card!!! </color>");
                cardSpriteRenderer.color = Color.green;
            }

        }

        //not using

        // private void PlayCardTransition(float from, float to) => StartCoroutine(CardScaleTransition(from, to));

        // private IEnumerator CardScaleTransition(float from, float to)
        // {
        //     transform.DOScale(from, .1f);
        //     yield return new WaitForSeconds(.5f);
        //     transform.DOScale(to, .5f);
        // }

        private void ResetCardScale() => transform.localScale = Vector3.zero;



    }

    public enum CardType
    {
        FlashCard,
        WordCard,
    }

}