using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamControls : MonoBehaviour {

	public int screenWidth = 1920;
	public int screenHeight = 1080;
	public bool fullscreen = true;
	public float step = 0.1f;
	public int controlDisplay = 0;
    public bool disableControlDisplay = true;

	[HideInInspector] public bool ready = false;

	private float spinX = 0f;
	private float spinY = 0f;
	private float inputX = 0f;
	private float inputY = 0f;
	private bool clicked = false;
	private Camera cam;
	private bool firstRun = true;
	private float stepOrig = 0f;

	void Awake() {
		cam = GetComponent<Camera>();
		stepOrig = step;
	}

	/*
	void Start() {
		Screen.SetResolution(screenWidth, screenHeight, fullscreen);
	}
	*/

	void Update() {
		if (firstRun && ready) {
			Screen.SetResolution(screenWidth, screenHeight, fullscreen);
			firstRun = false;
		}

		clicked = false;
		step = stepOrig;
		inputX = 0f;
		inputY = 0f;

		if (controlDisplay == cam.targetDisplay && !disableControlDisplay) {
			if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
				clicked = true;

				if (Input.GetMouseButton(0)) {
					inputX = -Input.GetAxis ("Mouse X");
					inputY = Input.GetAxis ("Mouse Y");
				}

				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) step *= 10f;
				if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) step /= 10f;

				if (Input.GetKeyDown(KeyCode.UpArrow)) inputY = step;
				if (Input.GetKeyDown(KeyCode.DownArrow)) inputY = -step;
				if (Input.GetKeyDown(KeyCode.LeftArrow)) inputX = step;
				if (Input.GetKeyDown(KeyCode.RightArrow)) inputX = -step;
			}

			if (clicked) {
				float h = 40.0f * inputX * Time.deltaTime;
				float v = 40.0f * inputY * Time.deltaTime;
				h = Mathf.Clamp (h, -0.5f, 0.5f);
				v = Mathf.Clamp (v, -0.5f, 0.5f);
				spinX += h;
				spinY += v;
			}
		}

		if (!Mathf.Approximately(spinX, 0f) || !Mathf.Approximately(spinY, 0f)) {
			this.transform.Rotate(Vector3.up, spinX);
			this.transform.Rotate(Vector3.right, spinY);

			spinX = Mathf.MoveTowards(spinX, 0f, 5f * Time.deltaTime);
			spinY = Mathf.MoveTowards(spinY, 0f, 5f * Time.deltaTime);
		}
	}

}
