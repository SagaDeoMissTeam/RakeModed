// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ConsoleParsers
using System.Collections;
using UnityEngine;

internal class ConsoleParsers : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(InitParsers());
	}

	private IEnumerator InitParsers()
	{
		yield return null;
		Console.Instance.RegisterParser(typeof(Vector3), parseVector3);
	}

	private bool parseVector3(string v, out object obj)
	{
		Vector3 vector = default(Vector3);
		string[] array = v.Split(',');
		if (!float.TryParse(array[0], out var result))
		{
			Console.Instance.Print("Invalid Vector3: " + array[0] + " is not a float");
			obj = null;
			return false;
		}
		vector.x = result;
		if (!float.TryParse(array[1], out result))
		{
			Console.Instance.Print("Invalid Vector3: " + array[1] + " is not a float");
			obj = null;
			return false;
		}
		vector.y = result;
		if (!float.TryParse(array[2], out result))
		{
			Console.Instance.Print("Invalid Vector3: " + array[2] + " is not a float");
			obj = null;
			return false;
		}
		vector.z = result;
		obj = vector;
		return true;
	}

	private void vector3ParseTest(Vector3 vector)
	{
		Console.Instance.Print(vector.x.ToString());
		Console.Instance.Print(vector.y.ToString());
		Console.Instance.Print(vector.z.ToString());
	}
}
