// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RakeHealthUI
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MP_RakeHealthUI : MonoBehaviour
{
	public MP_Enemy enemy;

	public GameObject panel;

	public GameObject bg;

	public Text text;

	public float health;

	private void Start()
	{
	}

	private void Update()
	{
		bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(panel.GetComponent<RectTransform>().anchoredPosition.x - 2f, panel.GetComponent<RectTransform>().anchoredPosition.y);
		bg.GetComponent<RectTransform>().sizeDelta = new Vector2(283f, 14f);
		panel.GetComponent<RectTransform>().sizeDelta = new Vector2(health / 16f, 10f);
	}

	public void OnDamageTaken()
	{
		StartCoroutine(hide());
	}

	private IEnumerator hide()
	{
		bg.SetActive(value: true);
		panel.SetActive(value: true);
		yield return new WaitForSeconds(5f);
		bg.SetActive(value: false);
		panel.SetActive(value: false);
	}
}
