using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Lean.Touch;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace TMKOC.Grammer
{
    public class GameManager : SerializedSingleton<GameManager>
    {

        #region playschool API
        public int GAME_ID;
        private GameCategoryDataManager gameCategoryDataManager;
        public GameCategoryDataManager GameCategoryDataManager { get => gameCategoryDataManager; }
        private UpdateCategoryApiManager updateCategoryApiManager;
        public UpdateCategoryApiManager UpdateCategoryApiManager { get => updateCategoryApiManager; }

        #endregion

        //yash is gay...


        public LevelType currentLevel;
        public int levelNumber;
        public GrammerType grammerType;
        public CardType cardType;
        public GrammerTypeDataSO grammerTypeDataSO;
        public FlashCardListWrapper currentFlashCardData;
        public int QuizGamesPlayed;


        public static event Action<LevelType> OnLoadFlashCards;
        public static event Action<LevelType> OnLoadQuiz;
        public static event Action<LevelType> OnLoadSelection;
        public static event Action<LevelType> OnGameOver;
        // public static event Action<bool> SetDraggingState;
        // public static event Action<bool> EnableCollector;
        public static event Action<FlashCardListWrapper> SetFlashCardData;
        public static event Action<FlashCardListWrapper> SetQuizCardsData;
        public static event Action ResetFlashCardsIndex;
        public static event Action OnResetQuiz;
        // public static event Action<float, float> PlayCardTransition;

        public static event Action<Action> ScaleCardsOneByOne;
        public static event Action<Action> ScaleDownCards;
        public static event Action ResetCardScale;



        private void OnEnable()
        {
            FlashCardHandler.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            FlashCardHandler.OnGameOver -= GameOver;
        }

        protected override void Awake()
        {
            base.Awake();
            gameCategoryDataManager = new GameCategoryDataManager(GAME_ID);
            updateCategoryApiManager = new UpdateCategoryApiManager(GAME_ID);
        }

        private void Start()
        {
            LoadSelection();
        }


        public void LoadSelection()   //this is on back btn
        {
            if (TransitionHandler.Instance.inTransition == true) return;
            StartCoroutine(GoSelectionScreenCoroutine());
        }


        public void LoadFlashCardsLevel(int level)   //this will be on the levels btn...
        {
            if (TransitionHandler.Instance.inTransition == true) return;
            StartCoroutine(GoLevelCoroutine(level));

            // currentLevel = LevelType.FlashCards;
            // levelNumber = level + 1;
            // OnLoadFlashCards?.Invoke(currentLevel);
            // SetDraggingState?.Invoke(false);
            // currentFlashCardData = grammerTypeDataSO.flashCardNestedList[level];

            // SetFlashCardData?.Invoke(currentFlashCardData);

            // // PlayCardTransition?.Invoke(0, .8f);
            // ScaleCardsOneByOne?.Invoke(() => { });
        }




        public void LoadQuiz()   //this is on the test quiz btn...
        {
            if (TransitionHandler.Instance.inTransition == true) return;
            StartCoroutine(GoQuizCoroutine());

            // currentLevel = LevelType.Quiz;
            // levelNumber = 6;
            // OnLoadQuiz?.Invoke(currentLevel);
            // SetDraggingState?.Invoke(true);

            // if (cardType == CardType.FlashCard)
            //     grammerTypeDataSO.SetNestedListQuizFlashCards();   //this function is for flash cards
            // else
            //     grammerTypeDataSO.SetNestedListQuizWordCards();   //this function is for word cards

            // currentFlashCardData = grammerTypeDataSO.flashCardNestedList[5];  //last element is quiz cards

            // SetQuizCardsData?.Invoke(currentFlashCardData);
            // QuizGamesPlayed += 1;

            // // PlayCardTransition?.Invoke(0, .8f);
            // ScaleCardsOneByOne?.Invoke(() => { });
        }



        private IEnumerator GoSelectionScreenCoroutine()
        {
            CloudUI.Instance.PlayCloudEnterAnimation();
            yield return new WaitUntil(() => { return !CloudUI.Instance.InTransition; });

            ResetCardScale?.Invoke();
            ResetFlashCardsIndex?.Invoke();
            OnResetQuiz?.Invoke();
            currentLevel = LevelType.Selection;
            levelNumber = 0;

            // PlayCardTransition?.Invoke(0, 0);

            OnLoadSelection?.Invoke(currentLevel);
            QuizGamesPlayed = 0;
        }


        private IEnumerator GoLevelCoroutine(int level)
        {
            CloudUI.Instance.PlayCloudEnterAnimation();
            yield return new WaitUntil(() => { return !CloudUI.Instance.InTransition; });

            currentLevel = LevelType.FlashCards;
            levelNumber = level + 1;
            OnLoadFlashCards?.Invoke(currentLevel);

            // SetDraggingState?.Invoke(false);

            currentFlashCardData = grammerTypeDataSO.flashCardNestedList[level];

            yield return new WaitForEndOfFrame();

            SetFlashCardData?.Invoke(currentFlashCardData);

            // PlayCardTransition?.Invoke(0, .8f);
            ScaleCardsOneByOne?.Invoke(() => { });
        }


        private IEnumerator GoQuizCoroutine()
        {
            CloudUI.Instance.PlayCloudEnterAnimation();
            yield return new WaitUntil(() => { return !CloudUI.Instance.InTransition; });

            currentLevel = LevelType.Quiz;
            levelNumber = 6;
            OnLoadQuiz?.Invoke(currentLevel);

            // SetDraggingState?.Invoke(true);

            // EnableCollector?.Invoke(true);

            if (cardType == CardType.FlashCard)
                grammerTypeDataSO.SetNestedListQuizFlashCards();   //this function is for flash cards
            else
                grammerTypeDataSO.SetNestedListQuizWordCards();   //this function is for word cards

            currentFlashCardData = grammerTypeDataSO.flashCardNestedList[5];  //last element is quiz cards

            SetQuizCardsData?.Invoke(currentFlashCardData);
            QuizGamesPlayed += 1;

            // PlayCardTransition?.Invoke(0, .8f);
            ScaleCardsOneByOne?.Invoke(() => { });

        }


        public void GameOver()
        {
            currentLevel = LevelType.GameOver;
            OnResetQuiz?.Invoke();
            OnGameOver?.Invoke(currentLevel);   //this event sets the game over canvas...
            QuizGamesPlayed = 0;
        }

    }



    public enum LevelType
    {
        Selection,
        FlashCards,
        LevelQuiz,
        Quiz,
        GameOver,
    }
}