using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LoadCamera : MonoBehaviour
{
    static int REQUEST_IMAGE_CAPTURE = 0;

    public void StartPlugin()
    {
        AndroidJavaClass IntentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject IntentObject = new AndroidJavaObject("android.content.Intent");

        AndroidJavaObject MSObject = new AndroidJavaObject("android.provider.MediaStore");

        IntentObject.Call<AndroidJavaObject>("setAction", MSObject.GetStatic<string>("ACTION_IMAGE_CAPTURE"));

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivityForResult", IntentObject, REQUEST_IMAGE_CAPTURE);

    }
}
