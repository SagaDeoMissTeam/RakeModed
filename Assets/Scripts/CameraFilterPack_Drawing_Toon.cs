// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Drawing_Toon
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Drawing/Toon")]
[ExecuteInEditMode]
public class CameraFilterPack_Drawing_Toon : MonoBehaviour
{
	public Shader SCShader;

	private Material SCMaterial;

	private float TimeX = 1f;

	[Range(0f, 2f)]
	public float Threshold = 1f;

	[Range(0f, 8f)]
	public float DotSize = 1f;

	public static float ChangeThreshold;

	public static float ChangeDotSize;

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
		ChangeThreshold = Threshold;
		ChangeDotSize = DotSize;
		SCShader = Shader.Find("CameraFilterPack/Drawing_Toon");
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
			material.SetFloat("_Distortion", Threshold);
			material.SetFloat("_DotSize", DotSize);
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
			Threshold = ChangeThreshold;
			DotSize = ChangeDotSize;
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
