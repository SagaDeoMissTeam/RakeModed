// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AI_Test
using UnityEngine;

public class AI_Test : MonoBehaviour
{
	public NavMeshAgent nav;

	public bool bPathIsSet;

	public Transform target;

	public NavMeshPath path;

	private void Start()
	{
		path = new NavMeshPath();
		nav.CalculatePath(target.position, path);
	}

	private void Update()
	{
		nav.SetDestination(target.position);
		if (nav.path.status == NavMeshPathStatus.PathPartial)
		{
			Debug.Log("PAth is partial");
		}
		if (nav.path.status == NavMeshPathStatus.PathInvalid)
		{
			Debug.Log("PAth is invalid");
		}
	}
}
