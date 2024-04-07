// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// NotificationsManager
using System.Collections.Generic;
using UnityEngine;

public class NotificationsManager : MonoBehaviour
{
	private Dictionary<string, List<Component>> Listeners = new Dictionary<string, List<Component>>();

	private static NotificationsManager instance;

	public static NotificationsManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new NotificationsManager();
			}
			return instance;
		}
	}

	public void AddListener(Component Sender, string NotificationName)
	{
		if (!Listeners.ContainsKey(NotificationName))
		{
			Listeners.Add(NotificationName, new List<Component>());
		}
		Listeners[NotificationName].Add(Sender);
	}

	public void RemoveListener(Component Sender, string NotificationName)
	{
		if (!Listeners.ContainsKey(NotificationName))
		{
			return;
		}
		for (int num = Listeners[NotificationName].Count - 1; num >= 0; num--)
		{
			if (Listeners[NotificationName][num].GetInstanceID() == Sender.GetInstanceID())
			{
				Listeners[NotificationName].RemoveAt(num);
			}
		}
	}

	public void PostNotification(Component Sender, string NotificationName)
	{
		if (!Listeners.ContainsKey(NotificationName))
		{
			return;
		}
		foreach (Component item in Listeners[NotificationName])
		{
			item.SendMessage(NotificationName, Sender, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void ClearListeners()
	{
		Listeners.Clear();
	}

	public void RemoveRedundancies()
	{
		Dictionary<string, List<Component>> dictionary = new Dictionary<string, List<Component>>();
		foreach (KeyValuePair<string, List<Component>> listener in Listeners)
		{
			for (int num = listener.Value.Count - 1; num >= 0; num--)
			{
				if (listener.Value[num] == null)
				{
					listener.Value.RemoveAt(num);
				}
			}
			if (listener.Value.Count > 0)
			{
				dictionary.Add(listener.Key, listener.Value);
			}
		}
		Listeners = dictionary;
	}

	private void OnLevelWasLoaded()
	{
		RemoveRedundancies();
	}

	private void Awake()
	{
		if ((bool)instance)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
