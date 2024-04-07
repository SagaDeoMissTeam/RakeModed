// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SetupCameras_objective
public class SetupCameras_objective : Objective
{
	public override void ObjectiveAction()
	{
		if (!Status)
		{
			ObjectiveManager.instance.CompleteObjective(this);
		}
	}
}
