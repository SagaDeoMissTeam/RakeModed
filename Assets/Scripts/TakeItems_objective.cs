// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TakeItems_objective
public class TakeItems_objective : Objective
{
	public override void ObjectiveAction()
	{
		if (!Status)
		{
			ObjectiveManager.instance.CompleteObjective(this);
		}
	}
}
