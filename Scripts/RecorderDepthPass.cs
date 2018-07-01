using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderDepthPass : MonoBehaviour {

    public Material depthMat;
    public bool bypass = false;

    private Camera cam;

    private void Awake() {
        cam = GetComponent<Camera>();
	}
    
    private void OnRenderImage(RenderTexture source, RenderTexture dest) {
        // depthMat material contains shader that reads the destination RenderTexture
        if (!bypass && depthMat) {
            cam.depthTextureMode = DepthTextureMode.Depth;
            Graphics.Blit(source, dest, depthMat);
        } else {
            cam.depthTextureMode = DepthTextureMode.None;
            Graphics.Blit(source,dest);
        }
    }

}
