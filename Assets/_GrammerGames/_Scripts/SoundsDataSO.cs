using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;



[CreateAssetMenu(fileName = "SoundsData", menuName = "ScriptableObjects/SoundsData", order = 2)]
public class SoundsDataSO : SerializedScriptableObject
{


    public AudioClip nounsIntroduction;
    public AudioClip categorySelection;

    // public List<LevelSounds> levels;
    // [OdinSerialize]
    // private Dictionary<int, AudioClip> audioClips;


}

public class LevelSounds
{
    [OdinSerialize]
    private Dictionary<int, AudioClip> audioClips;
}



