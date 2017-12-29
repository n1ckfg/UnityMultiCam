/*
https://www.linkedin.com/pulse/5-tips-improve-your-photogrammetry-scan-quality-using-jingyi-zhang/

"We've seen people build setups with as few as 64 cameras, but at that point
there's so much occlusion that the mesh is going to need a lot more cleanup...At
our studio, we currently use 128 cameras to ensure that we're not missing any
information...If you are using an APS-C format camera, we recommend using at
least a 24mm lens (the equivalent to about a 38mm on a full frame) for best
results."
*/

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
