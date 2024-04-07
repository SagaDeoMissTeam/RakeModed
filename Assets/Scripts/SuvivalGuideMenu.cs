// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SuvivalGuideMenu
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuvivalGuideMenu : MonoBehaviour
{
	public List<Sprite> images = new List<Sprite>();

	public Image currentPage;

	public int currentPageID;

	public void NextPage()
	{
		currentPageID++;
		currentPageID = Mathf.Clamp(currentPageID, 0, images.Count - 1);
		currentPage.sprite = images[currentPageID];
	}

	public void PrevPage()
	{
		currentPageID--;
		currentPageID = Mathf.Clamp(currentPageID, 0, images.Count - 1);
		currentPage.sprite = images[currentPageID];
	}
}
