using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArray : MonoBehaviour {

    public Camera camPrefab;
    //public float fov = 60f;
    //public Color bgColor = new Color(0f, 0f, 0f, 1f);

    [HideInInspector] public List<Camera> cams;

    private Mesh mesh;
    private Vector3[] vertices;
    private Renderer ren;

    private void Awake() {
        ren = GetComponent<Renderer>();
        ren.enabled = false;
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        cams = new List<Camera>();

        for (int i = 0; i < vertices.Length; i++) {
            Vector3 p = transform.TransformPoint(vertices[i]);

            bool makeNewCam = true;
            for (int j=0; j < cams.Count; j++) {
                if (cams[j].transform.position == p) {
                    makeNewCam = false;
                    break;
                }
            }

            if (makeNewCam) {
                Camera cam = Instantiate(camPrefab, transform);// new GameObject().AddComponent<Camera>();
                cam.name = "ArrayCamera_" + (i+1);

                /*
                cam.backgroundColor = bgColor;
                cam.fieldOfView = fov;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.nearClipPlane = 0.01f;
                cam.farClipPlane = 1000f;
                */

                //cam.transform.parent = transform;
                cam.transform.position = p;
                cam.transform.LookAt(transform);
                cams.Add(cam);
            }
        }
    }

}
