// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ObjectiveManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
	public List<Objective> objective = new List<Objective>();

	public Objective currentObjective;

	public int currentObjectiveID;

	private int objCounter;

	public static ObjectiveManager instance;

	private bool bShowObjective;

	public void Start()
	{
		instance = this;
		ShowCurrentObjective();
	}

	public void CompleteObjective(Objective obj)
	{
		if (currentObjective == obj)
		{
			obj.Status = true;
			Debug.Log(obj);
			InformationUI.instance.DrawObjective("Objective: " + currentObjective.name + " has been completed");
			StartCoroutine(showObjective());
		}
	}

	private IEnumerator showObjective()
	{
		yield return new WaitForSeconds(3f);
		SetupNextObjective();
	}

	public void SetupNextObjective()
	{
		objCounter++;
		if (objective[objCounter] != null)
		{
			currentObjective = objective[objCounter];
			ShowCurrentObjective();
		}
	}

	public void SetupNewObjective(int id)
	{
	}

	public void RemoveObjective(Objective obj)
	{
	}

	public void ShowCurrentObjective()
	{
		InformationUI.instance.DrawObjective("Objective: " + currentObjective.name);
	}
}
