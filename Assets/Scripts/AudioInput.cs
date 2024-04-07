// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AudioInput
using UnityEngine;

public class AudioInput : MonoBehaviour
{
	public int qSamples = 4096;

	public float[] samples;

	public float inputVolume;

	public RectTransform volScale;

	private void Start()
	{
		samples = new float[qSamples];
	}

	private void Update()
	{
		float rMS = GetRMS(0);
		rMS += GetRMS(1);
		inputVolume = rMS;
		volScale.sizeDelta = new Vector2(20f, inputVolume * inputVolume * 200f);
	}

	public float GetRMS(int channel)
	{
		AudioListener.GetOutputData(samples, channel);
		float num = 0f;
		for (int i = 0; i < qSamples; i++)
		{
			num += samples[i] * samples[i];
		}
		return num / (float)qSamples;
	}
}
