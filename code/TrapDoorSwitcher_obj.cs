// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TrapDoorSwitcher_obj
public class TrapDoorSwitcher_obj : DynamicObject
{
	public bool DoorStage;

	public AnimalCage_obj cage;

	public void Update()
	{
		if (!DoorStage)
		{
			name = "Press E to open door";
		}
		else
		{
			name = "Press E to close door";
		}
	}

	public override void Action()
	{
		if (!DoorStage)
		{
			cage.OpenCage();
		}
		else
		{
			cage.CloseCage();
		}
	}
}
