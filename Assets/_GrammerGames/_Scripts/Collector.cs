using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.Grammer
{

    public class Collector : MonoBehaviour
    {
        public GrammerType collectorGrammerType;
        public static event Action OnRightAnswer;
        public static event Action OnWrongAnswer;

        private void Start()
        {
            collectorGrammerType = GameManager.Instance.grammerType;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Card :: " + other.gameObject.name);
            var collectable = other.gameObject.GetComponent<Collectable>();
            if (collectable.grammerType != collectorGrammerType)
            {
                Debug.Log("<color=red>Wrong answer !!! </color>");
                OnWrongAnswer?.Invoke();
            }
            else
            {
                Debug.Log("<color=yellow>Right answer !!! </color>");
                OnRightAnswer?.Invoke();

            }
        }


    }

}