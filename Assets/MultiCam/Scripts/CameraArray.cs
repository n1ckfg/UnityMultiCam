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

    public enum AlignMode { CENTER, UP, NONE };
    public enum LensMode { FIELD_OF_VIEW, FOCAL_LENGTH };
    public AlignMode alignMode = AlignMode.CENTER;
    public LensMode lensMode = LensMode.FIELD_OF_VIEW;
    public float fieldOfView = 24f;
    public float focalLength = 38f;
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
                cam.name = "ArrayCamera_" + (i + 1);

                if (lensMode == LensMode.FIELD_OF_VIEW) {
                    cam.fieldOfView = fieldOfView;
                } else if (lensMode == LensMode.FOCAL_LENGTH) {
                    cam.fieldOfView = flToFov();
                }
                cam.transform.position = p;

                if (alignMode == AlignMode.CENTER) {
                    cam.transform.LookAt(transform);
                } else if (alignMode == AlignMode.UP) {
                    cam.transform.rotation = Quaternion.LookRotation(transform.up);
                }

                cams.Add(cam);
            }
        }
    }

    private float flToFov() {
        // https://answers.unity.com/questions/431046/is-it-possible-to-get-the-actual-simulated-lens-in.html
        // http://paulbourke.net/miscellaneous/lens/
        // https://en.wikipedia.org/wiki/35mm_format -- 35mm film is 36mm x 24mm
        float fov = 100f * 2f * Mathf.Atan((0.5f * 24f) / focalLength);
        Debug.Log(fov);
        if (fov < 1f) fov = 1f;
        return fov;
    }

}
