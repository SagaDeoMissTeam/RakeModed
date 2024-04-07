// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RECCamera_obj
using UnityEngine;

public class MP_RECCamera_obj : MP_DynamicObject
{
	[RPC]
	public virtual void SetupCamera()
	{
	}

	public virtual bool CheckSetup()
	{
		return false;
	}
}
