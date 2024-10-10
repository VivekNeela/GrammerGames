using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.Grammer
{
    public class CavasHandler : MonoBehaviour
    {
        public List<GameObject> gameCanvasList;

        void OnEnable()
        {
            GameManager.OnLoadLevel += OnLoadLevel;
        }
        private void OnDisable()
        {
            GameManager.OnLoadLevel += OnLoadLevel;
        }

        private void OnLoadLevel(int arg0, LevelType arg1)
        {
            //Based on level type toggle canvas
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


    }
}
