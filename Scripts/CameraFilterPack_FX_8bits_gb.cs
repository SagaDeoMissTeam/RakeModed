// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_FX_8bits_gb
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/FX/8bits_gb")]
public class CameraFilterPack_FX_8bits_gb : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Material SCMaterial;

	[Range(-1f, 1f)]
	public float Brightness;

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
		ChangeBrightness = Brightness;
		SCShader = Shader.Find("CameraFilterPack/FX_8bits_gb");
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
			if (Brightness == 0f)
			{
				Brightness = 0.001f;
			}
			material.SetFloat("_Distortion", Brightness);
			RenderTexture temporary = RenderTexture.GetTemporary(160, 144, 0);
			Graphics.Blit(sourceTexture, temporary, material);
			temporary.filterMode = FilterMode.Point;
			Graphics.Blit(temporary, destTexture);
			RenderTexture.ReleaseTemporary(temporary);
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
