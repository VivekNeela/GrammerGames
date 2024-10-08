using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TMKOC.Grammer
{

    public class CavasHandler : MonoBehaviour
    {
        void OnEnable()
        {
            GameManager.OnLoadLevel += OnLoadLevel;
        }

        private void OnLoadLevel(int arg0, LevelType arg1)
        {
            //Based on level type toggle canvas
        }
    }
    public class LevelManager : MonoBehaviour
    {
        public GrammerState currentGamestate;
        public List<Canvas> gameCanvasList;
        public static event Action OnLevelChanged;

        void OnEnable()
        {
            GameManager.OnLoadLevel += OnLoadLevel;
        }

        private void OnLoadLevel(int arg0, LevelType arg1)
        {
            //Load level
        }

        public void SetLevel()
        {
            OnLevelChanged.Invoke();
        }





    }




    public enum GrammerState
    {
        Adjectives,
        Articles,
        Conjunctions,
        Nouns,
        Prepostions,
        Pronouns,
        Verbs,

    }
}