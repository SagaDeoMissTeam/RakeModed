// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Sharpen_Sharpen
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Sharpen/Sharpen")]
[ExecuteInEditMode]
public class CameraFilterPack_Sharpen_Sharpen : MonoBehaviour
{
	public Shader SCShader;

	[Range(0.001f, 40f)]
	public float Value = 4f;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public static float ChangeValue;

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
		ChangeValue = Value;
		SCShader = Shader.Find("CameraFilterPack/Sharpen_Sharpen");
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
			material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetFloat("_Value", Value);
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
			Value = ChangeValue;
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
