// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.uSkymapRenderer
using UnityEngine;

[AddComponentMenu("uSky/uSkymap Renderer")]
public class uSkymapRenderer : MonoBehaviour
{
	private RenderTexture m_skyMap;

	public Material m_skymapMaterial;

	public Material m_oceanMaterial;

	public int SkymapResolution = 256;

	[Range(0f, 2f)]
	public float SkyReflection = 1f;

	[Range(0f, 2f)]
	public float CloudReflection = 1f;

	public float CloudTextureScale = 590f;

	public bool DebugSkymap;

	private int m_frameCount;

	private uSkyManager m_uSM;

	private DistanceCloud m_DC;

	private GameObject m_cloudCamera;

	private RenderTexture m_cloudRT;

	private bool RenderCloud;

	private Camera cam;

	protected uSkyManager uSM
	{
		get
		{
			if (m_uSM == null)
			{
				m_uSM = base.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
				{
					Debug.Log("Can't not find uSkyManager");
				}
			}
			return m_uSM;
		}
	}

	private DistanceCloud DC
	{
		get
		{
			if (m_DC == null)
			{
				m_DC = base.gameObject.GetComponent<DistanceCloud>();
				if (m_DC == null)
				{
					Debug.Log("Can't not find DistanceCloud");
				}
			}
			return m_DC;
		}
	}

	private void Start()
	{
		if (!SystemInfo.supportsRenderTextures)
		{
			Debug.LogWarning("RenderTexture is not supported with your Graphic Card");
			return;
		}
		if (uSM == null)
		{
			Debug.Log("Can NOT find uSkyManager, Please asign this uSkymapRenderer script to uSkyManager gameObject.");
			base.enabled = false;
			return;
		}
		m_skyMap = new RenderTexture(SkymapResolution, SkymapResolution, 0, RenderTextureFormat.ARGBHalf);
		m_skyMap.filterMode = FilterMode.Trilinear;
		m_skyMap.wrapMode = TextureWrapMode.Clamp;
		m_skyMap.anisoLevel = 1;
		m_skyMap.Create();
		if (m_skymapMaterial != null)
		{
			InitMaterial(m_skymapMaterial);
			Graphics.Blit(null, m_skyMap, m_skymapMaterial);
		}
		if (m_oceanMaterial != null)
		{
			m_oceanMaterial.SetTexture("_SkyMap", m_skyMap);
			m_oceanMaterial.SetVector("EARTH_POS", new Vector3(0f, 6360010f, 0f));
			updateOceanMaterial(m_oceanMaterial);
		}
		RenderCloud = ((DC != null) ? true : false);
		if (RenderCloud)
		{
			m_cloudRT = new RenderTexture(SkymapResolution, SkymapResolution, 0, RenderTextureFormat.ARGBHalf);
			m_cloudRT.filterMode = FilterMode.Trilinear;
			m_cloudRT.wrapMode = TextureWrapMode.Clamp;
			m_cloudRT.anisoLevel = 1;
			m_cloudRT.Create();
			if (m_cloudCamera == null)
			{
				m_cloudCamera = new GameObject("cloudCamera", typeof(Camera));
			}
			m_cloudCamera.hideFlags = HideFlags.HideInHierarchy;
			m_cloudCamera.transform.Rotate(new Vector3(270f, 0f, 0f));
			cam = m_cloudCamera.GetComponent<Camera>();
			cam.orthographic = true;
			cam.orthographicSize = CloudTextureScale;
			cam.aspect = 1f;
			cam.backgroundColor = Color.black;
			cam.clearFlags = CameraClearFlags.Color;
			cam.cullingMask &= 1 << DC.cloudLayer;
			cam.targetTexture = m_cloudRT;
			m_skymapMaterial.SetTexture("CloudSampler", m_cloudRT);
		}
	}

	private void Update()
	{
		if (m_skyMap != null && m_skymapMaterial != null)
		{
			if (m_frameCount == 1)
			{
				InitMaterial(m_skymapMaterial);
			}
			m_frameCount++;
			Graphics.Blit(null, m_skyMap, m_skymapMaterial);
			InitMaterial(m_skymapMaterial);
			if (m_oceanMaterial != null)
			{
				updateOceanMaterial(m_oceanMaterial);
			}
		}
	}

	private void InitMaterial(Material mat)
	{
		if (uSM != null)
		{
			mat.SetVector("_SunDir", uSM.SunDir);
			mat.SetVector("_betaR", uSM.BetaR);
			mat.SetVector("_betaM", new Vector3(0.004f, 0.004f, 0.004f) * 0.9f);
			mat.SetVector("_SkyMultiplier", new Vector3(uSM.skyMultiplier.x, uSM.skyMultiplier.y * SkyReflection, CloudReflection));
			mat.SetVector("_mieConst", uSM.mieConst);
			mat.SetVector("_miePhase_g", uSM.miePhase_g);
			mat.SetVector("_NightHorizonColor", uSM.getNightHorizonColor() * SkyReflection);
			mat.SetVector("_NightZenithColor", uSM.getNightZenithColor() * SkyReflection);
			mat.SetVector("_colorCorrection", uSM.ColorCorrection);
		}
	}

	private void updateOceanMaterial(Material mat)
	{
		mat.SetVector("SUN_DIR", uSM.SunDir);
		mat.SetFloat("SUN_INTENSITY", 100f * uSM.Exposure);
	}

	private void OnDestroy()
	{
		m_skyMap.Release();
		if (m_cloudRT != null)
		{
			m_cloudRT.Release();
		}
	}
}
