using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using TMPro;
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

        #endregion

        public static event Action<int> OnSelected;


        private void OnEnable()
        {
            GameManager.SetDraggingState += SetLeanDragState;
            FlashCardHandler.SetFlashCardData += SetFlashCardData;
            // GameManager.ResetFlashCardsIndex += ResetIndex;
        }


        private void OnDisable()
        {
            GameManager.SetDraggingState -= SetLeanDragState;
            FlashCardHandler.SetFlashCardData -= SetFlashCardData;
            // GameManager.ResetFlashCardsIndex -= ResetIndex;
        }

        private void Awake()
        {
            BoxCollider = GetComponent<BoxCollider2D>();
            sortingGroup = GetComponent<SortingGroup>();
            textCanvas = GetComponentInChildren<Canvas>();
        }

        private void Start()
        {
            initialPos = transform.position;
        }

        public void ResetCollectablePosition()
        {
            transform.DOMove(initialPos, .5f);
        }

        public void Selected()  //this function is on the On Deselected event of Lean Selectable by finger.
        {
            OnSelected?.Invoke(Index);
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

        // private void ResetIndex()
        // {
        //     var name = gameObject.name;
        //     var s = name.Split("_");
        //     var num = int.Parse(s[1]);
        //     Index = num;
        // }


    }

}