using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayaCamera : MonoBehaviour {

    public Transform mayaCamera;
    public float mayaAngleOfView = 54.43f; // Maya default

    private void Awake() {
        if (mayaCamera != null) mayaCameraCorrect();
    }

    private void mayaCameraCorrect() {
        Camera.main.transform.position = mayaCamera.transform.position;
        Camera.main.transform.rotation = mayaCamera.transform.rotation * Quaternion.Euler(0, 180, 0);
        Camera.main.transform.parent = mayaCamera.transform;
        Camera.main.fieldOfView = aovToFov();
    }

    private float aovToFov() {
        float fov = mayaAngleOfView * 0.596f;
        if (fov < 1f) fov = 1f;
        return fov;
    }

}
