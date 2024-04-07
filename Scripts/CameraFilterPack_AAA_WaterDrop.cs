// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_AAA_WaterDrop
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/AAA/WaterDrop")]
[ExecuteInEditMode]
public class CameraFilterPack_AAA_WaterDrop : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	[Range(8f, 64f)]
	public float Distortion = 8f;

	[Range(0f, 7f)]
	public float SizeX = 1f;

	[Range(0f, 7f)]
	public float SizeY = 0.5f;

	[Range(0f, 10f)]
	public float Speed = 1f;

	private Material SCMaterial;

	public Texture2D Texture2;

	public static float ChangeDistortion;

	public static float ChangeSizeX;

	public static float ChangeSizeY;

	public static float ChangeSpeed;

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
		ChangeDistortion = Distortion;
		ChangeSizeX = SizeX;
		ChangeSizeY = SizeY;
		ChangeSpeed = Speed;
		Texture2 = Resources.Load("CameraFilterPack_WaterDrop") as Texture2D;
		SCShader = Shader.Find("CameraFilterPack/AAA_WaterDrop");
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
			material.SetFloat("_SizeX", SizeX);
			material.SetFloat("_SizeY", SizeY);
			material.SetFloat("_Speed", Speed);
			material.SetTexture("_MainTex2", Texture2);
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
			Distortion = ChangeDistortion;
			SizeX = ChangeSizeX;
			SizeY = ChangeSizeY;
			Speed = ChangeSpeed;
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
