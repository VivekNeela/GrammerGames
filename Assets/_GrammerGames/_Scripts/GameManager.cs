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
        public int levelNumber;
        public GrammerType grammerType;
        public CardType cardType;
        public GrammerTypeDataSO grammerTypeDataSO;
        // public  GrammerFlashCardsSO grammerTypeDataSO;
        public FlashCardListWrapper currentFlashCardData;
        public int QuizGamesPlayed;


        public static event Action<LevelType> OnLoadFlashCards;
        public static event Action<LevelType> OnLoadQuiz;
        public static event Action<LevelType> OnLoadSelection;
        public static event Action<LevelType> OnGameOver;
        public static event Action<bool> SetDraggingState;
        public static event Action<FlashCardListWrapper> SetFlashCardData;
        public static event Action<FlashCardListWrapper> SetQuizCardsData;
        public static event Action ResetFlashCardsIndex;
        public static event Action OnResetQuiz;
        public static event Action<float, float> PlayCardTransition;


        private void OnEnable()
        {
            FlashCardHandler.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            FlashCardHandler.OnGameOver -= GameOver;
        }


        private void Start()
        {
            LoadSelection();
        }

        public void LoadSelection()   //this is on back btn
        {
            ResetFlashCardsIndex?.Invoke();
            OnResetQuiz?.Invoke();
            currentLevel = LevelType.Selection;
            levelNumber = 0;
            
            PlayCardTransition?.Invoke(0, 0);

            OnLoadSelection?.Invoke(currentLevel);
            QuizGamesPlayed = 0;

        }

        public void LoadFlashCardsLevel(int level)   //this will be on the levels btn...
        {
            currentLevel = LevelType.FlashCards;
            levelNumber = level + 1;
            OnLoadFlashCards?.Invoke(currentLevel);
            SetDraggingState?.Invoke(false);
            currentFlashCardData = grammerTypeDataSO.flashCardNestedList[level];

            SetFlashCardData?.Invoke(currentFlashCardData);

            PlayCardTransition?.Invoke(0, .8f);

        }


        // public void LoadWordCardsLevel(int level)
        // {
        //     currentLevel = LevelType.FlashCards;
        //     OnLoadFlashCards?.Invoke(currentLevel);
        //     SetDraggingState?.Invoke(false);
        //     currentFlashCardData=grammerTypeDataSO.flashCardNestedList[level];

        //     SetWordCardData?.Invoke(currentFlashCardData);
        // }


        public void LoadQuiz()   //this is on the test quiz btn...
        {
            currentLevel = LevelType.Quiz;
            levelNumber = 6;
            OnLoadQuiz?.Invoke(currentLevel);
            SetDraggingState?.Invoke(true);

            if (cardType == CardType.FlashCard)
                grammerTypeDataSO.SetNestedListQuizFlashCards();   //this function is for flash cards
            else
                grammerTypeDataSO.SetNestedListQuizWordCards();   //this function is for word cards

            currentFlashCardData = grammerTypeDataSO.flashCardNestedList[5];  //last element is quiz cards

            SetQuizCardsData?.Invoke(currentFlashCardData);
            QuizGamesPlayed += 1;

            PlayCardTransition?.Invoke(0, .8f);
        }


        public void GameOver()
        {
            currentLevel = LevelType.GameOver;
            OnResetQuiz?.Invoke();
            OnGameOver?.Invoke(currentLevel);
            QuizGamesPlayed = 0;
        }


    }



    public enum LevelType
    {
        Selection,
        FlashCards,
        Quiz,
        GameOver,
    }
}