// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraFilterPack_Drawing_CellShading
using UnityEngine;

[AddComponentMenu("Camera Filter Pack/Drawing/CellShading")]
[ExecuteInEditMode]
public class CameraFilterPack_Drawing_CellShading : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float EdgeSize = 0.1f;

	[Range(0f, 10f)]
	public float ColorLevel = 4f;

	public static float ChangeEdgeSize;

	public static float ChangeColorLevel;

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
		ChangeEdgeSize = EdgeSize;
		ChangeColorLevel = ColorLevel;
		SCShader = Shader.Find("CameraFilterPack/Drawing_CellShading");
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
			material.SetFloat("_EdgeSize", EdgeSize);
			material.SetFloat("_ColorLevel", ColorLevel);
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
			EdgeSize = ChangeEdgeSize;
			ColorLevel = ChangeColorLevel;
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
