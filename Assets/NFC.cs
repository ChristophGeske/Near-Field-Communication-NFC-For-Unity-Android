using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class NFC : MonoBehaviour {

	public string tagID;
	public Text tag_output_text;
	public bool tagFound = false;

	private AndroidJavaObject mActivity;
	private AndroidJavaObject mIntent;
	private string sAction;


	void Start() {
		tag_output_text.text = "No tag...";
	}

	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
			if (!tagFound) {
				try {
					// Create new NFC Android object
					mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
					mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
					sAction = mIntent.Call<String>("getAction");
					if (sAction == "android.nfc.action.NDEF_DISCOVERED") {
						Debug.Log("Tag of type NDEF");
					}
					else if (sAction == "android.nfc.action.TECH_DISCOVERED") {
						Debug.Log("TAG DISCOVERED");
						// Get ID of tag
						AndroidJavaObject mNdefMessage = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
						if (mNdefMessage != null) {
							byte[] payLoad = mNdefMessage.Call<byte[]>("getId");
							string text = System.Convert.ToBase64String(payLoad);
							tag_output_text.text = text + "HereWeAre";
							Destroy (GetComponent("MeshRenderer")); //Destroy when ID is displayed
							tagID = text;
						}
						else {
							tag_output_text.text = "No ID found !";
						}
						tagFound = true;
						return;
					}
					else if (sAction == "android.nfc.action.TAG_DISCOVERED") {
						Debug.Log("This type of tag is not supported !");
					}
					else {
						tag_output_text.text = "No tag...";
						return;
					}
				}
				catch (Exception ex) {
					string text = ex.Message;
					tag_output_text.text = text;
				}
			}
		}
		if (tagFound) {
			Destroy (GetComponent("MeshRenderer"));
		}
	}
}