// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Notebook_obj
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notebook_obj : DynamicObject
{
	public GameObject gui;

	public List<Sprite> notes = new List<Sprite>();

	public int currentNoteID;

	public Image currentNote;

	public void NextNote()
	{
		currentNoteID++;
		currentNoteID = Mathf.Clamp(currentNoteID, 0, notes.Count - 1);
		currentNote.sprite = notes[currentNoteID];
	}

	public void PrevNote()
	{
		currentNoteID--;
		currentNoteID = Mathf.Clamp(currentNoteID, 0, notes.Count - 1);
		currentNote.sprite = notes[currentNoteID];
	}

	public void Exit()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		gui.SetActive(value: false);
		FPSController.instance.SetPlayerDisableState("FULL");
	}

	public override void Action()
	{
		gui.SetActive(value: true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		WeaponManager.instance.HideCurrentWeapon();
		FPSController.instance.SetPlayerDisableState("SCREEMER");
	}
}
