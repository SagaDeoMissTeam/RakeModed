// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.DistanceCloud
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(uSkyManager), typeof(uSkyLight))]
[AddComponentMenu("uSky/Distance Cloud (beta)")]
public class DistanceCloud : MonoBehaviour
{
	private const float NightBrightness = 0.25f;

	private uSkyManager m_uSM;

	private uSkyLight m_uSL;

	public int cloudLayer = 18;

	public Material CloudMaterial;

	private Mesh skyDome;

	protected uSkyManager uSM
	{
		get
		{
			if (m_uSM == null)
			{
				m_uSM = base.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
				{
					Debug.Log(" Can't not find uSkyManager Component, Please apply DistanceCloud in uSkyManager gameobject");
				}
			}
			return m_uSM;
		}
	}

	protected uSkyLight uSL
	{
		get
		{
			if (m_uSL == null)
			{
				m_uSL = base.gameObject.GetComponent<uSkyLight>();
				if (m_uSL == null)
				{
					Debug.Log("Can't not find uSkyLight Component, Please apply DistanceCloud in uSkyManager gameobject");
				}
			}
			return m_uSL;
		}
	}

	protected Mesh InitSkyDomeMesh()
	{
		Mesh mesh = Resources.Load<Mesh>("Hemisphere_Mesh");
		if (mesh == null)
		{
			Debug.Log("Can't find Hemisphere_Mesh.fbx file.");
			return null;
		}
		Mesh mesh2 = new Mesh();
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i].y *= 0.85f;
		}
		mesh2.vertices = vertices;
		mesh2.triangles = mesh.triangles;
		mesh2.normals = mesh.normals;
		mesh2.uv = mesh.uv;
		mesh2.uv2 = mesh.uv2;
		mesh2.bounds = new Bounds(Vector3.zero, Vector3.one * 2E+09f);
		mesh2.hideFlags = HideFlags.DontSave;
		mesh2.name = "skydomeMesh";
		return mesh2;
	}

	private void OnEnable()
	{
		if (skyDome == null)
		{
			skyDome = InitSkyDomeMesh();
		}
	}

	private void OnDisable()
	{
		if ((bool)skyDome)
		{
			Object.DestroyImmediate(skyDome);
		}
	}

	private void Start()
	{
		if (uSM != null && uSL != null)
		{
			UpdateCloudMaterial();
		}
	}

	private void Update()
	{
		if (uSM != null && uSM.SkyUpdate && uSL != null)
		{
			UpdateCloudMaterial();
		}
		if ((bool)skyDome && (bool)CloudMaterial)
		{
			Graphics.DrawMesh(skyDome, Vector3.zero, Quaternion.identity, CloudMaterial, cloudLayer);
		}
	}

	private void UpdateCloudMaterial()
	{
		float num = Mathf.Max(Mathf.Pow(0.25f, (!uSM.LinearSpace) ? 1f : 1.5f), uSM.DayTime);
		num *= Mathf.Sqrt(uSM.Exposure);
		if (CloudMaterial != null)
		{
			CloudMaterial.SetVector("ShadeColorFromSun", (!uSM.LinearSpace) ? (uSL.CurrentLightColor * num) : (uSL.CurrentLightColor.linear * num));
			CloudMaterial.SetVector("ShadeColorFromSky", (!uSM.LinearSpace) ? (uSL.CurrentSkyColor * num) : (uSL.CurrentSkyColor.linear * num));
		}
	}
}
