// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DeveloperConsole
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
	public GUISkin guiSkin;

	public float ySizePercent = 0.5f;

	public float xSizePercent = 0.9f;

	public float textFieldHeight = 24f;

	public float lineHeight = 22f;

	public float transitionTime = 1f;

	public char[] disallowedChars = new char[1] { '`' };

	public Vector2 autocompleteBoxMaxSize = new Vector2(200f, 250f);

	public Color autocompleteSelectedColor = Color.green;

	public KeyCode toggleKeyCode = KeyCode.BackQuote;

	public KeyCode autocompleteKeyCode = KeyCode.Tab;

	public bool copyLogOutput = true;

	public bool showCursorWhenActive = true;

	public bool onlyAcceptAliases;

	public DeveloperConsoleAlias[] aliasList;

	private bool showing;

	private Vector2 scrollPosition = Vector2.zero;

	private Vector2[] autocompleteScrollPos = new Vector2[2]
	{
		Vector2.zero,
		Vector2.zero
	};

	private string consoleText = string.Empty;

	private string previousConsoleText = string.Empty;

	private List<string> previousTextEntries = new List<string>();

	private int previousEntriesIndex;

	private List<string> consoleOutput = new List<string>();

	private float displayAmount;

	private string[] autocomplete;

	private List<string> gameObjectNames = new List<string>();

	private float findObjectsFrequency = 5f;

	private List<string> foundGameObjectNames = new List<string>();

	private List<string> foundComponentNames = new List<string>();

	private List<string> foundMethodNames = new List<string>();

	private List<string> foundFieldNames = new List<string>();

	private List<string> foundPropertyNames = new List<string>();

	private int autocompleteIndex;

	private int autocompleteWindowIndex;

	private bool closeAutocomplete;

	private bool showingAutocomplete;

	private EventType previousEvent = EventType.Layout;

	private float autocompleteButtonHeight = 20f;

	private float autocompleteTitleHeight = 26f;

	private float autocompleteBoxBottomPadding = 4f;

	private float autocompleteScrollBarBuffer = 16f;

	private float autocompleteWindowPadding = 10f;

	private float timeBetweenAutocompleteSelectionChange = 0.1f;

	private float timeUntilAutocompleteSelectionChangeAllowed;

	private bool focusOnTextField;

	private bool cursorShowingBefore = true;

	private bool cursorLockedBefore = true;

	private Dictionary<string, string> dictionaryOfAliases = new Dictionary<string, string>();

	private IEnumerator Start()
	{
		StartCoroutine(FindAllGameObjects());
		for (int aliasI = 0; aliasI < aliasList.Length; aliasI++)
		{
			dictionaryOfAliases[aliasList[aliasI].aliasName] = aliasList[aliasI].aliasValue;
		}
		int framesPassed = 0;
		while (framesPassed < 10)
		{
			if (copyLogOutput)
			{
				Application.RegisterLogCallback(CopyLogOutout);
			}
			framesPassed++;
			yield return 0;
		}
	}

	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	private void Update()
	{
		if (transitionTime < 0f)
		{
			transitionTime = 0f;
		}
		if (showing)
		{
			if (displayAmount < 1f)
			{
				displayAmount = Mathf.Clamp01(displayAmount + Time.deltaTime / transitionTime);
			}
		}
		else if (displayAmount > 0f)
		{
			displayAmount = Mathf.Clamp01(displayAmount - Time.deltaTime / transitionTime);
		}
		if (!showing && displayAmount == 0f && Input.GetKeyUp(toggleKeyCode))
		{
			cursorShowingBefore = Cursor.visible;
			cursorLockedBefore = Screen.lockCursor;
			showing = !showing;
			if (showing)
			{
				focusOnTextField = true;
			}
			if (showCursorWhenActive)
			{
				Cursor.visible = true;
				Screen.lockCursor = false;
			}
		}
		if (showing && showCursorWhenActive)
		{
			Cursor.visible = true;
			Screen.lockCursor = false;
		}
		if (timeUntilAutocompleteSelectionChangeAllowed > 0f)
		{
			timeUntilAutocompleteSelectionChangeAllowed -= 0.01f;
		}
		if (previousConsoleText != consoleText && !closeAutocomplete)
		{
			showingAutocomplete = false;
			foundGameObjectNames.Clear();
			foundComponentNames.Clear();
			foundMethodNames.Clear();
			foundFieldNames.Clear();
			foundPropertyNames.Clear();
			string text = consoleText;
			if (text.ToLower().StartsWith("print "))
			{
				text = text.Substring(6, text.Length - 6);
			}
			string[] splitDots = text.Split('.');
			GameObject gameObject = null;
			if (splitDots.Length == 1)
			{
				if (!onlyAcceptAliases)
				{
					foundGameObjectNames.AddRange(gameObjectNames.FindAll((string goName) => goName.ToLower().StartsWith(splitDots[0].ToLower())));
				}
				foreach (string key in dictionaryOfAliases.Keys)
				{
					if (key.ToLower().StartsWith(splitDots[0].ToLower()))
					{
						foundGameObjectNames.Add(key);
					}
				}
			}
			if (splitDots.Length == 2 && !onlyAcceptAliases)
			{
				gameObject = GameObject.Find(splitDots[0]);
				if (gameObject != null)
				{
					Component[] components = gameObject.GetComponents<Component>();
					for (int i = 0; i < components.Length; i++)
					{
						string text2 = components[i].GetType().ToString();
						string[] array = text2.Split('.');
						text2 = array[array.Length - 1];
						if (text2.ToLower().StartsWith(splitDots[1].ToLower()))
						{
							foundComponentNames.Add(text2);
						}
					}
				}
			}
			if (splitDots.Length == 3 && !onlyAcceptAliases)
			{
				gameObject = GameObject.Find(splitDots[0]);
				if (gameObject != null)
				{
					Component component = gameObject.GetComponent(splitDots[1]);
					if (component != null)
					{
						MethodInfo[] methods = component.GetType().GetMethods();
						MethodInfo[] array2 = methods;
						foreach (MethodInfo methodInfo in array2)
						{
							if (methodInfo.Name.ToLower().StartsWith(splitDots[2].ToLower()) && !foundMethodNames.Contains(methodInfo.Name))
							{
								foundMethodNames.Add(methodInfo.Name);
							}
						}
						FieldInfo[] fields = component.GetType().GetFields();
						FieldInfo[] array3 = fields;
						foreach (FieldInfo fieldInfo in array3)
						{
							if (fieldInfo.Name.ToLower().StartsWith(splitDots[2].ToLower()) && !foundFieldNames.Contains(fieldInfo.Name))
							{
								foundFieldNames.Add(fieldInfo.Name);
							}
						}
						PropertyInfo[] properties = component.GetType().GetProperties();
						PropertyInfo[] array4 = properties;
						foreach (PropertyInfo propertyInfo in array4)
						{
							if (propertyInfo.Name.ToLower().StartsWith(splitDots[2].ToLower()) && !foundPropertyNames.Contains(propertyInfo.Name))
							{
								foundPropertyNames.Add(propertyInfo.Name);
							}
						}
					}
				}
			}
			if (foundGameObjectNames.Count > 0 || foundComponentNames.Count > 0 || foundMethodNames.Count > 0)
			{
				showingAutocomplete = true;
			}
		}
		previousConsoleText = consoleText;
	}

	public void CopyLogOutout(string condition, string stackTrace, LogType type)
	{
		PrintToConsole("Log Output: " + condition);
	}

	public void PrintToConsole(string text)
	{
		consoleOutput.Add(text);
		if (scrollPosition.y >= (float)consoleOutput.Count * lineHeight - (float)Camera.main.pixelHeight * ySizePercent - textFieldHeight)
		{
			scrollPosition.y = (float)consoleOutput.Count * lineHeight;
		}
	}

	private IEnumerator FindAllGameObjects()
	{
		while (Application.isPlaying)
		{
			gameObjectNames.Clear();
			GameObject[] gos = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
			GameObject[] array = gos;
			foreach (GameObject go in array)
			{
				gameObjectNames.Add(go.name);
			}
			yield return new WaitForSeconds(findObjectsFrequency);
		}
		yield return null;
	}

	private void OnGUI()
	{
		if (guiSkin != null)
		{
			GUI.skin = guiSkin;
		}
		bool flag = false;
		EventType type = Event.current.type;
		if (showingAutocomplete && previousEvent != Event.current.type && (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp) && (Event.current.keyCode == KeyCode.DownArrow || Event.current.keyCode == KeyCode.UpArrow))
		{
			Event.current.Use();
			flag = true;
		}
		previousEvent = Event.current.type;
		if (displayAmount == 0f && !showing)
		{
			return;
		}
		Camera main = Camera.main;
		float num = (float)main.pixelHeight * ySizePercent;
		float num2 = (float)main.pixelWidth * xSizePercent;
		float left = ((float)main.pixelWidth - num2) * 0.5f;
		Rect position = new Rect(left, Mathf.Lerp(0f - num, 0f, displayAmount), num2, num);
		Rect position2 = new Rect(0f, 0f, num2, num - textFieldHeight);
		Rect viewRect = new Rect(0f, 0f, num2 - 16f, Mathf.Max((float)consoleOutput.Count * lineHeight, num - textFieldHeight));
		Rect position3 = new Rect(5f, position.height - textFieldHeight, num2 - 10f, textFieldHeight);
		GUI.BeginGroup(position);
		GUI.SetNextControlName("ConsoleBackground");
		GUI.Box(new Rect(0f, 0f, position.width, position.height), string.Empty);
		scrollPosition = GUI.BeginScrollView(position2, scrollPosition, viewRect, alwaysShowHorizontal: false, alwaysShowVertical: true);
		for (int i = 0; i < consoleOutput.Count; i++)
		{
			string text = consoleOutput[i];
			GUI.Label(new Rect(8f, (float)i * lineHeight, position2.width, lineHeight), text);
		}
		GUI.EndScrollView();
		string text2 = consoleText;
		GUI.SetNextControlName("ConsoleTextField");
		consoleText = GUI.TextField(position3, consoleText);
		consoleText = consoleText.Trim(disallowedChars);
		if (text2 != consoleText)
		{
			closeAutocomplete = false;
			if (previousTextEntries.Count > 0)
			{
				previousEntriesIndex = previousTextEntries.Count;
			}
			else
			{
				previousEntriesIndex = 0;
			}
		}
		if (focusOnTextField)
		{
			GUI.FocusControl("ConsoleBackground");
			GUI.FocusControl("ConsoleTextField");
			((TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl))?.MoveCursorToPosition(new Vector2(5555f, 5555f));
		}
		focusOnTextField = false;
		GUI.EndGroup();
		if (!closeAutocomplete)
		{
			bool flag2 = foundMethodNames.Count > 0;
			bool flag3 = foundFieldNames.Count > 0;
			bool flag4 = foundPropertyNames.Count > 0;
			if (flag2 || flag3 || flag4)
			{
				int num3 = 0;
				if (flag2)
				{
					num3++;
				}
				if (flag3 || flag4)
				{
					num3++;
				}
				autocompleteWindowIndex = Mathf.Clamp(autocompleteWindowIndex, 0, num3 - 1);
				int selectedIndex = -1;
				float num4 = Input.compositionCursorPos.x + autocompleteBoxMaxSize.x * 0.5f;
				if (flag2)
				{
					if (autocompleteWindowIndex == 0)
					{
						autocompleteIndex = Mathf.Clamp(autocompleteIndex, 0, foundMethodNames.Count - 1);
						selectedIndex = autocompleteIndex;
					}
					DrawAutocompleteWindow("Methods", foundMethodNames.ToArray(), new Vector2(num4, position.yMax), selectedIndex, 0);
					num4 += autocompleteBoxMaxSize.x + 10f;
				}
				if (flag3 || flag4)
				{
					selectedIndex = -1;
					string[] array = new string[foundFieldNames.Count + foundPropertyNames.Count];
					Array.Copy(foundFieldNames.ToArray(), 0, array, 0, foundFieldNames.Count);
					Array.Copy(foundPropertyNames.ToArray(), 0, array, foundFieldNames.Count, foundPropertyNames.Count);
					if (autocompleteWindowIndex == 0 + Convert.ToInt32(flag2))
					{
						autocompleteIndex = Mathf.Clamp(autocompleteIndex, 0, array.Length - 1);
						selectedIndex = autocompleteIndex;
					}
					DrawAutocompleteWindow("Fields & Properties", array, new Vector2(num4 + autocompleteWindowPadding, position.yMax), selectedIndex, Convert.ToInt32(flag2));
				}
			}
			else if (foundComponentNames.Count > 0)
			{
				autocompleteIndex = Mathf.Clamp(autocompleteIndex, 0, foundComponentNames.Count - 1);
				autocompleteWindowIndex = 0;
				DrawAutocompleteWindow("Components", foundComponentNames.ToArray(), new Vector2(Input.compositionCursorPos.x + autocompleteBoxMaxSize.x * 0.5f, position.yMax), autocompleteIndex);
			}
			else if (foundGameObjectNames.Count > 0)
			{
				autocompleteIndex = Mathf.Clamp(autocompleteIndex, 0, foundGameObjectNames.Count - 1);
				autocompleteWindowIndex = 0;
				DrawAutocompleteWindow("GameObjects", foundGameObjectNames.ToArray(), new Vector2(Input.compositionCursorPos.x + autocompleteBoxMaxSize.x * 0.5f, position.yMax), autocompleteIndex);
			}
		}
		if (Event.current.keyCode == KeyCode.LeftArrow && Event.current.type == EventType.Used && showingAutocomplete && !closeAutocomplete && autocompleteWindowIndex > 0)
		{
			autocompleteWindowIndex--;
			Event.current.Use();
			ScrollToDisplaySelected();
		}
		if (Event.current.keyCode == KeyCode.RightArrow && Event.current.type == EventType.Used)
		{
			bool value = foundMethodNames.Count > 0;
			bool flag5 = foundFieldNames.Count > 0;
			bool flag6 = foundPropertyNames.Count > 0;
			int num5 = Convert.ToInt32(value) + Convert.ToInt32(flag5 || flag6);
			if (showingAutocomplete && !closeAutocomplete && autocompleteWindowIndex < num5 - 1)
			{
				autocompleteWindowIndex++;
				Event.current.Use();
				ScrollToDisplaySelected();
			}
		}
		if (Event.current.isKey || flag)
		{
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == autocompleteKeyCode)
			{
				if (!closeAutocomplete)
				{
					ApplyAutocompleteText();
				}
				closeAutocomplete = false;
			}
			if (Event.current.keyCode == KeyCode.DownArrow)
			{
				if (showingAutocomplete && !closeAutocomplete)
				{
					if (timeUntilAutocompleteSelectionChangeAllowed <= 0f)
					{
						autocompleteIndex++;
						timeUntilAutocompleteSelectionChangeAllowed = timeBetweenAutocompleteSelectionChange;
						ScrollToDisplaySelected();
					}
				}
				else if (type == EventType.KeyUp)
				{
					if (previousEntriesIndex < previousTextEntries.Count)
					{
						previousEntriesIndex++;
					}
					if (previousTextEntries.Count > previousEntriesIndex)
					{
						consoleText = previousTextEntries[previousEntriesIndex];
					}
					else
					{
						consoleText = string.Empty;
					}
				}
			}
			if (Event.current.keyCode == KeyCode.UpArrow)
			{
				if (showingAutocomplete && !closeAutocomplete)
				{
					if (timeUntilAutocompleteSelectionChangeAllowed <= 0f)
					{
						autocompleteIndex--;
						timeUntilAutocompleteSelectionChangeAllowed = timeBetweenAutocompleteSelectionChange;
						ScrollToDisplaySelected();
					}
				}
				else if (type == EventType.KeyUp)
				{
					if (previousEntriesIndex > 0)
					{
						previousEntriesIndex--;
					}
					if (previousTextEntries.Count > 0 && previousTextEntries.Count >= previousEntriesIndex - 1)
					{
						consoleText = previousTextEntries[previousEntriesIndex];
					}
				}
			}
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Escape)
			{
				closeAutocomplete = true;
			}
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == toggleKeyCode)
			{
				if (showing && showCursorWhenActive)
				{
					Cursor.visible = cursorShowingBefore;
					Screen.lockCursor = cursorLockedBefore;
				}
				showing = !showing;
				if (showing)
				{
					focusOnTextField = true;
				}
			}
			if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
			{
				closeAutocomplete = true;
				bool printResult = false;
				string text3 = consoleText;
				string[] array2 = new string[dictionaryOfAliases.Count];
				dictionaryOfAliases.Keys.CopyTo(array2, 0);
				string[] array3 = new string[dictionaryOfAliases.Count];
				dictionaryOfAliases.Values.CopyTo(array3, 0);
				for (int j = 0; j < dictionaryOfAliases.Count; j++)
				{
					if (text3.StartsWith(array2[j]))
					{
						text3 = text3.Replace(array2[j], array3[j]);
					}
				}
				if (text3.ToLower().StartsWith("print "))
				{
					text3 = text3.Substring(6, text3.Length - 6);
					printResult = true;
				}
				string[] array4 = text3.Split(' ');
				if (array4.Length > 0)
				{
					string[] array5 = text3.Split('.');
					if (array5.Length >= 3)
					{
						GameObject gameObject = GameObject.Find(array5[0]);
						if (gameObject != null)
						{
							int num6 = array5[0].Length + 1;
							string[] array6 = text3.Substring(num6, text3.Length - num6).Split(' ');
							array5 = array6[0].Split('.');
							if (array5.Length >= 2)
							{
								string text4 = string.Empty;
								if (array6[0].Length > 0 && text3.Length - (num6 + array6[0].Length + 1) >= 0)
								{
									text4 = text3.Substring(num6 + array6[0].Length + 1, text3.Length - (num6 + array6[0].Length + 1));
								}
								List<string> list = new List<string>();
								bool flag7 = false;
								string text5 = string.Empty;
								for (int k = 0; k < text4.Length; k++)
								{
									if (text4[k] == ' ')
									{
										if (!flag7)
										{
											if (text5 != string.Empty)
											{
												list.Add(text5);
												text5 = string.Empty;
											}
											continue;
										}
									}
									else if (text4[k] == '"')
									{
										if (flag7)
										{
											flag7 = false;
											if (text5 != string.Empty)
											{
												list.Add(text5);
												text5 = string.Empty;
											}
										}
										else
										{
											flag7 = true;
										}
										continue;
									}
									text5 += text4[k];
									if (k == text4.Length - 1)
									{
										list.Add(text5);
									}
								}
								string[] array7 = list.ToArray();
								Component component = gameObject.GetComponent(array5[0]);
								if (component != null)
								{
									bool flag8 = false;
									MethodInfo[] methods = component.GetType().GetMethods();
									MethodInfo[] array8 = methods;
									foreach (MethodInfo methodInfo in array8)
									{
										if (methodInfo.Name.ToLower() == array5[1].ToLower())
										{
											RunMethod(gameObject, component, array5[1], array7, printResult);
											flag8 = true;
											break;
										}
									}
									if (!flag8)
									{
										FieldInfo[] fields = component.GetType().GetFields();
										FieldInfo[] array9 = fields;
										foreach (FieldInfo fieldInfo in array9)
										{
											if (fieldInfo.Name.ToLower() == array5[1].ToLower())
											{
												if (array7.Length == 0)
												{
													GetField(gameObject, component, array5[1]);
												}
												else
												{
													SetField(gameObject, component, array5[1], array7[0]);
												}
												flag8 = true;
												break;
											}
										}
									}
									if (!flag8)
									{
										PropertyInfo[] properties = component.GetType().GetProperties();
										PropertyInfo[] array10 = properties;
										foreach (PropertyInfo propertyInfo in array10)
										{
											if (propertyInfo.Name.ToLower() == array5[1].ToLower())
											{
												if (array7.Length == 0)
												{
													GetProperty(gameObject, component, array5[1]);
												}
												else
												{
													SetProperty(gameObject, component, array5[1], array7[0]);
												}
												flag8 = true;
												break;
											}
										}
									}
								}
							}
						}
					}
				}
				previousTextEntries.Add(consoleText);
				consoleText = string.Empty;
				previousEntriesIndex = previousTextEntries.Count;
			}
		}
		GUI.skin = null;
	}

	private void RunMethod(GameObject gameObject, Component component, string methodName, string[] arguments, bool printResult)
	{
		Type type = component.GetType();
		MethodInfo[] methods = type.GetMethods();
		object[] parameters = new object[0];
		int num = -1;
		int num2 = 0;
		int num3 = -1;
		bool flag = false;
		object obj = null;
		for (int i = 0; i < methods.Length; i++)
		{
			MethodInfo methodInfo = methods[i];
			int num4 = 0;
			if (methodInfo == null || methodInfo.Name != methodName)
			{
				continue;
			}
			bool flag2 = true;
			ParameterInfo[] parameters2 = methodInfo.GetParameters();
			object[] array = new object[parameters2.Length];
			for (int j = 0; j < parameters2.Length; j++)
			{
				if (arguments.Length > j)
				{
					if (parameters2[j].ParameterType == typeof(int))
					{
						int result = 0;
						flag2 = int.TryParse(arguments[j], out result);
						if (flag2)
						{
							array[j] = result;
							num4++;
						}
					}
					else if (parameters2[j].ParameterType == typeof(float))
					{
						float result2 = 0f;
						flag2 = float.TryParse(arguments[j], out result2);
						if (flag2)
						{
							array[j] = result2;
							num4++;
						}
					}
					else if (parameters2[j].ParameterType == typeof(double))
					{
						double result3 = 0.0;
						flag2 = double.TryParse(arguments[j], out result3);
						if (flag2)
						{
							array[j] = result3;
							num4++;
						}
					}
					else if (parameters2[j].ParameterType == typeof(bool))
					{
						if (arguments[j].ToUpper() == "FALSE")
						{
							array[j] = false;
							num4++;
						}
						else if (arguments[j].ToUpper() == "TRUE")
						{
							array[j] = true;
							num4++;
						}
						else
						{
							int result4 = 0;
							flag2 = int.TryParse(arguments[j], out result4);
							if (flag2)
							{
								array[j] = ((result4 != 0) ? true : false);
								num4++;
							}
						}
					}
					else if (parameters2[j].ParameterType == typeof(string))
					{
						array[j] = arguments[j];
						num4++;
					}
					else if (parameters2[j].ParameterType == typeof(Vector2))
					{
						string[] array2 = arguments[j].Split(',');
						if (array2.Length == 2)
						{
							float result5 = 0f;
							bool flag3 = false;
							flag3 = float.TryParse(array2[0], out result5);
							float result6 = 0f;
							bool flag4 = false;
							flag4 = float.TryParse(array2[2], out result6);
							if (flag3 && flag4)
							{
								array[j] = new Vector2(result5, result6);
								num4++;
							}
							else
							{
								flag2 = false;
							}
						}
					}
					else if (parameters2[j].ParameterType == typeof(Vector3))
					{
						string[] array3 = arguments[j].Split(',');
						if (array3.Length == 3)
						{
							float result7 = 0f;
							bool flag5 = false;
							flag5 = float.TryParse(array3[0], out result7);
							float result8 = 0f;
							bool flag6 = false;
							flag6 = float.TryParse(array3[1], out result8);
							float result9 = 0f;
							bool flag7 = false;
							flag7 = float.TryParse(array3[2], out result9);
							if (flag5 && flag6 && flag7)
							{
								array[j] = new Vector3(result7, result8, result9);
								num4++;
							}
							else
							{
								flag2 = false;
							}
						}
					}
					else if (parameters2[j].ParameterType == typeof(Quaternion))
					{
						string[] array4 = arguments[j].Split(',');
						if (array4.Length == 4)
						{
							float result10 = 0f;
							bool flag8 = false;
							flag8 = float.TryParse(array4[0], out result10);
							float result11 = 0f;
							bool flag9 = false;
							flag9 = float.TryParse(array4[1], out result11);
							float result12 = 0f;
							bool flag10 = false;
							flag10 = float.TryParse(array4[2], out result12);
							float result13 = 0f;
							bool flag11 = false;
							flag11 = float.TryParse(array4[2], out result13);
							if (flag8 && flag9 && flag10 && flag11)
							{
								array[j] = new Quaternion(result10, result11, result12, result13);
								num4++;
							}
							else
							{
								flag2 = false;
							}
						}
					}
					else if (parameters2[j].ParameterType == typeof(Color))
					{
						string[] array5 = arguments[j].Split(',');
						if (array5.Length == 4)
						{
							float result14 = 0f;
							bool flag12 = false;
							flag12 = float.TryParse(array5[0], out result14);
							float result15 = 0f;
							bool flag13 = false;
							flag13 = float.TryParse(array5[1], out result15);
							float result16 = 0f;
							bool flag14 = false;
							flag14 = float.TryParse(array5[2], out result16);
							float result17 = 0f;
							bool flag15 = false;
							flag15 = float.TryParse(array5[2], out result17);
							if (flag12 && flag13 && flag14 && flag15)
							{
								array[j] = new Color(result14, result15, result16, result17);
								num4++;
							}
							else
							{
								flag2 = false;
							}
						}
					}
				}
				else if (parameters2[j].DefaultValue != null)
				{
					flag2 = false;
				}
				else
				{
					array[j] = parameters2[j].DefaultValue;
				}
				if (!flag2)
				{
					break;
				}
			}
			if (num4 > num || (num4 == num && num2 > parameters2.Length))
			{
				num2 = parameters2.Length;
				num = num4;
				num3 = i;
				parameters = array;
				flag = ((methodInfo.ReturnType != typeof(void)) ? true : false);
			}
		}
		if (num3 < 0)
		{
			return;
		}
		if (flag)
		{
			obj = methods[num3].Invoke(component, parameters);
			if (printResult)
			{
				PrintToConsole(obj.ToString());
			}
		}
		else
		{
			methods[num3].Invoke(component, parameters);
		}
	}

	private void GetField(GameObject gameObject, Component component, string fieldName)
	{
		Type type = component.GetType();
		FieldInfo field = type.GetField(fieldName);
		if (field != null)
		{
			PrintToConsole(field.GetValue(component).ToString());
		}
	}

	private void SetField(GameObject gameObject, Component component, string fieldName, string argument)
	{
		Type type = component.GetType();
		FieldInfo field = type.GetField(fieldName);
		if (field != null)
		{
			bool flag = true;
			object returnObject = null;
			if (ParseStringToObject(field.FieldType, argument, ref returnObject))
			{
				field.SetValue(component, returnObject);
			}
			else
			{
				PrintToConsole("Failed to set variable of type " + field.FieldType.ToString());
			}
		}
	}

	private void GetProperty(GameObject gameObject, Component component, string propertyName)
	{
		Type type = component.GetType();
		PropertyInfo property = type.GetProperty(propertyName);
		if (property != null)
		{
			PrintToConsole(property.GetValue(component, null).ToString());
		}
	}

	private void SetProperty(GameObject gameObject, Component component, string propertyName, string argument)
	{
		Type type = component.GetType();
		PropertyInfo property = type.GetProperty(propertyName);
		if (property != null)
		{
			bool flag = true;
			object returnObject = null;
			if (ParseStringToObject(property.PropertyType, argument, ref returnObject))
			{
				property.SetValue(component, returnObject, null);
			}
			else
			{
				PrintToConsole("Failed to set variable of type " + property.PropertyType.ToString());
			}
		}
	}

	private bool ParseStringToObject(Type objectType, string argument, ref object returnObject)
	{
		bool flag = false;
		if (objectType == typeof(int))
		{
			int result = 0;
			if (int.TryParse(argument, out result))
			{
				returnObject = result;
				return true;
			}
			return false;
		}
		if (objectType == typeof(float))
		{
			float result2 = 0f;
			if (float.TryParse(argument, out result2))
			{
				returnObject = result2;
				return true;
			}
			return false;
		}
		if (objectType == typeof(double))
		{
			double result3 = 0.0;
			if (double.TryParse(argument, out result3))
			{
				returnObject = result3;
				return true;
			}
			return false;
		}
		if (objectType == typeof(bool))
		{
			if (argument.ToUpper() == "FALSE")
			{
				returnObject = false;
				return true;
			}
			if (argument.ToUpper() == "TRUE")
			{
				returnObject = true;
				return true;
			}
			int result4 = 0;
			if (int.TryParse(argument, out result4))
			{
				returnObject = ((result4 != 0) ? true : false);
				return true;
			}
			return false;
		}
		if (objectType == typeof(string))
		{
			returnObject = argument;
			return true;
		}
		if (objectType == typeof(Vector2))
		{
			string[] array = argument.Split(',');
			if (array.Length == 2)
			{
				float result5 = 0f;
				bool flag2 = false;
				flag2 = float.TryParse(array[0], out result5);
				float result6 = 0f;
				bool flag3 = false;
				flag3 = float.TryParse(array[2], out result6);
				if (flag2 && flag3)
				{
					returnObject = new Vector2(result5, result6);
					return true;
				}
				return false;
			}
		}
		else if (objectType == typeof(Vector3))
		{
			string[] array2 = argument.Split(',');
			if (array2.Length == 3)
			{
				float result7 = 0f;
				bool flag4 = false;
				flag4 = float.TryParse(array2[0], out result7);
				float result8 = 0f;
				bool flag5 = false;
				flag5 = float.TryParse(array2[1], out result8);
				float result9 = 0f;
				bool flag6 = false;
				flag6 = float.TryParse(array2[2], out result9);
				if (flag4 && flag5 && flag6)
				{
					returnObject = new Vector3(result7, result8, result9);
					return true;
				}
				return false;
			}
		}
		else if (objectType == typeof(Quaternion))
		{
			string[] array3 = argument.Split(',');
			if (array3.Length == 4)
			{
				float result10 = 0f;
				bool flag7 = false;
				flag7 = float.TryParse(array3[0], out result10);
				float result11 = 0f;
				bool flag8 = false;
				flag8 = float.TryParse(array3[1], out result11);
				float result12 = 0f;
				bool flag9 = false;
				flag9 = float.TryParse(array3[2], out result12);
				float result13 = 0f;
				bool flag10 = false;
				flag10 = float.TryParse(array3[3], out result13);
				if (flag7 && flag8 && flag9 && flag10)
				{
					returnObject = new Quaternion(result10, result11, result12, result13);
					return true;
				}
				return false;
			}
		}
		else if (objectType == typeof(Rect))
		{
			string[] array4 = argument.Split(',');
			if (array4.Length == 4)
			{
				float result14 = 0f;
				bool flag11 = false;
				flag11 = float.TryParse(array4[0], out result14);
				float result15 = 0f;
				bool flag12 = false;
				flag12 = float.TryParse(array4[1], out result15);
				float result16 = 0f;
				bool flag13 = false;
				flag13 = float.TryParse(array4[2], out result16);
				float result17 = 0f;
				bool flag14 = false;
				flag14 = float.TryParse(array4[3], out result17);
				if (flag11 && flag12 && flag13 && flag14)
				{
					returnObject = new Rect(result14, result15, result16, result17);
					return true;
				}
				return false;
			}
		}
		else if (objectType == typeof(Color))
		{
			string[] array5 = argument.Split(',');
			if (array5.Length == 4)
			{
				float result18 = 0f;
				bool flag15 = false;
				flag15 = float.TryParse(array5[0], out result18);
				float result19 = 0f;
				bool flag16 = false;
				flag16 = float.TryParse(array5[1], out result19);
				float result20 = 0f;
				bool flag17 = false;
				flag17 = float.TryParse(array5[2], out result20);
				float result21 = 0f;
				bool flag18 = false;
				flag18 = float.TryParse(array5[2], out result21);
				if (flag15 && flag16 && flag17 && flag18)
				{
					returnObject = new Color(result18, result19, result20, result21);
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private void DrawAutocompleteWindow(string title, string[] names, Vector2 offset, int selectedIndex)
	{
		DrawAutocompleteWindow(title, names, offset, selectedIndex, 0);
	}

	private void DrawAutocompleteWindow(string title, string[] names, Vector2 offset, int selectedIndex, int windowIndex)
	{
		ref Vector2 reference = ref autocompleteScrollPos[windowIndex];
		reference = GUI.BeginScrollView(new Rect(offset.x + autocompleteScrollBarBuffer, offset.y, autocompleteBoxMaxSize.x + autocompleteScrollBarBuffer, autocompleteBoxMaxSize.y), autocompleteScrollPos[windowIndex], new Rect(0f, 0f, autocompleteBoxMaxSize.x + autocompleteScrollBarBuffer, (float)names.Length * autocompleteButtonHeight), alwaysShowHorizontal: false, alwaysShowVertical: false);
		GUI.Box(new Rect(0f, 0f, autocompleteBoxMaxSize.x, autocompleteTitleHeight + autocompleteBoxBottomPadding + (float)names.Length * autocompleteButtonHeight), string.Empty);
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
		gUIStyle.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(0f, 0f, autocompleteBoxMaxSize.x, autocompleteTitleHeight), title, gUIStyle);
		for (int i = 0; i < names.Length; i++)
		{
			Color color = GUI.color;
			if (selectedIndex == i)
			{
				GUI.color = autocompleteSelectedColor;
			}
			if (GUI.Button(new Rect(0f, autocompleteTitleHeight + (float)i * autocompleteButtonHeight, autocompleteBoxMaxSize.x, autocompleteButtonHeight), names[i]))
			{
				autocompleteWindowIndex = windowIndex;
				autocompleteIndex = i;
				ApplyAutocompleteText();
			}
			if (selectedIndex == i)
			{
				GUI.color = color;
			}
		}
		GUI.EndScrollView();
	}

	private void ScrollToDisplaySelected()
	{
		float num = autocompleteBoxMaxSize.y - autocompleteTitleHeight;
		float num2 = autocompleteTitleHeight + autocompleteButtonHeight * (float)autocompleteIndex;
		if (autocompleteScrollPos[autocompleteWindowIndex].y > num2)
		{
			autocompleteScrollPos[autocompleteWindowIndex].y = num2;
		}
		else if (autocompleteScrollPos[autocompleteWindowIndex].y < num2 + autocompleteButtonHeight - num)
		{
			autocompleteScrollPos[autocompleteWindowIndex].y = num2 - num + autocompleteButtonHeight;
		}
	}

	private void ApplyAutocompleteText()
	{
		int num = 0;
		if (consoleText.ToLower().StartsWith("print "))
		{
			num += 6;
		}
		string[] array = consoleText.Split('.');
		bool flag = foundMethodNames.Count > 0;
		bool flag2 = foundFieldNames.Count > 0;
		bool flag3 = foundPropertyNames.Count > 0;
		if (flag || flag2 || flag3)
		{
			int num2 = 0;
			if (flag)
			{
				num2++;
			}
			if (flag2 || flag3)
			{
				num2++;
			}
			string text = consoleText.Substring(num, consoleText.Length - num);
			consoleText = consoleText.Substring(0, num) + text.Substring(0, text.LastIndexOf('.')) + ".";
			if (autocompleteWindowIndex == 0 && flag)
			{
				consoleText += foundMethodNames[autocompleteIndex];
			}
			else if (autocompleteIndex < foundFieldNames.Count)
			{
				consoleText += foundFieldNames[autocompleteIndex];
			}
			else
			{
				consoleText += foundPropertyNames[autocompleteIndex - foundFieldNames.Count];
			}
			focusOnTextField = true;
		}
		else if (foundComponentNames.Count > 0)
		{
			string text2 = consoleText.Substring(num, consoleText.Length - num);
			consoleText = consoleText.Substring(0, num) + text2.Substring(0, text2.LastIndexOf('.')) + ".";
			consoleText = consoleText + foundComponentNames[autocompleteIndex] + ".";
			focusOnTextField = true;
		}
		else if (foundGameObjectNames.Count > 0)
		{
			string text3 = consoleText.Substring(num, consoleText.Length - num);
			consoleText = consoleText.Substring(0, num) + text3.Substring(0, consoleText.LastIndexOf(array[array.Length - 1]));
			consoleText = consoleText + foundGameObjectNames[autocompleteIndex] + ".";
			focusOnTextField = true;
		}
	}
}
