// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// LevelLoader
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	public static LevelLoader instance;

	private void Start()
	{
		instance = this;
	}

	public void LoadLevel(int id)
	{
		SaveGameSystem.instance.SaveGame();
	}
}
