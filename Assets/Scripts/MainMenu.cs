// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MainMenu
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject GameMenu;

	public GameObject newGame;

	public GameObject setting;

	public GameObject multiplayer;

	public GameObject credits;

	public int levelToload;

	public int savedLevel;

	public Animator effects;

	public GameObject loadLogo;

	public Text loadLevelText;

	public Button loadGameButton;

	public GameObject LoadingCanvas;

	public float mouseSens;

	public float audioVolume;

	public GameObject help;

	public void Start()
	{
		Time.timeScale = 1f;
		if (PlayerPrefs.GetFloat("Sound:") != 0f)
		{
			AudioListener.volume = PlayerPrefs.GetFloat("Sound:");
		}
		else
		{
			AudioListener.volume = 1f;
		}
		if (PlayerPrefs.GetFloat("Sound:") != 0f)
		{
			mouseSens = PlayerPrefs.GetFloat("Sens:");
		}
		else
		{
			mouseSens = 2f;
		}
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		savedLevel = PlayerPrefs.GetInt("Level:");
		if (savedLevel <= 1)
		{
			loadLevelText.color = new Color(1f, 1f, 1f, 0.3f);
			loadGameButton.interactable = false;
		}
	}

	private void OnApplicationQuit()
	{
	}

	public void ActiveHelpMenu()
	{
		help.SetActive(value: true);
	}

	public void ActiveSettings()
	{
		setting.SetActive(value: true);
	}

	public void ActiveGameMenu()
	{
		GameMenu.SetActive(value: true);
	}

	private IEnumerator startGame()
	{
		PlayerPrefs.SetFloat("Sound:", audioVolume);
		PlayerPrefs.SetFloat("Sens:", mouseSens);
		LoadingCanvas.SetActive(value: true);
		loadLogo.SetActive(value: true);
		effects.SetBool("Fade", value: true);
		newGame.SetActive(value: false);
		yield return new WaitForSeconds(3f);
		Application.LoadLevel(levelToload);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Back()
	{
		GameMenu.SetActive(value: false);
		help.SetActive(value: false);
		setting.SetActive(value: false);
		multiplayer.SetActive(value: false);
		credits.SetActive(value: false);
	}

	public void Loadgame()
	{
		levelToload = savedLevel;
		StartCoroutine(startGame());
	}

	public void StartNewGame()
	{
		PlayerPrefs.DeleteAll();
		levelToload = 1;
		StartCoroutine(startGame());
	}

	public void SetSoundVolume(float sound)
	{
		AudioListener.volume = sound;
		audioVolume = sound;
	}

	public void OpenCredits()
	{
		credits.SetActive(value: true);
	}

	public void SetMouseSens(float speed)
	{
		mouseSens = speed;
	}

	public void OpenMultiplayer()
	{
		multiplayer.SetActive(value: true);
	}

	public void Newgame()
	{
		newGame.SetActive(value: true);
	}
}
