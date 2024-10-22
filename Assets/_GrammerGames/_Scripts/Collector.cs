using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.Grammer
{

    public class Collector : MonoBehaviour
    {
        public GrammerType collectorGrammerType;
        public static event Action<int> OnRightAnswer;
        public static event Action<int> OnWrongAnswer;
        public static event Action<float, int> AddPoints;


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
                Debug.Log("<color=red> Wrong answer !!! </color>");

                OnWrongAnswer?.Invoke(collectable.Index);
            }
            else
            {
                Debug.Log("<color=yellow> Right answer !!! </color>");

                OnRightAnswer?.Invoke(collectable.Index);
                // if (collectable.cardType == CardType.FlashCard)
                //     OnRightAnswer?.Invoke(collectable.Index);
                // else
                //     AddPoints?.Invoke(.5f, collectable.Index);


            }
        }



    }

}