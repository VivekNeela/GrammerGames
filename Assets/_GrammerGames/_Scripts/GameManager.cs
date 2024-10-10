using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace TMKOC.Grammer
{
    public class GameManager : SerializedSingleton<GameManager>
    {
        public int GAME_ID;
        public LevelType currentLevel;
        public static event Action<int, LevelType> OnLoadLevel;


    
        private void Start()
        {
            LoadSelection();
        }


        public void SetLevel()
        {
            // SetCanvas(currentLevel);
        }



        void OnNExtBUtton()
        {
            // GameManager.Instance.LoadLevel(1);
        }




        //Called from UI BUtton
        public void LoadLevel(int levelNo, LevelType levelType)
        {
            currentLevel = levelType;
            OnLoadLevel?.Invoke(levelNo, levelType);
        }

        public void LoadSelection() => LoadLevel(0, LevelType.Selection);  //these will be on the btns
        public void LoadFlashCards() => LoadLevel(1, LevelType.FlashCards);
        public void LoadQuiz() => LoadLevel(2, LevelType.Quiz);


    }






    public enum LevelType
    {
        Selection,
        FlashCards,
        Quiz,
    }
}