// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Oculus_NightVision2
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Oculus/NightVision2")]
public class CameraFilterPack_Oculus_NightVision2 : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float BinocularSize = 0.5f;

	[Range(0f, 1f)]
	public float BinocularDistance = 0.5f;

	[Range(0f, 1f)]
	public float Greenness = 0.4f;

	public static float ChangeBinocularSize;

	public static float ChangeBinocularDistance;

	public static float ChangeGreenness;

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
		ChangeBinocularSize = BinocularSize;
		ChangeBinocularDistance = BinocularDistance;
		ChangeGreenness = Greenness;
		SCShader = Shader.Find("CameraFilterPack/Oculus_NightVision2");
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
			material.SetFloat("_BinocularSize", BinocularSize);
			material.SetFloat("_BinocularDistance", BinocularDistance);
			material.SetFloat("_Greenness", Greenness);
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
			BinocularSize = ChangeBinocularSize;
			BinocularDistance = ChangeBinocularDistance;
			Greenness = ChangeGreenness;
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
