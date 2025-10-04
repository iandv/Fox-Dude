using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SaveData
{
    //public string playerName;
    public int gems, stars, lastReward; // exp, expCap, lvl,
    public bool notNew, gemPopUp; // nicknamed;
    public StageResult[] stageResults;
    public int[] highScores;
    public PlayerSkin playerSkin;
    public bool[] unlockedSkins;
    //Clean Data
    public StageResult[] noResults;
    public int[] zeroScore;
    public bool[] lockSkins;

    public enum PlayerSkin
    {
        knight,
        mage,
    }

    public enum StageResult
    {
        zero,
        one,
        two,
        three,
    }
}
