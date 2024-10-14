using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


namespace TMKOC.Grammer
{
    public class Collectable : MonoBehaviour
    {
        #region Public Fields
        [SerializeField] private int index;
        public int Index { get => index; set => index = value; }
        public GrammerType grammerType;
        // public string word;
        // public Sprite image;
        #endregion

        #region Privates and Properties

        private BoxCollider2D boxCollider;
        public BoxCollider2D BoxCollider { get => boxCollider; set => boxCollider = value; }

        private SortingGroup sortingGroup;
        public SortingGroup SortingGroup { get => sortingGroup; set => sortingGroup = value; }

        private Canvas textCanvas;
        public Canvas TextCanvas { get => textCanvas; set => textCanvas = value; }

        [SerializeField] private LeanDragTranslate leanDragTranslate;
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
            LivesManager.ResetCollectablePos += ResetCollectablePosition;
        }


        private void OnDisable()
        {
            GameManager.SetDraggingState -= SetLeanDragState;
            FlashCardHandler.SetFlashCardData -= SetFlashCardData;
            ProgressManager.ResetCollectablePos -= ResetCollectablePosition;
            LivesManager.ResetCollectablePos -= ResetCollectablePosition;
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

        public void ResetCollectablePosition(bool showNext, int _index)   //this is also on the on deselected event on the lean selectable by finger...
        {
            SetLeanDragState(false);
            transform.DOMove(InitialPos, .5f).OnComplete(() => { OnReset(showNext, _index); });
        }

        public void Selected()  //this function is on the On Selected event of Lean Selectable by finger.
        {
            OnSelected?.Invoke(Index);
        }

        public void Deselected()  //this function is on the On Deselected event of Lean Selectable by finger. 
        {
            OnDeselected?.Invoke(Index);
            ResetCollectablePosition(false, index);
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

        private void OnReset(bool showNext, int _index)
        {
            // Debug.Log("<color=yellow>card pos was reset...</color>");
            SetLeanDragState(true);
            //if show next true invoke an event that tells flashcard handler to show next cards...
            if (_index == index && showNext)
            {
                // Debug.Log("<color=green>now we need to show next cards...,</color>");
                ShowNextCards?.Invoke();
            }
        }

    }

}