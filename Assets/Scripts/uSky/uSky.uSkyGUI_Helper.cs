// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.uSkyGUI_Helper
using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("uSky/uSkyGUI Helper")]
public class uSkyGUI_Helper : MonoBehaviour
{
	public uSkyManager m_uSkyManager;

	public Text TimeDisplay;

	public Slider[] slider;

	public void SetTimeline(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Timeline = value;
		}
	}

	public void SetLongitude(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Longitude = value;
		}
	}

	public void SetExposure(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Exposure = value;
		}
	}

	public void SetRayleigh(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.RayleighScattering = value;
		}
	}

	public void SetMie(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.MieScattering = value;
		}
	}

	public void SetSunAnisotropyFactor(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.SunAnisotropyFactor = value;
		}
	}

	public void SetSunSize(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.SunSize = value;
		}
	}

	public void SetWavelength_X(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Wavelengths.x = value;
		}
	}

	public void SetWavelength_Y(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Wavelengths.y = value;
		}
	}

	public void SetWavelength_Z(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.Wavelengths.z = value;
		}
	}

	public void SetStarIntensity(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.StarIntensity = value;
		}
	}

	public void SetOuterSpaceIntensity(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.OuterSpaceIntensity = value;
		}
	}

	public void SetMoonSize(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.MoonSize = value;
		}
	}

	public void SetMoonInnerCoronaScale(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.MoonInnerCorona.a = value;
		}
	}

	public void SetMoonOuterCoronaScale(float value)
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.MoonOuterCorona.a = value;
		}
	}

	private void Start()
	{
		if ((bool)m_uSkyManager)
		{
			m_uSkyManager.SkyUpdate = true;
		}
	}

	private void Update()
	{
		if ((bool)TimeDisplay && (bool)m_uSkyManager)
		{
			/*TimeSpan timeSpan = TimeSpan.FromHours(m_uSkyManager.Timeline);
            TimeDisplay.text = $"{timeSpan.Hours}:{timeSpan.Minutes}";*/
		}
	}

	public void Reset_uSky()
	{
		slider[0].value = 1f;
		slider[1].value = 1f;
		slider[2].value = 1f;
		slider[3].value = 0.76f;
		slider[4].value = 1f;
		slider[5].value = 680f;
		slider[6].value = 550f;
		slider[7].value = 440f;
		slider[8].value = 1f;
		slider[9].value = 0.25f;
		slider[10].value = 0.15f;
		slider[11].value = 0.5f;
		slider[12].value = 0.5f;
	}
}
