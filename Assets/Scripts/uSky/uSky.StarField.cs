// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// uSky.StarField
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarField
{
	private struct Star
	{
		public Vector3 position;

		public Color color;
	}

	private float starSizeScale = 1f;

	private List<CombineInstance> starQuad = new List<CombineInstance>();

	public Mesh InitializeStarfield()
	{
		float num = ((Camera.main != null) ? (Camera.main.farClipPlane - 10f) : ((!(Camera.current != null)) ? 990f : Camera.current.farClipPlane));
		float size = num / 100f * starSizeScale;
		TextAsset textAsset = Resources.Load<TextAsset>("StarsData");
		if (textAsset == null)
		{
			Debug.Log("Can't find or read StarsData.bytes file.");
			return null;
		}
		Star[] array = new Star[9110];
		using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(textAsset.bytes)))
		{
			for (int i = 0; i < 9110; i++)
			{
				array[i].position.x = binaryReader.ReadSingle();
				array[i].position.z = binaryReader.ReadSingle();
				array[i].position.y = binaryReader.ReadSingle();
				array[i].position = Vector3.Scale(array[i].position, new Vector3(-1f, 1f, -1f));
				array[i].color.r = binaryReader.ReadSingle();
				array[i].color.g = binaryReader.ReadSingle();
				array[i].color.b = binaryReader.ReadSingle();
				float a = Vector3.Dot(new Vector3(array[i].color.r, array[i].color.g, array[i].color.b), new Vector3(0.22f, 0.707f, 0.071f));
				array[i].color.a = a;
				if (!(array[i].color.a < 0.0162f) && !(array[i].color.a > 2.2f))
				{
					CombineInstance item = default(CombineInstance);
					item.mesh = createQuad(size);
					item.transform = BillboardMatrix(array[i].position * num);
					Color[] colors = new Color[4]
					{
						array[i].color,
						array[i].color,
						array[i].color,
						array[i].color
					};
					item.mesh.colors = colors;
					starQuad.Add(item);
				}
			}
		}
		Mesh mesh = new Mesh();
		mesh.name = "StarFieldMesh";
		mesh.CombineMeshes(starQuad.ToArray());
		mesh.Optimize();
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2E+09f);
		return mesh;
	}

	protected Mesh createQuad(float size)
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(1f, 1f, 0f) * size,
			new Vector3(-1f, 1f, 0f) * size,
			new Vector3(1f, -1f, 0f) * size,
			new Vector3(-1f, -1f, 0f) * size
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0f, 0f),
			new Vector2(1f, 0f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.hideFlags = HideFlags.DontSave;
		return mesh;
	}

	private Matrix4x4 BillboardMatrix(Vector3 particlePosition)
	{
		Vector3 vector = particlePosition - Vector3.zero;
		vector.Normalize();
		Vector3 vector2 = Vector3.Cross(vector, Vector3.up);
		vector2.Normalize();
		Vector3 vector3 = Vector3.Cross(vector2, vector);
		Matrix4x4 result = default(Matrix4x4);
		result.SetColumn(0, vector2);
		result.SetColumn(1, vector3);
		result.SetColumn(2, vector);
		result.SetColumn(3, particlePosition);
		return result;
	}
}
