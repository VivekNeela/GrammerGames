using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMKOC.Grammer;
using UnityEngine;
using UnityEngine.UI;

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
        // public List<FlashCardData> Quiz_FlashCards;

        [Header("Nested list given below is being used to set data for the game ")]
        [Space(20)]
        public List<FlashCardListWrapper> flashCardNestedList;


        [Header("Random list of flashcards from all level flashcards")]
        public List<FlashCardData> correctFlashCardList;
        public List<FlashCardData> incorrectWords;
        public List<FlashCardData> RandomQuizCards;



        // [ContextMenu("SetNestedList")]
        // [Title("this Button sets the Nested List")]
        [Button]
        private void AssignNestedList()
        {
            flashCardNestedList.Clear();

            FlashCardListWrapper level_1 = new FlashCardListWrapper();
            level_1.listOfFlashCards = Level_1_FlashCards;
            flashCardNestedList.Add(level_1);

            FlashCardListWrapper level_2 = new FlashCardListWrapper();
            level_2.listOfFlashCards = Level_2_FlashCards;
            flashCardNestedList.Add(level_2);

            FlashCardListWrapper level_3 = new FlashCardListWrapper();
            level_3.listOfFlashCards = Level_3_FlashCards;
            flashCardNestedList.Add(level_3);

            FlashCardListWrapper level_4 = new FlashCardListWrapper();
            level_4.listOfFlashCards = Level_4_FlashCards;
            flashCardNestedList.Add(level_4);

            FlashCardListWrapper level_5 = new FlashCardListWrapper();
            level_5.listOfFlashCards = Level_5_FlashCards;
            flashCardNestedList.Add(level_5);

            // FlashCardListWrapper quizCards = new FlashCardListWrapper();
            // quizCards.listOfFlashCards = Quiz_FlashCards;
            // flashCardNestedList.Add(quizCards);

        }


        // [Title("sets random 5 flash cards from all levels flash cards")]
        [Button]
        private void GetCorrectFlashCards(int num)
        {
            correctFlashCardList.Clear();

            System.Random random = new System.Random();

            var nestedList = new List<List<FlashCardData>>
            {
                Level_1_FlashCards,
                Level_2_FlashCards,
                Level_3_FlashCards,
                Level_4_FlashCards,
                Level_5_FlashCards
            };

            var index = 0;
            while (index < num)
            {
                var randomFlashCard = GetRandomCard();
                while (correctFlashCardList.Contains(randomFlashCard))
                {
                    randomFlashCard = GetRandomCard();
                }

                correctFlashCardList.Add(randomFlashCard);

                index++;
            }


            FlashCardData GetRandomCard()
            {

                var randomListIndex = random.Next(nestedList.Count);
                var itemList = nestedList[randomListIndex];

                var randomFlashCardIndex = random.Next(itemList.Count);
                var randomFlashCard = itemList[randomFlashCardIndex];

                return randomFlashCard;

            }


        }

        // [Button]
        private void SetGrammer()   //just for setting all falsh cards grammer type...
        {
            SetGrammerType(Level_1_FlashCards);
            SetGrammerType(Level_2_FlashCards);
            SetGrammerType(Level_3_FlashCards);
            SetGrammerType(Level_4_FlashCards);
            SetGrammerType(Level_5_FlashCards);

            // incorrectWords = Quiz_FlashCards;
        }

        private void SetGrammerType(List<FlashCardData> list)
        {
            foreach (var item in list)
            {
                item.grammerType = GrammerType.Noun;
            }
        }


        // [Button]
        private void GetRandomFlashCards()
        {
            RandomQuizCards.Clear();

            System.Random random = new System.Random();

            int index1 = 0;
            int index2 = 0;

            while (index2 < incorrectWords.Count)
            {
                var tempList = new List<FlashCardData>();

                for (int i = 0; i < 2 && index2 < incorrectWords.Count; i++)
                {
                    tempList.Add(incorrectWords[index2]);
                    index2++;
                }

                if (index1 < correctFlashCardList.Count)
                {
                    tempList.Insert(random.Next(tempList.Count + 1), correctFlashCardList[index1]);
                    index1++;
                }

                if (index1 == correctFlashCardList.Count)
                {
                    index1 = 0;
                }

                RandomQuizCards.AddRange(tempList);
            }

        }


        [Button]
        private void GetRandomWordCards()
        {

            RandomQuizCards.Clear();

            System.Random random = new System.Random();

            int list1index = 0;
            int list2index = 0;

            for (int i = 0; i < 25; i += 5)
            {
                // int correctWords = random.Next(2, 4);
                int correctWords = 1;

                var currentChunk = new List<FlashCardData>();

                for (int j = 0; j < correctWords && list1index < correctFlashCardList.Count; j++)   //adds correct words to chunk
                {
                    currentChunk.Add(correctFlashCardList[list1index]);
                    list1index++;
                }

                // // Add one correct word to the chunk if available
                // if (list1index < correctFlashCardList.Count)
                // {
                //     currentChunk.Add(correctFlashCardList[list1index]);
                //     list1index++;
                // }


                for (int j = 0; j < (5 - correctWords) && list2index < incorrectWords.Count; j++)   //adds incorrect words to chunk
                {
                    currentChunk.Add(incorrectWords[list2index]);
                    list2index++;
                }

                // shuffles the chunk of 5 elements...
                currentChunk = currentChunk.OrderBy(x => random.Next()).ToList();

                RandomQuizCards.AddRange(currentChunk);

            }


            // for (int i = 0; i < 30; i += 5)   //adds only one correct word to a chunk of 5 
            // {
            //     var currentChunk = new List<FlashCardData>();

            //     // Add one correct word to the chunk if available
            //     if (list1index < correctFlashCardList.Count)
            //     {
            //         currentChunk.Add(correctFlashCardList[list1index]);
            //         list1index++;
            //     }

            //     // Add four incorrect words to the chunk if available
            //     for (int j = 0; j < 5 && list2index < incorrectWords.Count; j++)
            //     {
            //         currentChunk.Add(incorrectWords[list2index]);
            //         list2index++;
            //     }

            //     // Shuffle the chunk of 5 elements
            //     currentChunk = currentChunk.OrderBy(x => random.Next()).ToList();

            //     RandomQuizCards.AddRange(currentChunk);
            // }

        }




        [Title("this button set random quiz cards data to quiz cards list")]
        [Button(30)]
        private void SetQuizCards()
        {
            // Quiz_FlashCards = RandomQuizCards;
            flashCardNestedList[5].listOfFlashCards = RandomQuizCards;
        }




        public void SetNestedListQuizFlashCards()   //this is for nouns...(Flash cards)
        {
            GetCorrectFlashCards(5);
            GetRandomFlashCards();
            flashCardNestedList[5].listOfFlashCards = RandomQuizCards;
        }

        public void SetNestedListQuizWordCards()   //this is for adjectives...(Word cards)
        {
            System.Random random = new System.Random();
            random.Next(10, 16);
            GetCorrectFlashCards(10);
            GetRandomWordCards();
            flashCardNestedList[5].listOfFlashCards = RandomQuizCards;
        }

        [Button]
        private void ClearList() => RandomQuizCards.Clear();




    }


}
