// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Objective
using UnityEngine;

public class Objective : MonoBehaviour
{
	public new string name;

	public bool Status;

	public int id;

	public string description;

	public virtual void ObjectiveAction()
	{
	}
}
