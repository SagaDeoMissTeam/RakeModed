// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Colors_Brightness
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Colors/Brightness")]
[ExecuteInEditMode]
public class CameraFilterPack_Colors_Brightness : MonoBehaviour
{
	public Shader SCShader;

	[Range(0f, 2f)]
	public float _Brightness = 1.5f;

	private Material SCMaterial;

	public static float ChangeBrightness;

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
		ChangeBrightness = _Brightness;
		SCShader = Shader.Find("CameraFilterPack/Colors_Brightness");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetFloat("_Val", _Brightness);
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
			_Brightness = ChangeBrightness;
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
