﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class SettingsManager : MonoBehaviour {

    public string fileName = "settings.json";
    public bool loadOnStart = true;

    [HideInInspector] public JSONNode json;

    private void Awake() {
        if (loadOnStart) load();
    }

    public void load() {
        StartCoroutine(loadSettings());
    }

    public void save() {
        StartCoroutine(saveSettings());
    }

    private IEnumerator loadSettings() {
        string url;

        #if UNITY_ANDROID
		url = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", fileName);
        #endif

        #if UNITY_IOS
		url = Path.Combine("file://" + Application.dataPath + "/Raw", fileName);
        #endif

        #if UNITY_EDITOR
        url = Path.Combine("file://" + Application.dataPath, fileName);
        #endif

        #if UNITY_STANDALONE_WIN
        url = Path.Combine("file://" + Application.dataPath, fileName);
        #endif

        #if UNITY_STANDALONE_OSX
		url = Path.Combine("file://" + Application.dataPath, fileName);		
        #endif

        #if UNITY_WSA
		url = Path.Combine("file://" + Application.dataPath, fileName);		
        #endif

        WWW www = new WWW(url);
        yield return www;

        json = JSON.Parse(www.text);

        Debug.Log(json);
    }

    private IEnumerator saveSettings() {
        string url = Path.Combine(Application.dataPath, fileName);

        #if UNITY_ANDROID
		url = "/sdcard/Movies/" + fileName;
        #endif

        #if UNITY_IOS
		url = Path.Combine(Application.persistentDataPath, fileName);
        #endif

        File.WriteAllText(url, json.ToString());

        yield return null;
    }

}

public class SettingsJson {

    public int mode;
    public int width;
    public int height;
    public float fov;
    public float near_clip;
    public float far_clip;
    public float refresh_rate;
    public bool fullscreen;
    public bool use_volume_trigger;
    
    public List<CameraObj> cameras;

    [System.Serializable]
    public struct CameraObj {
        public string name;
        public float rotation_y;
        public int target_display;
        public int control_display;
    }

}
