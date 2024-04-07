// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SS
using System;
using UnityEngine;

public class SS : MonoBehaviour
{
	private Transform ThisTransform;

	public float Frequency = 1f;

	public float Amplitude = 1f;

	public Vector3 MoveAxisSpeed = Vector3.zero;

	public float Angle = 360f;

	private void Awake()
	{
		Angle *= (float)Math.PI / 180f;
		ThisTransform = GetComponent<Transform>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		float num = Amplitude * Mathf.Sin(Angle / Frequency * Time.deltaTime);
		float num2 = Amplitude * Mathf.Cos(Angle / Frequency * Time.deltaTime);
		ThisTransform.Translate(num * MoveAxisSpeed.x, num2 * MoveAxisSpeed.y, 0f);
	}
}
