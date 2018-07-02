using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class MultiCamManager : MonoBehaviour {

    public enum MultiMode { CAM_TO_DISPLAY, CAM_TO_SCREEN, CAM_TO_TEX };
    public MultiMode mmode = MultiMode.CAM_TO_DISPLAY;

	public Camera renderCam;
    public SettingsManager settingsMgr;
	public float fov = 70f;
	public int screenWidth = 1920;
	public int screenHeight = 1080;
	public int refreshRate = 60;
    public float nearClip = 0.01f;
    public float farClip = 1000f;
	public bool fullscreen = true;
	public float step = 0.1f;
	public Camera[] cam;
	public int controlDisplay = 0;

	[HideInInspector] public MultiCamControls[] mcc;

	// Unity 5.5 allows up to 8 displays
	private KeyCode[] keyCodes = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
	};

	void Awake() {
		mcc = new MultiCamControls[cam.Length];
		for (int i = 0; i < cam.Length; i++) {
			cam[i].fieldOfView = fov;
            cam[i].nearClipPlane = nearClip;
            cam[i].farClipPlane = farClip;
			mcc[i] = cam[i].GetComponent<MultiCamControls>();
			mcc[i].screenWidth = screenWidth;
			mcc[i].screenHeight = screenHeight;
			mcc[i].fullscreen = fullscreen;
			mcc[i].step = step;
		}

        if (renderCam != null) mmode = MultiMode.CAM_TO_TEX;
	}

	void Start() {
		if (mmode == MultiMode.CAM_TO_DISPLAY) {
			// https://web.archive.org/web/20160505150236/http://docs.unity3d.com/Manual/MultiDisplay.html
			// Display 0 is always active
			for (int i = 1; i < Display.displays.Length; i++) {
				//Display.displays[i].Activate();
				Display.displays[i].Activate(screenWidth, screenHeight, refreshRate);
			}
		} else if (mmode == MultiMode.CAM_TO_SCREEN) {
            for (int i=0; i<cam.Length; i++) {
                cam[i].targetDisplay = 0;
                float width = 1f / cam.Length;
                float offset = width * i;
      
                cam[i].rect = new Rect(offset, 0f, width, 1f);
            }
        }

        for (int i = 0; i < mcc.Length; i++) {
            mcc[i].ready = true;
        }
    }

	void Update() {
		if (mmode == MultiMode.CAM_TO_DISPLAY || mmode == MultiMode.CAM_TO_SCREEN) {
			for (int i = 0; i < keyCodes.Length; i++) {
				if (Input.GetKeyDown(keyCodes[i])) {
					controlDisplay = i;
					//if (Application.isEditor) EditorApplication.ExecuteMenuItem("Window/Game/Maximize On Play");
				}
			}
			for (int i = 0; i < mcc.Length; i++) {
				mcc[i].controlDisplay = controlDisplay;
			}
		}
	}

}
