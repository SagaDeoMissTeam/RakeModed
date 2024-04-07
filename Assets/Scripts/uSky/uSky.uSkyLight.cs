// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.uSkyLight
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Light")]
[RequireComponent(typeof(uSkyManager))]
public class uSkyLight : MonoBehaviour
{
	[Range(0f, 4f)]
	[Tooltip("Brightness of the Sun (directional light)")]
	public float SunIntensity = 1f;

	[Tooltip("The color of the both Sun and Moon light emitted")]
	public Gradient LightColor = new Gradient
	{
		colorKeys = new GradientColorKey[7]
		{
			new GradientColorKey(new Color32(55, 66, 77, byte.MaxValue), 0.23f),
			new GradientColorKey(new Color32(245, 173, 84, byte.MaxValue), 0.26f),
			new GradientColorKey(new Color32(249, 208, 144, byte.MaxValue), 0.32f),
			new GradientColorKey(new Color32(252, 222, 186, byte.MaxValue), 0.5f),
			new GradientColorKey(new Color32(249, 208, 144, byte.MaxValue), 0.68f),
			new GradientColorKey(new Color32(245, 173, 84, byte.MaxValue), 0.74f),
			new GradientColorKey(new Color32(55, 66, 77, byte.MaxValue), 0.77f)
		},
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		}
	};

	[Tooltip("Toggle the Moon lighting during night time")]
	public bool EnableMoonLighting = true;

	[Range(0f, 2f)]
	[Tooltip("Brightness of the Moon (directional light)")]
	public float MoonIntensity = 0.4f;

	[Tooltip("Ambient light that shines into the scene.")]
	public uSkyAmbient Ambient;

	private uSkyManager _uSM;

	private Light _sun_Light;

	private Light _moon_Light;

    private float currentTime
    {
        get
        {
            if (uSM != null)
                return uSM.Timeline01;
            else
                return 1f;
        }
    }

    private float dayTime
    {
        get
        {
            if (uSM != null)
                return uSM.DayTime;
            else
                return 1f;
        }
    }

    private float nightTime
    {
        get
        {
            if (uSM != null)
                return uSM.NightTime;
            else
                return 0f;
        }
    }

    private float sunsetTime
    {
        get
        {
            if (uSM != null)
                return uSM.SunsetTime;
            else
                return 1f;
        }
    }

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



    private GameObject sunLightObj
	{
		get
		{
			if (uSM != null)
			{
				return (!(uSM.m_sunLight != null)) ? null : uSM.m_sunLight;
			}
			return null;
		}
	}

	private GameObject moonLightObj
	{
		get
		{
			if (uSM != null)
			{
				return (!(uSM.m_moonLight != null)) ? null : uSM.m_moonLight;
			}
			return null;
		}
	}

	private Light sun_Light
	{
		get
		{
			if ((bool)sunLightObj)
			{
				_sun_Light = sunLightObj.GetComponent<Light>();
			}
			if ((bool)_sun_Light)
			{
				return _sun_Light;
			}
			return null;
		}
	}

	private Light moon_Light
	{
		get
		{
			if ((bool)moonLightObj)
			{
				_moon_Light = moonLightObj.GetComponent<Light>();
			}
			if ((bool)_moon_Light)
			{
				return _moon_Light;
			}
			return null;
		}
	}

    private float exposure
    {
        get
        {
            if (uSM != null)
                return uSM.Exposure;
            else
                return 1f;
        }
    }

    private float rayleighSlider
    {
        get
        {
            if (uSM != null)
                return uSM.RayleighScattering;
            else
                return 1f;
        }
    }

    public Color CurrentLightColor
    {
        get { return LightColor.Evaluate(currentTime); }
    }

    public Color CurrentSkyColor
    {
        get { return colorOffset(Ambient.SkyColor.Evaluate(currentTime), 0.15f, 0.7f, IsGround: false); }
    }

    public Color CurrentEquatorColor
    {
        get { return colorOffset(Ambient.EquatorColor.Evaluate(currentTime), 0.15f, 0.9f, IsGround: false); }
    }

    public Color CurrentGroundColor
    {
        get { return colorOffset(Ambient.GroundColor.Evaluate(currentTime), 0.25f, 0.85f, IsGround: true); }
    }
    private void Start()
	{
		if (uSM != null)
		{
			InitUpdate();
		}
	}

	private void Update()
	{
		if (uSM != null && uSM.SkyUpdate)
		{
			InitUpdate();
		}
	}

	private void InitUpdate()
	{
		SunAndMoonLightUpdate();
		if (RenderSettings.ambientMode == AmbientMode.Trilight)
		{
			AmbientGradientUpdate();
		}
		else
		{
			RenderSettings.ambientLight = CurrentSkyColor;
		}
	}

	private void SunAndMoonLightUpdate()
	{
		if (sunLightObj != null && sun_Light != null)
		{
			sun_Light.intensity = uSM.Exposure * SunIntensity * dayTime;
			sun_Light.color = CurrentLightColor * dayTime;
			sun_Light.enabled = ((!(currentTime < 0.24f) && !(currentTime > 0.76f)) ? true : false);
		}
		if (moonLightObj != null)
		{
			if (moon_Light != null)
			{
				moon_Light.intensity = uSM.Exposure * MoonIntensity * nightTime;
				moon_Light.color = CurrentLightColor * nightTime;
				moon_Light.enabled = (!(currentTime > 0.26f) || !(currentTime < 0.74f)) && EnableMoonLighting && (EnableMoonLighting ? true : false);
				if (!EnableMoonLighting)
				{
					forceSunEnableAtNight();
				}
			}
		}
		else
		{
			forceSunEnableAtNight();
		}
	}

	private void forceSunEnableAtNight()
	{
		if ((bool)sun_Light)
		{
			sun_Light.enabled = true;
			sun_Light.intensity = Mathf.Max(0.001f, sun_Light.intensity);
			sun_Light.color = new Color(sun_Light.color.r, sun_Light.color.g, sun_Light.color.b, Mathf.Max(0.01f, sun_Light.color.a));
		}
	}

	private void AmbientGradientUpdate()
	{
		RenderSettings.ambientSkyColor = CurrentSkyColor;
		RenderSettings.ambientEquatorColor = CurrentEquatorColor;
		RenderSettings.ambientGroundColor = CurrentGroundColor;
	}

	private Color colorOffset(Color currentColor, float offsetRange, float rayleighOffsetRange, bool IsGround)
	{
		Vector3 vector = ((!(uSM != null)) ? new Vector3(5.81f, 13.57f, 33.13f) : (uSM.BetaR * 1000f));
		Vector3 vector2 = new Vector3(0.5f, 0.5f, 0.5f);
		vector2 = new Vector3(vector.x / 5.81f * 0.5f, vector.y / 13.57f * 0.5f, vector.z / 33.13f * 0.5f);
		if (!IsGround)
		{
			vector2 = Vector3.Lerp(new Vector3(Mathf.Abs(1f - vector2.x), Mathf.Abs(1f - vector2.y), Mathf.Abs(1f - vector2.z)), vector2, sunsetTime);
		}
		vector2 = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), vector2, dayTime);
		Vector3 vector3 = default(Vector3);
		vector3 = new Vector3(Mathf.Lerp(currentColor.r - offsetRange, currentColor.r + offsetRange, vector2.x), Mathf.Lerp(currentColor.g - offsetRange, currentColor.g + offsetRange, vector2.y), Mathf.Lerp(currentColor.b - offsetRange, currentColor.b + offsetRange, vector2.z));
		Vector3 to = new Vector3(vector3.x / vector.x, vector3.y / vector.y, vector3.z / vector.z) * 4f;
		vector3 = ((!(rayleighSlider < 1f)) ? Vector3.Lerp(vector3, to, Mathf.Max(0f, rayleighSlider - 1f) / 4f * rayleighOffsetRange) : Vector3.Lerp(Vector3.zero, vector3, rayleighSlider));
		return new Color(vector3.x, vector3.y, vector3.z, 1f) * exposure;
	}
}
