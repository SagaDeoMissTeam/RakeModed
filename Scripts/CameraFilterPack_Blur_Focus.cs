// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Blur_Focus
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Blur/Focus")]
public class CameraFilterPack_Blur_Focus : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(-1f, 1f)]
	public float CenterX;

	[Range(-1f, 1f)]
	public float CenterY;

	[Range(0f, 10f)]
	public float _Size = 5f;

	[Range(0.12f, 64f)]
	public float _Eyes = 2f;

	public static float ChangeCenterX;

	public static float ChangeCenterY;

	public static float ChangeSize;

	public static float ChangeEyes;

	private Material material
	{
		get
		{
			if (SCMaterial == null)
			{
				SCMaterial = new Material(SCShader);
				SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return SCMaterial;
		}
	}

	private void Start()
	{
		ChangeCenterX = CenterX;
		ChangeCenterY = CenterY;
		ChangeSize = _Size;
		ChangeEyes = _Eyes;
		SCShader = Shader.Find("CameraFilterPack/Blur_Focus");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			TimeX += Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_CenterX", CenterX);
			material.SetFloat("_CenterY", CenterY);
			float value = Mathf.Round(_Size / 0.2f) * 0.2f;
			material.SetFloat("_Size", value);
			material.SetFloat("_Circle", _Eyes);
			material.SetVector("_ScreenResolution", new Vector2(Screen.width, Screen.height));
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			CenterX = ChangeCenterX;
			CenterY = ChangeCenterY;
			_Size = ChangeSize;
			_Eyes = ChangeEyes;
		}
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			Object.DestroyImmediate(SCMaterial);
		}
	}
}
