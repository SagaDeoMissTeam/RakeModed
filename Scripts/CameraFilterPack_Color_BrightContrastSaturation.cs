// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Color_BrightContrastSaturation
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Colors/BrightContrastSaturation")]
public class CameraFilterPack_Color_BrightContrastSaturation : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 10f)]
	public float Brightness = 2f;

	[Range(0f, 10f)]
	public float Saturation = 1.5f;

	[Range(0f, 10f)]
	public float Contrast = 1.5f;

	public static float ChangeBrightness;

	public static float ChangeSaturation;

	public static float ChangeContrast;

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
		ChangeBrightness = Brightness;
		ChangeSaturation = Saturation;
		ChangeContrast = Contrast;
		SCShader = Shader.Find("CameraFilterPack/Color_BrightContrastSaturation");
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
			material.SetFloat("_Brightness", Brightness);
			material.SetFloat("_Saturation", Saturation);
			material.SetFloat("_Contrast", Contrast);
			material.SetFloat("_TimeX", TimeX);
			material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
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
			Brightness = ChangeBrightness;
			Saturation = ChangeSaturation;
			Contrast = ChangeContrast;
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
