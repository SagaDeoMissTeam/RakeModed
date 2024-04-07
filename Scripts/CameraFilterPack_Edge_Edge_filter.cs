// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Edge_Edge_filter
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Edge/Edge_filter")]
[ExecuteInEditMode]
public class CameraFilterPack_Edge_Edge_filter : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 10f)]
	public float RedAmplifier;

	[Range(0f, 10f)]
	public float GreenAmplifier = 2f;

	[Range(0f, 10f)]
	public float BlueAmplifier;

	public static float ChangeRedAmplifier;

	public static float ChangeGreenAmplifier;

	public static float ChangeBlueAmplifier;

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
		ChangeRedAmplifier = RedAmplifier;
		ChangeGreenAmplifier = GreenAmplifier;
		ChangeBlueAmplifier = BlueAmplifier;
		SCShader = Shader.Find("CameraFilterPack/Edge_Edge_filter");
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
			material.SetFloat("_RedAmplifier", RedAmplifier);
			material.SetFloat("_GreenAmplifier", GreenAmplifier);
			material.SetFloat("_BlueAmplifier", BlueAmplifier);
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
			RedAmplifier = ChangeRedAmplifier;
			GreenAmplifier = ChangeGreenAmplifier;
			BlueAmplifier = ChangeBlueAmplifier;
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
