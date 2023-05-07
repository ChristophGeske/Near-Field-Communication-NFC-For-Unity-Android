using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

    //CONFIG
    public bool hasSaveData;

    //CONSTANT
    public string petName;
    public int petSize;

    //VARIABLE
    public int stepsToday;
    public int stepsTotal;
    public int streak;
    public string lastPlayed;

    public int currentLevel;
    public float currentExp;
    public float totalExp;


    public GameData(GameManager manager)
    {
        hasSaveData = manager.hasSaveData;

        petName = manager.petName;
        petSize = manager.petSize;

        stepsToday = manager.stepsToday;
        stepsTotal = manager.stepsTotal;
        streak = manager.streak;
        lastPlayed = manager.lastPlayed;

        currentLevel = manager.currentLevel;
        currentExp = manager.currentExp;
        totalExp = manager.totalExp;
        
    }

}
