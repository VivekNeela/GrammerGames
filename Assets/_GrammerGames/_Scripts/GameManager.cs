using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public GrammerType grammerType;
        public GrammerTypeDataSO grammerTypeDataSO;
        public FlashCardListWrapper currentFlashCardData;


        public static event Action<LevelType> OnLoadFlashCards;
        public static event Action<LevelType> OnLoadQuiz;
        public static event Action<LevelType> OnLoadSelection;
        public static event Action<bool> SetDraggingState;
        public static event Action<FlashCardListWrapper> SetFlashCardData;
        public static event Action<FlashCardListWrapper> SetQuizCardsData;
        public static event Action ResetFlashCardsIndex;

        // public static event Action<bool> SetProgressBarState;


        private void Start()
        {
            LoadSelection();
        }

        public void LoadSelection()   //this is on back btn
        {
            ResetFlashCardsIndex?.Invoke();
            currentLevel = LevelType.Selection;
            OnLoadSelection?.Invoke(currentLevel);
        }

        public void LoadFlashCardsLevel(int level)   //this will be on the levels btn...
        {
            currentLevel = LevelType.FlashCards;
            OnLoadFlashCards?.Invoke(currentLevel);
            SetDraggingState?.Invoke(false);
            currentFlashCardData = grammerTypeDataSO.flashCardListList[level];

            SetFlashCardData?.Invoke(currentFlashCardData);
        }

        public void LoadQuiz()   //this is on the test quiz btn...
        {
            currentLevel = LevelType.Quiz;
            OnLoadQuiz?.Invoke(currentLevel);
            SetDraggingState?.Invoke(true);
            currentFlashCardData = grammerTypeDataSO.flashCardListList[5];

            SetQuizCardsData?.Invoke(currentFlashCardData);

        }


    }



    public enum LevelType
    {
        Selection,
        FlashCards,
        Quiz,
    }
}