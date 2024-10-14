using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMKOC.Grammer;
using UnityEngine;

namespace TMKOC.Grammer
{


    [CreateAssetMenu(fileName = "GrammerTypeData", menuName = "ScriptableObjects/GrammerTypeData", order = 1)]
    public class GrammerTypeDataSO : ScriptableObject
    {
        // public GrammerType grammerType;

        public List<FlashCardData> Level_1_FlashCards;
        public List<FlashCardData> Level_2_FlashCards;
        public List<FlashCardData> Level_3_FlashCards;
        public List<FlashCardData> Level_4_FlashCards;
        public List<FlashCardData> Level_5_FlashCards;
        public List<FlashCardData> Quiz_FlashCards;

        [Header("Nested list given below is being used to set data for the game ")]
        [Space(20)]
        public List<FlashCardListWrapper> flashCardListList;




        [ContextMenu("SetNestedList")]
        private void AssignNestedList()
        {
            flashCardListList.Clear();

            FlashCardListWrapper level_1 = new FlashCardListWrapper();
            level_1.listOfFlashCards = Level_1_FlashCards;
            flashCardListList.Add(level_1);

            FlashCardListWrapper level_2 = new FlashCardListWrapper();
            level_2.listOfFlashCards = Level_2_FlashCards;
            flashCardListList.Add(level_2);

            FlashCardListWrapper level_3 = new FlashCardListWrapper();
            level_3.listOfFlashCards = Level_3_FlashCards;
            flashCardListList.Add(level_3);

            FlashCardListWrapper level_4 = new FlashCardListWrapper();
            level_4.listOfFlashCards = Level_4_FlashCards;
            flashCardListList.Add(level_4);

            FlashCardListWrapper level_5 = new FlashCardListWrapper();
            level_5.listOfFlashCards = Level_5_FlashCards;
            flashCardListList.Add(level_5);

            FlashCardListWrapper quizCards = new FlashCardListWrapper();
            quizCards.listOfFlashCards = Quiz_FlashCards;
            flashCardListList.Add(quizCards);


        }


        // [ContextMenu("SetGrammerTypeForAll")]
        // private void SetGrammerTypeAndWordForAll()
        // {
        //     listOfList.Add(Level_1_FlashCards);
        //     listOfList.Add(Level_2_FlashCards);
        //     listOfList.Add(Level_3_FlashCards);
        //     listOfList.Add(Level_4_FlashCards);
        //     listOfList.Add(Level_5_FlashCards);



        //     foreach (var list in listOfList)
        //     {
        //         foreach (var item in list)
        //         {
        //             item.grammerType = grammerType;
        //             item.word = item.image.name;
        //         }
        //     }
        // }

    }


}
