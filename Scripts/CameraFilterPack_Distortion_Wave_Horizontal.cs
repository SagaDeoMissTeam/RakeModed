// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Distortion_Wave_Horizontal
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Distortion/Wave_Horizontal")]
[ExecuteInEditMode]
public class CameraFilterPack_Distortion_Wave_Horizontal : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(1f, 100f)]
	public float WaveIntensity = 32f;

	public static float ChangeWaveIntensity;

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
		ChangeWaveIntensity = WaveIntensity;
		SCShader = Shader.Find("CameraFilterPack/Distortion_Wave_Horizontal");
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
			material.SetFloat("_WaveIntensity", WaveIntensity);
			material.SetFloat("_TimeX", TimeX);
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
			WaveIntensity = ChangeWaveIntensity;
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
