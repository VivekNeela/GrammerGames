using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TMKOC.Grammer
{


    public class LevelManager : MonoBehaviour
    {
        // public GrammerType currentGamestate;
        public List<Canvas> gameCanvasList;
        public static event Action OnLevelChanged;

        void OnEnable()
        {
            GameManager.OnLoadLevel += OnLoadLevel;
        }

        private void OnLoadLevel(int arg0, LevelType arg1)
        {
            //Load level
            // GameManager.Instance.LoadLevel();
        }

        public void SetLevel()
        {
            OnLevelChanged.Invoke();
        }



    }




    
}