// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSkyManager
using System;
using UnityEngine;

[AddComponentMenu("uSky/uSky Manager")]
[ExecuteInEditMode]
public class uSkyManager : MonoBehaviour
{
	public enum NightModes
	{
		Off = 1,
		Static,
		Rotation
	}

	[Tooltip("Update of the sky calculations in each frame.")]
	public bool SkyUpdate = true;

	[Tooltip("This value controls the light vertically. It represents sunrise/day and sunset/night time( Rotation X )")]
	[Range(0f, 24f)]
	public float Timeline = 17f;

	[Range(-180f, 180f)]
	[Tooltip("This value controls the light horizionally.( Rotation Y )")]
	public float Longitude;

	[Space(10f)]
	[Tooltip("This value sets the brightness of the sky.(for day time only)")]
	[Range(0f, 5f)]
	public float Exposure = 1f;

	[Tooltip("Rayleigh scattering is caused by particles in the atmosphere (up to 8 km). It produces typical earth-like sky colors (reddish/yellowish colors at sun set, and the like).")]
	[Range(0f, 5f)]
	public float RayleighScattering = 1f;

	[Range(0f, 5f)]
	[Tooltip("Mie scattering is caused by aerosols in the lower atmosphere (up to 1.2 km). It is for haze and halos around the sun on foggy days.")]
	public float MieScattering = 1f;

	[Range(0f, 0.9995f)]
	[Tooltip("The anisotropy factor controls the sun's appearance in the sky.The closer this value gets to 1.0, the sharper and smaller the sun spot will be. Higher values cause more fuzzy and bigger sun spots.")]
	public float SunAnisotropyFactor = 0.76f;

	[Tooltip("Size of the sun spot in the sky")]
	[Range(0.001f, 10f)]
	public float SunSize = 1f;

	[Tooltip("It is visible spectrum light waves. Tweaking these values will shift the colors of the resulting gradients and produce different kinds of atmospheres.")]
	public Vector3 Wavelengths = new Vector3(680f, 550f, 440f);

	[Tooltip("It is wavelength dependent. Tweaking these values will shift the colors of sky color.")]
	public Color SkyTint = new Color(0.5f, 0.5f, 0.5f, 1f);

	[Tooltip("It is the bottom half color of the skybox")]
	public Color m_GroundColor = new Color(0.369f, 0.349f, 0.341f, 1f);

	[Tooltip("It is a Directional Light from the scene, it represents Sun Ligthing")]
	public GameObject m_sunLight;

	[Space(10f)]
	public NightModes NightSky = NightModes.Static;

