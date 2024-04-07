// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Distortion_Half_Sphere
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Distortion/Half_Sphere")]
[ExecuteInEditMode]
public class CameraFilterPack_Distortion_Half_Sphere : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	[Range(1f, 6f)]
	private Vector4 ScreenResolution;

	private Material SCMaterial;

	public float SphereSize = 2.5f;

	[Range(-1f, 1f)]
	public float SpherePositionX;

	[Range(-1f, 1f)]
	public float SpherePositionY;

	[Range(1f, 10f)]
	public float Strength = 5f;

	public static float ChangeSphereSize;

	public static float ChangeSpherePositionX;

	public static float ChangeSpherePositionY;

	public static float ChangeStrength;

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
		ChangeSphereSize = SphereSize;
		ChangeSpherePositionX = SpherePositionX;
		ChangeSpherePositionY = SpherePositionY;
		ChangeStrength = Strength;
		SCShader = Shader.Find("CameraFilterPack/Distortion_Half_Sphere");
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
			material.SetFloat("_SphereSize", SphereSize);
			material.SetFloat("_SpherePositionX", SpherePositionX);
			material.SetFloat("_SpherePositionY", SpherePositionY);
			material.SetFloat("_Strength", Strength);
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
			SphereSize = ChangeSphereSize;
			SpherePositionX = ChangeSpherePositionX;
			SpherePositionY = ChangeSpherePositionY;
			Strength = ChangeStrength;
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
