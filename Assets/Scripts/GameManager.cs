using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;

public class GameManager : MonoBehaviour
{
    #region GameData
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

    public enum Mood { HAPPY, NEUTRAL, SAD }
    public Mood currentMood;

    public GameData gameData;
    #endregion

    public Image expBar;
    public Image currentEmotion;
    public Sprite sadEmote;
    public Sprite neutralEmote;
    public Sprite happyEmote;
    public Text streakString;
    public InputField nameInput;

    private NFC nfcRef;

    public Text stepsText;
    public Text dateText;

    private bool canPlaySound;
    public AudioClip successAlert;
    public AudioClip failAlert;
    public AudioClip toneD;
    public AudioClip toneA;
    public AudioClip toneCsh;
    private AudioSource asRef;

    void Awake()
    {
        dateText.text = System.DateTime.Now.ToShortDateString();

        nfcRef = GetComponent<NFC>();
        asRef = GetComponent<AudioSource>();


        LoadGame();

        if (hasSaveData == false)
        {
            petName = "Name me!";
            petSize = 0;

            stepsToday = 0;
            stepsTotal = 0;
            streak = 0;
            lastPlayed = " ";

            currentLevel = 0;
            currentExp = 0;
            totalExp = 0;

            currentMood = Mood.NEUTRAL;

            hasSaveData = true;
            SaveGame();
        } else
        {
            nameInput.text = petName;
        }

        if (lastPlayed != System.DateTime.Now.ToShortDateString())
        {
            streak = 0;
            stepsToday = 0;
            SaveGame();
        }

                streakString.text = streak.ToString();
    }

    void Update()
    {
        SetEmotion();

        SetExp();

        stepsText.text = stepsToday.ToString();
        streakString.text = streak.ToString();

    }

    public void SaveGame()
    {
        SaveProgress.SaveData(this);
    }

    public void LoadGame()
    {
        GameData data = SaveProgress.LoadData();

        hasSaveData = data.hasSaveData;

        petName = data.petName;
        petSize = data.petSize;
        streak = data.streak;
        lastPlayed = data.lastPlayed;

        stepsToday = data.stepsToday;
        stepsTotal = data.stepsTotal;

        currentLevel = data.currentLevel;
        currentExp = data.currentExp;
        totalExp = data.totalExp;
    }

    public void ResetGame()
    {
        GameData data = SaveProgress.LoadData();

        hasSaveData = false;
        data.hasSaveData = false;

        petName = "";
        data.petName = "";
        petSize = 0;
        data.petSize = 0;
        streak = 0;
        data.streak = 0;
        lastPlayed = null;
        data.lastPlayed = null;

        stepsToday = 0;
        data.stepsToday = 0;
        stepsTotal = 0;
        data.stepsTotal = 0;

        currentLevel = 0;
        data.currentLevel = 0;
        currentExp = 0;
        data.currentExp = 0;
        totalExp = 0;
        data.totalExp = 0;

        SaveProgress.SaveData(this);

        asRef.PlayOneShot(failAlert);

    }

    public void IncrementExp()
    {
        currentExp += 1;

    }

    public void OpenHelpPage()
    {
        Application.OpenURL("https://github.com/nestrd/Companion-PLUS");
    }

    public void OpenMaps()
    {
        Application.OpenURL("http://maps.google.com/maps");
    }

    public void OpenMainSite()
    {
        Application.OpenURL("https://nestrd.github.io/");
    }

    public void SetNewSteps()
    {
        if (lastPlayed != System.DateTime.Now.ToShortDateString()) {
            int.TryParse(nfcRef.textAsString, out stepsToday);
            stepsTotal += stepsToday;
            asRef.PlayOneShot(successAlert);
        }

        SaveGame();
    }

    private void SetEmotion()
    {
        switch (currentMood)
        {
            case Mood.HAPPY:
                currentEmotion.sprite = happyEmote;
                break;
            case Mood.NEUTRAL:
                currentEmotion.sprite = neutralEmote;
                break;
            case Mood.SAD:
                currentEmotion.sprite = sadEmote;
                break;
        }

        // Switch emote sprites based on threshold values
        if (currentExp <= 25)
        {
            currentMood = Mood.SAD;
        }
        else
        {
            currentMood = Mood.NEUTRAL;
        }
        if (currentExp >= 75)
        {
            currentMood = Mood.HAPPY;
        }
    }

    private void SetExp()
    {
        expBar.fillAmount = currentExp / 100;

        if (currentExp >= 10000)
        {
            //asRef.PlayOneShot(successAlert);
        }

        if (currentExp < stepsTotal / 100)
        {
            currentExp += 0.1F;
        }
        else
        {
            currentExp = stepsTotal / 100;
        }
    }

    public void SetStreak()
    {
        if(lastPlayed != System.DateTime.Now.ToShortDateString())
        {
            if (lastPlayed == System.DateTime.Now.AddDays(-1).ToShortDateString())
            {
                ++streak;
                lastPlayed = System.DateTime.Now.ToShortDateString();
                streakString.text = streak.ToString();
                SaveGame();
            }
            else 
            {
                streak = 0;
                lastPlayed = System.DateTime.Now.ToShortDateString();
                streakString.text = streak.ToString();
                SaveGame();
            }

        }
    }

    public void SetName()
    {
        petName = nameInput.text;

        SaveGame();
    }

    public void SoundToneD()
    {
        asRef.PlayOneShot(toneD);
    }

    public void SoundToneA()
    {
        asRef.PlayOneShot(toneA);
    }

    public void SoundToneCsh()
    {
        asRef.PlayOneShot(toneCsh);
    }

    public void SoundToneFail()
    {
        asRef.PlayOneShot(failAlert);
    }
}
