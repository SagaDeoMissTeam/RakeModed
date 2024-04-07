// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DetectionTriggerRake
using UnityEngine;

public class DetectionTriggerRake : MonoBehaviour
{
	public Enemy_AI ai;

	private void OnTriggerEnter(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<RecordCamera_obj>() && info.gameObject.GetComponent<RecordCamera_obj>().cameraState == RecordCamera_obj.Camera_state.Setup && CameraComputer.instance.bWatching)
		{
			Vector3 sourcePosition = info.gameObject.transform.TransformPoint(Vector3.forward * 2f);
			NavMesh.SamplePosition(sourcePosition, out var hit, 5f, 1);
			ai.currentBreakPoint = hit.position;
			ai.viewRot = info.gameObject.transform.position;
			ai.cameraBreak = info.gameObject.transform;
			ai.SetBreakState();
		}
		if ((bool)info.gameObject.GetComponent<Animal_AI>() && !ai.bHaunt && !ai.bPlayerHount && ai.bHungry)
		{
			ai.target = info.gameObject.transform;
			ai.bHaunt = true;
		}
		if (info.gameObject.tag == "Player")
		{
			ai.SetPlayerHountState();
			ai.playerTarget = info.gameObject.transform;
		}
	}
}
