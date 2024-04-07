// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CustomCommandTest
using Homans.Console;
using UnityEngine;

public class CustomCommandTest : MonoBehaviour
{
	private void Start()
	{
		Console.Instance.RegisterCommand("customCommandTest", this, "Command");
	}

	[Help("Usage: customCommandTest param\nA simple custom command test")]
	private void Command(string param1)
	{
		Console.Instance.Print("Called with value " + param1);
	}
}
