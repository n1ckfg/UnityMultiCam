using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://forum.unity3d.com/threads/rendering-screenshot-using-multiple-cameras-and-one-rendertexture.135916/

public class MultiCamRender : MonoBehaviour {

	public MultiCamManager multiCamMgr;
	public RenderTexture rTex;

	void Awake() {
		rTex.width = multiCamMgr.cam.Length * multiCamMgr.screenWidth;
		rTex.height = multiCamMgr.screenHeight;
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (multiCamMgr.renderCam != null) {
			Graphics.Blit(createCubemap(multiCamMgr.screenWidth, multiCamMgr.screenHeight), multiCamMgr.renderCam.targetTexture);
		}
	}

	public Texture2D createCubemap(int a_Width, int a_Height) {
		Texture2D result = new Texture2D(a_Width * multiCamMgr.cam.Length, a_Height, TextureFormat.ARGB32, false);

		for (int i=0; i<multiCamMgr.cam.Length; i++) {
			RenderTexture renderTexture = RenderTexture.GetTemporary(a_Width, a_Height, 24);
			RenderTexture.active = renderTexture;  

			if (multiCamMgr.cam[i].enabled) {
				float fov = multiCamMgr.cam[i].fieldOfView;
				multiCamMgr.cam[i].targetTexture = renderTexture;
				multiCamMgr.cam[i].Render();
				multiCamMgr.cam[i].targetTexture = null;
				multiCamMgr.cam[i].fieldOfView = fov;
			}

			result.ReadPixels(new Rect(0f, 0f, a_Width, a_Height), i * a_Width, 0, false);
			result.Apply();

			RenderTexture.active = null;

			RenderTexture.ReleaseTemporary(renderTexture);
		}

     	return result;
	}


	public Texture2D getScreenshot(int a_Width, int a_Height) {
		//List<Camera> cameras = new List<Camera>(Camera.allCameras);
		RenderTexture renderTexture = RenderTexture.GetTemporary(a_Width, a_Height, 24);
		RenderTexture.active = renderTexture;  

		//foreach (Camera camera in cameras) {
		foreach (Camera camera in multiCamMgr.cam) {
			if (camera.enabled) {
				float fov = camera.fieldOfView;
				camera.targetTexture = renderTexture;
				camera.Render();
				camera.targetTexture = null;
				camera.fieldOfView = fov;
			}
		}

		Texture2D result = new Texture2D(a_Width, a_Height, TextureFormat.ARGB32, false);
		result.ReadPixels(new Rect(0.0f, 0.0f, a_Width, a_Height), 0, 0, false);
		result.Apply();

		RenderTexture.active = null;

		RenderTexture.ReleaseTemporary(renderTexture);

		return result;
	}

}
