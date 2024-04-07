// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ExampleScene
using UnityEngine;

public class ExampleScene : MonoBehaviour
{
	public bool showingText = true;

	private void OnGUI()
	{
		if (showingText)
		{
			GUI.color = Color.blue;
			GUILayout.BeginArea(new Rect(10f, (float)Camera.main.pixelHeight - 100f, Camera.main.pixelWidth, 100f));
			GUILayout.Label("Hit ~ to show/hide the console.\nBegin by typing the name of a GameObject and use the autocomplete to find variables and methods to run.\nTry entering 'Capsule.ExampleObject.SetMaterial Green' (without 's) to call the SetMaterial method with 'Green' as the parameter.\nTry the Alias system. One is setup for you already. Enter 'FastCube' (without 's).");
			GUILayout.EndArea();
		}
	}
}
