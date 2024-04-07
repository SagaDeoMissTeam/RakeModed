// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// LevelItemsManager
using System.Collections.Generic;
using UnityEngine;

public class LevelItemsManager : MonoBehaviour
{
	public List<GameObject> trapObjects = new List<GameObject>();

	public List<GameObject> items = new List<GameObject>();

	public List<GameObject> screemers = new List<GameObject>();

	public List<GameObject> soundTriggers = new List<GameObject>();

	public List<GameObject> windowsObj = new List<GameObject>();

	public static LevelItemsManager instance;

	private void Start()
	{
		instance = this;
	}

	public void Update()
	{
	}

	public void AddTrap(GameObject obj)
	{
		trapObjects.Add(obj);
	}

	public void RefreshTrapList()
	{
		for (int num = trapObjects.Count - 1; num >= 0; num--)
		{
			if (trapObjects[num] == null)
			{
				trapObjects.RemoveAt(num);
			}
		}
	}

	public void RefreshSoundTriggersList()
	{
		for (int num = soundTriggers.Count - 1; num >= 0; num--)
		{
			if (soundTriggers[num] == null || !soundTriggers[num].activeSelf)
			{
				soundTriggers.RemoveAt(num);
			}
		}
	}

	public void RefreshScreemersList()
	{
		for (int num = screemers.Count - 1; num >= 0; num--)
		{
			if (screemers[num] == null || !screemers[num].activeSelf)
			{
				screemers.RemoveAt(num);
			}
		}
	}

	public void RefrashItemList()
	{
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (items[num] == null || !items[num].activeSelf)
			{
				MonoBehaviour.print("disable " + num);
				items.RemoveAt(num);
			}
		}
	}
}
