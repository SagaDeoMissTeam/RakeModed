// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CustomParseTest
using Homans.Console;
using System;
using UnityEngine;

public class CustomParseTest : MonoBehaviour
{
	private void Start()
	{
		Console.Instance.RegisterCommand("customParseTest", this, "Command");
		Console.Instance.RegisterParser(typeof(TestObject), ParseTestObject);
	}

	[Help("Usage: customParseTest param\nA simple custom parse test")]
	private void Command(TestObject param1)
	{
		Console.Instance.Print("Called with value " + param1.x + ", " + param1.y);
	}

	public bool ParseTestObject(string line, out object obj)
	{
		string[] array = line.Split(',');
		if (array.Length != 2)
		{
			obj = null;
			return false;
		}
		Int32 result;
        if (!int.TryParse(array[0], out result))
		{
			obj = null;
			return false;
		}
        Int32 result2;
        if (!int.TryParse(array[1], out result2))
		{
			obj = null;
			return false;
		}
		TestObject testObject = default(TestObject);
		testObject.x = result;
		testObject.y = result2;
		obj = testObject;
		return true;
	}
}
