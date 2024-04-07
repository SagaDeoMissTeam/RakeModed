// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Oculus_NightVision1
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Oculus/NightVision1")]
[ExecuteInEditMode]
public class CameraFilterPack_Oculus_NightVision1 : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private float Distortion = 1f;

	private Material SCMaterial;

	private Vector4 ScreenResolution;

	[Range(0f, 100f)]
	public float Vignette = 1.3f;

	[Range(1f, 150f)]
	public float Linecount = 90f;

	public static float ChangeVignette;

	public static float ChangeLinecount;

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
		ChangeVignette = Vignette;
		ChangeLinecount = Linecount;
		SCShader = Shader.Find("CameraFilterPack/Oculus_NightVision1");
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
			material.SetFloat("_Distortion", Distortion);
			material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetFloat("_Vignette", Vignette);
			material.SetFloat("_Linecount", Linecount);
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
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