	[Tooltip("The zenith color of the night sky gradient. (Top of the night sky)")]
	public Gradient NightZenithColor = new Gradient
	{
		colorKeys = new GradientColorKey[4]
		{
			new GradientColorKey(new Color32(50, 71, 99, byte.MaxValue), 0.225f),
			new GradientColorKey(new Color32(74, 107, 148, byte.MaxValue), 0.25f),
			new GradientColorKey(new Color32(74, 107, 148, byte.MaxValue), 0.75f),
			new GradientColorKey(new Color32(50, 71, 99, byte.MaxValue), 0.775f)
		},
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		}
	};

	[Tooltip("The horizon color of the night sky gradient.")]
	public Color NightHorizonColor = new Color(0.43f, 0.47f, 0.5f, 1f);

	[Tooltip("This controls the intensity of the Stars field in night sky.")]
	[Range(0f, 5f)]
	public float StarIntensity = 1f;

	[Range(0f, 2f)]
	[Tooltip("This controls the intensity of the Outer Space Cubemap in night sky.")]
	public float OuterSpaceIntensity = 0.25f;

	[Tooltip("The color of the moon's inner corona. This Alpha value controls the size and blurriness corona.")]
	public Color MoonInnerCorona = new Color(1f, 1f, 1f, 0.5f);

	[Tooltip("The color of the moon's outer corona. This Alpha value controls the size and blurriness corona.")]
	public Color MoonOuterCorona = new Color(0.25f, 0.39f, 0.5f, 0.5f);

	[Tooltip("This controls the moon texture size in the night sky.")]
	[Range(0f, 1f)]
	public float MoonSize = 0.15f;

	[Tooltip("It is additional Directional Light from the scene, it represents Moon Ligthing.")]
	public GameObject m_moonLight;

	[Tooltip("It is the uSkybox Material of the uSky.")]
	public Material SkyboxMaterial;

	[SerializeField]
	[Tooltip("It will automatically assign the current skybox material to Render Settings.")]
	private bool _AutoApplySkybox = true;

	[HideInInspector]
	public bool LinearSpace;

	[Tooltip("Toggle it if the Main Camera is using HDR mode and Tonemapping image effect.")]
	public bool Tonemapping;

	private Vector3 euler;

	private Matrix4x4 moon_wtl;

	private Material m_starMaterial;

	private Mesh _starsMesh;

	private readonly Vector3 BetaM = new Vector3(0.004f, 0.004f, 0.004f) * 0.9f;

    private bool EnableNightSky
    {
        get { return NightSky != NightModes.Off; }
    }

    public bool AutoApplySkybox
	{
		get
		{
			return _AutoApplySkybox;
		}
		set
		{
			if (value && (bool)SkyboxMaterial && RenderSettings.skybox != SkyboxMaterial)
			{
				RenderSettings.skybox = SkyboxMaterial;
			}
			_AutoApplySkybox = value;
		}
	}

	protected Material starMaterial
	{
		get
		{
			if (m_starMaterial == null)
			{
				m_starMaterial = new Material(Shader.Find("Hidden/uSky/Stars"));
				m_starMaterial.hideFlags = HideFlags.DontSave;
			}
			return m_starMaterial;
		}
	}

	public Mesh starsMesh
	{
		get
		{
			StarField starField = new StarField();
			if (_starsMesh == null)
			{
				_starsMesh = starField.InitializeStarfield();
			}
			return _starsMesh;
		}
	}

    public float Timeline01
    {
        get { return Timeline / 24f; }
    }

    public Vector3 SunDir
	{
		get { return (!(m_sunLight != null)) ? new Vector3(0.321f, 0.766f, -0.557f) : (m_sunLight.transform.forward * -1f); }
	}

	private Matrix4x4 getMoonMatrix
	{
		get
		{
			if (m_moonLight == null)
			{
				moon_wtl = Matrix4x4.TRS(Vector3.zero, new Quaternion(-0.9238795f, 8.817204E-08f, 8.817204E-08f, 0.3826835f), Vector3.one);
			}
			else if (m_moonLight != null)
			{
				moon_wtl = m_moonLight.transform.worldToLocalMatrix;
				moon_wtl.SetColumn(2, Vector4.Scale(new Vector4(1f, 1f, 1f, -1f), moon_wtl.GetColumn(2)));
			}
			return moon_wtl;
		}
	}

    private Vector3 variableRangeWavelengths
    {
        get
        {
            float newX = Mathf.Lerp(Wavelengths.x + 150f, Wavelengths.x - 150f, SkyTint.r);
            float newY = Mathf.Lerp(Wavelengths.y + 150f, Wavelengths.y - 150f, SkyTint.g);
            float newZ = Mathf.Lerp(Wavelengths.z + 150f, Wavelengths.z - 150f, SkyTint.b);

            return new Vector3(newX, newY, newZ);
        }
    }
    public Vector3 BetaR
	{
		get
		{
			Vector3 vector = variableRangeWavelengths * 1E-09f;
			Vector3 vector2 = new Vector3(Mathf.Pow(vector.x, 4f), Mathf.Pow(vector.y, 4f), Mathf.Pow(vector.z, 4f));
			Vector3 vector3 = 7.635E+25f * vector2 * 5.755f;
			float num = 8f * Mathf.Pow((float)Math.PI, 3f) * Mathf.Pow(0.0006002188f, 2f) * 6.105f;
			return 1000f * new Vector3(num / vector3.x, num / vector3.y, num / vector3.z);
		}
	}

    private Vector3 betaR_RayleighOffset
    {
        get { return BetaR * Mathf.Max(0.001f, RayleighScattering); }
    }

    public float uMuS
    {
        get { return Mathf.Atan(Mathf.Max(SunDir.y, -0.1975f) * 5.35f) / 1.1f + 0.739f; }
    }

    public float DayTime
    {
        get { return Mathf.Clamp01(uMuS); }
    }

    public float SunsetTime
    {
        get { return Mathf.Clamp01((uMuS - 1f) * (1.5f / Mathf.Pow(RayleighScattering, 4f))); }
    }

    public float NightTime
    {
        get { return 1f - DayTime; }
    }

    public Vector3 miePhase_g
	{
		get
		{
			float num = SunAnisotropyFactor * SunAnisotropyFactor;
			float num2 = ((!LinearSpace || !Tonemapping) ? 1f : 2f);
			return new Vector3(num2 * ((1f - num) / (2f + num)), 1f + num, 2f * SunAnisotropyFactor);
		}
	}

	public Vector3 mieConst
	{
		get
		{
			Vector3 vector = new Vector3(1f, BetaR.x / BetaR.y, BetaR.x / BetaR.z);
			Vector3 betaM = BetaM;
			return vector * betaM.x * MieScattering;
		}
	}

    public Vector3 skyMultiplier
    {
        get
        {
            float x = SunsetTime;
            float y = Exposure * 4f * DayTime * Mathf.Sqrt(RayleighScattering);
            float z = NightTime;
            return new Vector3(x, y, z);
        }
    }

    private Vector3 bottomTint
	{
		get
		{
			float num = ((!LinearSpace) ? 0.02f : 0.01f);
			return new Vector3(betaR_RayleighOffset.x / (m_GroundColor.r * num), betaR_RayleighOffset.y / (m_GroundColor.g * num), betaR_RayleighOffset.z / (m_GroundColor.b * num));
		}
	}

    public Vector2 ColorCorrection
    {
        get
        {
            if (LinearSpace && Tonemapping)
                return new Vector2(0.38317f, 1.413f);
            else if (!LinearSpace)
                return Vector2.one;
            else
                return new Vector2(1f, 2f);
        }
    }

    public Color getNightHorizonColor()
    {
        return NightHorizonColor * NightTime;
    }

    public Color getNightZenithColor()
    {
        return NightZenithColor.Evaluate(Timeline01) * 0.01f;
    }

    private float moonCoronaFactor
	{
		get
		{
			float num = 0f;
			float y = m_sunLight.transform.forward.y;
			if (NightSky == NightModes.Rotation)
			{
				return NightTime * y;
			}
			return NightTime;
		}
	}

    private Vector4 getMoonInnerCorona()
    {
        return new Vector4(MoonInnerCorona.r * moonCoronaFactor, MoonInnerCorona.g * moonCoronaFactor, MoonInnerCorona.b * moonCoronaFactor, 400f / MoonInnerCorona.a);
    }
    private Vector4 getMoonOuterCorona
	{
		get
		{
			float num = ((!LinearSpace) ? 8f : ((!Tonemapping) ? 12f : 16f));
			return new Vector4(MoonOuterCorona.r * 0.25f * moonCoronaFactor, MoonOuterCorona.g * 0.25f * moonCoronaFactor, MoonOuterCorona.b * 0.25f * moonCoronaFactor, num / MoonOuterCorona.a);
		}
	}

	private float starBrightness
	{
		get
		{
			float num = ((!LinearSpace) ? 1.5f : 1f);
			return StarIntensity * NightTime * num;
		}
	}

	private void OnEnable()
	{
		if (m_sunLight == null)
		{
			m_sunLight = GameObject.Find("Directional Light");
		}
		InitMaterial(SkyboxMaterial);
	}

	private void OnDisable()
	{
		if ((bool)starsMesh)
		{
			UnityEngine.Object.DestroyImmediate(starsMesh);
		}
		if ((bool)m_starMaterial)
		{
			UnityEngine.Object.DestroyImmediate(m_starMaterial);
		}
	}

	private void detectColorSpace()
	{
		if (SkyboxMaterial != null)
		{
			InitMaterial(SkyboxMaterial);
		}
	}

	private void Start()
	{
		InitSun();
		InitMoon();
		if (SkyboxMaterial != null)
		{
			InitMaterial(SkyboxMaterial);
		}
		AutoApplySkybox = _AutoApplySkybox;
	}

	private void Update()
	{
		if (SkyUpdate)
		{
			if (Timeline >= 24f)
			{
				Timeline = 0f;
			}
			if (Timeline < 0f)
			{
				Timeline = 24f;
			}
			if (SkyboxMaterial != null)
			{
				InitSun();
				InitMoon();
				InitMaterial(SkyboxMaterial);
			}
		}
		if (EnableNightSky && starsMesh != null && starMaterial != null && SunDir.y < 0.2f)
		{
			Graphics.DrawMesh(starsMesh, Vector3.zero, Quaternion.identity, starMaterial, 0);
		}
	}

	private void InitSun()
	{
		euler.x = Timeline * 360f / 24f - 90f;
		euler.y = Longitude;
		if (m_sunLight != null)
		{
			m_sunLight.transform.localEulerAngles = euler;
		}
	}

	public void InitMaterial(Material mat)
	{
		mat.SetVector("_SunDir", SunDir);
		mat.SetMatrix("_Moon_wtl", getMoonMatrix);
		mat.SetVector("_betaR", betaR_RayleighOffset);
		mat.SetVector("_betaM", BetaM);
		mat.SetVector("_SkyMultiplier", skyMultiplier);
		mat.SetFloat("_SunSize", 32f / SunSize);
		mat.SetVector("_mieConst", mieConst);
		mat.SetVector("_miePhase_g", miePhase_g);
		mat.SetVector("_GroundColor", bottomTint);
		mat.SetVector("_NightHorizonColor", getNightHorizonColor());
		mat.SetVector("_NightZenithColor", getNightZenithColor());
		mat.SetVector("_MoonInnerCorona", getMoonInnerCorona());
		mat.SetVector("_MoonOuterCorona", getMoonOuterCorona);
		mat.SetFloat("_MoonSize", MoonSize);
		mat.SetVector("_colorCorrection", ColorCorrection);
		if (Tonemapping)
		{
			mat.DisableKeyword("USKY_HDR_OFF");
			mat.EnableKeyword("USKY_HDR_ON");
		}
		else
		{
			mat.EnableKeyword("USKY_HDR_OFF");
			mat.DisableKeyword("USKY_HDR_ON");
		}
		if (EnableNightSky)
		{
			mat.DisableKeyword("NIGHTSKY_OFF");
		}
		else
		{
			mat.EnableKeyword("NIGHTSKY_OFF");
		}
		if (NightSky == NightModes.Rotation)
		{
			mat.SetMatrix("rotationMatrix", m_sunLight.transform.worldToLocalMatrix);
		}
		else
		{
			mat.SetMatrix("rotationMatrix", Matrix4x4.identity);
		}
		mat.SetFloat("_OuterSpaceIntensity", OuterSpaceIntensity);
		if (starMaterial != null)
		{
			starMaterial.SetFloat("StarIntensity", starBrightness);
			if (NightSky == NightModes.Rotation)
			{
				starMaterial.SetMatrix("rotationMatrix", m_sunLight.transform.localToWorldMatrix);
			}
			else
			{
				starMaterial.SetMatrix("rotationMatrix", Matrix4x4.identity);
			}
		}
	}

	private void InitMoon()
	{
		if (NightSky == NightModes.Rotation)
		{
			m_moonLight.transform.forward = m_sunLight.transform.forward * -1f;
		}
	}
}
