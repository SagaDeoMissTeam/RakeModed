// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Colors_HSV
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Colors/HSV")]
[ExecuteInEditMode]
public class CameraFilterPack_Colors_HSV : MonoBehaviour
{
	public Shader SCShader;

	[Range(0f, 360f)]
	public float _HueShift = 180f;

	[Range(-32f, 32f)]
	public float _Saturation = 1f;

	[Range(-32f, 32f)]
	public float _ValueBrightness = 1f;

	private Material SCMaterial;

	public static float ChangeHueShift;

	public static float ChangeSaturation;

	public static float ChangeValueBrightness;

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
		ChangeHueShift = _HueShift;
		ChangeSaturation = _Saturation;
		ChangeValueBrightness = _ValueBrightness;
		SCShader = Shader.Find("CameraFilterPack/Colors_HSV");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetFloat("_HueShift", _HueShift);
			material.SetFloat("_Sat", _Saturation);
			material.SetFloat("_Val", _ValueBrightness);
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
			_HueShift = ChangeHueShift;
			_Saturation = ChangeSaturation;
			_ValueBrightness = ChangeValueBrightness;
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
