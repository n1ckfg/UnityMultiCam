using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class MultiCamManager : MonoBehaviour {

	public Camera renderCam;
	public float fov = 70f;
	public int screenWidth = 1920;
	public int screenHeight = 1080;
	public int refreshRate = 60;
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
			mcc[i] = cam[i].GetComponent<MultiCamControls>();
			mcc[i].screenWidth = screenWidth;
			mcc[i].screenHeight = screenHeight;
			mcc[i].fullscreen = fullscreen;
			mcc[i].step = step;
		}
	}

	void Start() {
		if (renderCam == null) {
			// https://web.archive.org/web/20160505150236/http://docs.unity3d.com/Manual/MultiDisplay.html
			// Display 0 is always active
			for (int i = 1; i < Display.displays.Length; i++) {
				//Display.displays[i].Activate();
				Display.displays[i].Activate(screenWidth, screenHeight, refreshRate);
			}
			for (int i = 0; i < mcc.Length; i++) {
				mcc[i].ready = true;
			}
		}
	}

	void Update() {
		if (renderCam == null) {
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
