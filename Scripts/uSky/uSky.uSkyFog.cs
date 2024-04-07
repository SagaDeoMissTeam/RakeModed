// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.uSkyFog
using UnityEngine;

[AddComponentMenu("uSky/uSky Fog (Image Effects)")]
[ExecuteInEditMode]
public class uSkyFog : MonoBehaviour
{
	public enum FogModes
	{
		Linear = 1,
		Exponential,
		Exponential_Squared
	}

	public FogModes fogMode = FogModes.Exponential;

	public bool useRadialDistance;

	[Range(0.0001f, 1f)]
	public float Density = 0.001f;

	[Range(0.06f, 0.4f)]
	public float ColorDecay = 0.2f;

	[Range(0f, 1f)]
	public float Scattering = 1f;

	[Range(0f, 0.1f)]
	public float HorizonOffset;

	public float StartDistance;

	public float EndDistance = 300f;

	public Material FogMaterial;

	private uSkyManager _uSM;

	private Material _skybox;

	private uSkyManager uSM
	{
		get
		{
			if (_uSM == null)
			{
				_uSM = base.gameObject.GetComponent<uSkyManager>();
			}
			return _uSM;
		}
	}

	private Material skybox
	{
		get
		{
			if (_skybox == null && uSM != null)
			{
				_skybox = base.gameObject.GetComponent<uSkyManager>().SkyboxMaterial;
			}
			return _skybox;
		}
	}

	private void Start()
	{
		updateFog(FogMaterial);
		skybox.SetFloat("_HorizonOffset", HorizonOffset);
	}

	private void Update()
	{
		FogModes fogModes = fogMode;
		float density = Density;
		float startDistance = StartDistance;
		float endDistance = EndDistance;
		bool flag = fogModes == FogModes.Linear;
		float num = ((!flag) ? 0f : (endDistance - startDistance));
		float num2 = ((!(Mathf.Abs(num) > 0.0001f)) ? 0f : (1f / num));
		Vector4 vector = default(Vector4);
		vector.x = density * 1.2011224f;
		vector.y = density * 1.442695f;
		vector.z = ((!flag) ? 0f : (0f - num2));
		vector.w = ((!flag) ? 0f : (endDistance * num2));
		FogMaterial.SetVector("_SceneFogParams", vector);
		FogMaterial.SetVector("_SceneFogMode", new Vector4((float)fogModes, useRadialDistance ? 1 : 0, 0f, 0f));
		FogMaterial.SetVector("_fParams", new Vector4((fogMode != FogModes.Linear) ? 0f : (0f - Mathf.Max(StartDistance, 0f)), ColorDecay, Scattering, 0f));
		updateFog(FogMaterial);
	}

	private void updateFog(Material mat)
	{
		if (uSM != null && mat != null && uSM.SkyUpdate)
		{
			uSM.InitMaterial(mat);
			skybox.SetFloat("_HorizonOffset", HorizonOffset);
		}
	}

	public void SetFogDensity(float value)
	{
		Density = value;
	}

	public void SetFogColorDecay(float value)
	{
		ColorDecay = value;
	}
}
