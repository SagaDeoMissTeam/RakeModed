// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Vintage
using UnityEngine;

[ExecuteInEditMode]
public class Vintage : MonoBehaviour
{
	private Shader curShader;

	private Color Yellow = Color.yellow;

	private Color Cyan = Color.cyan;

	private Color Magenta = Color.magenta;

	public float YellowLevel = 0.01f;

	public float CyanLevel = 0.03f;

	public float MagentaLevel = 0.04f;

	private Material curMaterial;

	private Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else
		{
			curShader = Shader.Find("Custom/Vintage");
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (curShader != null)
		{
			material.SetFloat("_YellowLevel", YellowLevel);
			material.SetFloat("_CyanLevel", CyanLevel);
			material.SetFloat("_MagentaLevel", MagentaLevel);
			material.SetColor("_Yellow", Yellow);
			material.SetColor("_Cyan", Cyan);
			material.SetColor("_Magenta", Magenta);
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if ((bool)curMaterial)
		{
			Object.DestroyImmediate(curMaterial);
		}
	}
}
