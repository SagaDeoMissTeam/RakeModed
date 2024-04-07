// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Blur_Noise
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Blur/Noise")]
[ExecuteInEditMode]
public class CameraFilterPack_Blur_Noise : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(2f, 16f)]
	public int Level = 4;

	public Vector2 Distance = new Vector2(30f, 0f);

	public static int ChangeLevel;

	public static Vector2 ChangeDistance;

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
		ChangeLevel = Level;
		ChangeDistance = Distance;
		SCShader = Shader.Find("CameraFilterPack/Blur_Noise");
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
			material.SetFloat("_Level", Level);
			material.SetVector("_Distance", Distance);
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
			Level = ChangeLevel;
			Distance = ChangeDistance;
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
