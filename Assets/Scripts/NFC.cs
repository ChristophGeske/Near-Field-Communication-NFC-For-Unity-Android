using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.XR;

public class NFC : MonoBehaviour
{

    public string textAsString;

    private AndroidJavaObject mActivity;
    private AndroidJavaObject mIntent;
    private string sAction;

    private GameManager gmRef;

    void Awake()
    {
        gmRef = GetComponent<GameManager>();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                // Create new NFC Android object
                mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
                mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
                sAction = mIntent.Call<String>("getAction"); // resulte are returned in the Intent object
                if (sAction == "android.nfc.action.NDEF_DISCOVERED")
                {
                    Debug.Log("Error?");
                    sAction = "";
                    gmRef.SoundToneFail();
                    return;
                }
                else if (sAction == "android.nfc.action.TECH_DISCOVERED")
                {
                    Debug.Log("GAME CARD DISCOVERED");
                    // Get PAYLOAD of tag
                    AndroidJavaObject[] mNdefMessage = mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
                    AndroidJavaObject[] mNdefRecord = mNdefMessage[0].Call<AndroidJavaObject[]>("getRecords");
                    // If NDEF PAYLOAD isn't empty, use it!
                    if (mNdefMessage != null)
                    {
                        // Get the payload from the first sector of the NDEF Record
                        byte[] payLoad = mNdefRecord[0].Call<byte[]>("getPayload");
                        // Next convert that from bytes into a string that can be displayed ingame!
                        string text = System.Text.Encoding.UTF8.GetString(payLoad, 1, payLoad.Length - 1);
                        textAsString = text;
                        // Set the output text to the steps read by the game card                       
                        gmRef.SetNewSteps();
                        gmRef.SetStreak();
                        sAction = "";
                    }
                    return;
                }

                else if (sAction == "android.nfc.action.TAG_DISCOVERED")
                {
                    Debug.Log("Error?");
                    sAction = "";
                    gmRef.SoundToneFail();
                    return;
                } else {
                    return; 
                }
            }
            
            catch (Exception ex)
            {
                Debug.Log(ex);
                Debug.Log("Error?");
                sAction = "";
                gmRef.SoundToneFail();
                return;
            }

        }
    }
}
