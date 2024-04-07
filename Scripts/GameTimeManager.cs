// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// GameTimeManager
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
	public int day;

	public int month;

	public int year;

	public float second;

	public int fSecond;

	public float minute;

	public float hour;

	public static GameTimeManager instance;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		second += Time.deltaTime;
		fSecond = (int)second;
		if (second >= 60f)
		{
			second = 0f;
			minute += 1f;
			if (minute >= 60f)
			{
				minute = 0f;
				hour += 1f;
			}
		}
	}
}
