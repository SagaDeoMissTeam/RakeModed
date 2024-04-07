// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DrawRakeHealth
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DrawRakeHealth : MonoBehaviour
{
	public Enemy_AI enemy;

	public GameObject panel;

	public GameObject bg;

	public Text text;

	private void Start()
	{
		NotificationsManager.Instance.AddListener(this, "OnDamageTaken");
		enemy = GameObject.FindGameObjectWithTag("Rake").GetComponent<Enemy_AI>();
	}

	private void Update()
	{
		bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(panel.GetComponent<RectTransform>().anchoredPosition.x - 2f, panel.GetComponent<RectTransform>().anchoredPosition.y);
		bg.GetComponent<RectTransform>().sizeDelta = new Vector2(283f, 14f);
		panel.GetComponent<RectTransform>().sizeDelta = new Vector2(enemy.Health / 16f, 10f);
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
