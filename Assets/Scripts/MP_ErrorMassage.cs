// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_ErrorMassage
using UnityEngine;
using UnityEngine.UI;

public class MP_ErrorMassage : MonoBehaviour
{
	public string errorMassage;

	[SerializeField]
	private Text text;

	[SerializeField]
	private GameObject errorForm;

	public void OpenErrorWindow(string massage)
	{
		errorForm.SetActive(value: true);
		errorMassage = massage;
		text.text = errorMassage;
	}

	public void CloseWindow()
	{
		errorForm.SetActive(value: false);
	}
}
