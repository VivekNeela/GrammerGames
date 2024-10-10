using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;


namespace TMKOC.Grammer
{
    public class Collectable : MonoBehaviour
    {
        public int index;
        private BoxCollider2D boxCollider;
        public BoxCollider2D BoxCollider { get => boxCollider; set => boxCollider = value; }
        private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
        public GrammerType grammerType;
        private LeanSelectableByFinger leanSelectableByFinger;
        public bool IsSelected
        {
            get { return leanSelectableByFinger.IsSelected; }
        }


        public static event Action<int> OnSelected;
        private Vector2 initialPos;


        private void Awake()
        {
            initialPos = transform.position;
            leanSelectableByFinger = GetComponent<LeanSelectableByFinger>();
            BoxCollider = GetComponent<BoxCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ResetCollectablePosition()
        {
            transform.DOMove(initialPos, .5f);
        }

        public void Selected()  //this function is on the lean selectable event
        {
            OnSelected?.Invoke(index);
        }




    }

    public enum GrammerType
    {
        Adjective,
        Article,
        Conjunction,
        Noun,
        Prepostion,
        Pronoun,
        Verb,

    }

}