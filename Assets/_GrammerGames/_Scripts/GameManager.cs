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
        public LevelType currentLevel;
        public List<LeanSelectableByFinger> wordCards;
        public List<GameObject> gameCanvasList;


        private void OnEnable()
        {
            LevelManager.OnLevelChanged += SetLevel;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelChanged -= SetLevel;
        }


        public void SetLevel()
        {
            SetCanvas(currentLevel);
        }


        public void SetCanvas(LevelType level)
        {
            switch (level)
            {
                case LevelType.Selection:
                    SetActiveCanvas(0);
                    break;

                case LevelType.FlashCards:
                    SetActiveCanvas(1);
                    break;

                case LevelType.Quiz:
                    SetActiveCanvas(2);
                    break;

                default:
                    break;
            }
        }


        public void SetActiveCanvas(int level)
        {
            foreach (var item in gameCanvasList)
            {
                item.SetActive(false);
            }
            gameCanvasList[level].SetActive(true);
        }

        void OnNExtBUtton()
        {
            // GameManager.Instance.LoadLevel(1);
        }


        private void Update()
        {
            // SetLevel();
        }

        public static UnityAction<int,LevelType> OnLoadLevel;

        //Called from UI BUtton
        public  void LoadLevel(int levelNo, LevelType levelType)
        {
            OnLoadLevel?.Invoke(levelNo, levelType);
        }

        


    }

    public enum LevelType
    {
        Selection,
        FlashCards,
        Quiz,
    }
}