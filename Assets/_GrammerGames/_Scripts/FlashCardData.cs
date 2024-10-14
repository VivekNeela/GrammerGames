using System;
using UnityEngine;

namespace TMKOC.Grammer
{
    [Serializable]
    public class FlashCardData   //this class will store the data of the flash cards
    {
        public string word;
        public Sprite image;
        public GrammerType grammerType;
    }
}