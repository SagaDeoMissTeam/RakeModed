// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ConsoleWatch
using System;
using System.Collections.Generic;
using System.Reflection;
using Homans.Console;
using UnityEngine;

internal class ConsoleWatch : MonoBehaviour
{
	private class Watch
	{
		public string name;

		public FieldInfo field;

		public PropertyInfo property;

		public WeakReference instance;

		public string lastValue;
	}

	private List<Watch> watches = new List<Watch>();

	private void Start()
	{
		Console.Instance.RegisterCommand("AddWatch", this, "AddWatchCommand");
		InvokeRepeating("UpdateWatches", 1f, 1f);
	}

	private void UpdateWatches()
	{
		watches.RemoveAll((Watch m) => m.instance.Target == null);
		foreach (Watch watch in watches)
		{
			if (watch.field != null)
			{
				watch.lastValue = watch.field.GetValue(watch.instance.Target).ToString();
			}
			else if (watch.property != null)
			{
				watch.lastValue = watch.property.GetValue(watch.instance.Target, null).ToString();
			}
		}
	}

	private void OnGUI()
	{
		foreach (Watch watch in watches)
		{
			GUILayout.Label(watch.name + ": " + watch.lastValue);
		}
	}

	public void AddWatchField(string name, string fieldName, object instance)
	{
		Watch watch = new Watch();
		watch.name = name;
		watch.instance = new WeakReference(instance, trackResurrection: false);
		watch.field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (instance != null && watch.field != null)
		{
			watches.Add(watch);
		}
	}

	public void AddWatchProperty(string name, string fieldName, object instance)
	{
		Watch watch = new Watch();
		watch.name = name;
		watch.instance = new WeakReference(instance, trackResurrection: false);
		watch.property = instance.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (instance != null && watch.property != null)
		{
			watches.Add(watch);
		}
	}

	[Help("Usage: \"AddWatch name object.component.field\"\nDisplays the given field or property on the screen. Will automaticly update.")]
	private void AddWatchCommand(string name, string goPath)
	{
		Console.parseGameObjectString(goPath, out var gameobjectPath, out var componentName, out var methodName);
		string text = string.Empty;
		string[] array = gameobjectPath;
		foreach (string text2 in array)
		{
			text = text + "/" + text2;
		}
		GameObject gameObject = GameObject.Find(text);
		if (gameObject == null)
		{
			Console.Instance.Print("Unknown gameobject");
			return;
		}
		Component component = gameObject.GetComponent(componentName);
		if (component == null)
		{
			Console.Instance.Print("Unknown component");
			return;
		}
		FieldInfo field = component.GetType().GetField(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (field == null)
		{
			PropertyInfo property = component.GetType().GetProperty(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				Console.Instance.Print("Unknown field or property");
			}
			else
			{
				AddWatchProperty(name, methodName, component);
			}
		}
		else
		{
			AddWatchField(name, methodName, component);
		}
	}
}
