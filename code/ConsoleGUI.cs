// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ConsoleGUI
using Homans.Containers;
using UnityEngine;

internal class ConsoleGUI : MonoBehaviour
{
	private int historyScrollValue;

	private int commandIndex;

	private string command = string.Empty;

	private bool returnPressed;

	public GUISkin skin;

	public int linesVisible = 17;

	private bool isOpen;

	private string partialCommand = string.Empty;

	private bool moveCursorToEnd;

	public bool showHierarchy = true;

	private string[] displayObjects;

	private string[] displayComponents;

	private Vector2 hierarchyScrollValue;

	private Vector2 componentScrollValue;

	private int commandLastPos;

	private int commandLastSelectPos;

	private string[] displayMethods;

	private Vector2 methodScrollValue;

	private bool wasCursorVisible;

	private int hierarchyWidth = 150;

	private void Start()
	{
		displayObjects = Console.Instance.GetGameobjectsAtPath("/");
		displayComponents = Console.Instance.GetComponentsOfGameobject("/");
		displayMethods = Console.Instance.GetMethodsOfComponent("/");
		float num = Screen.height / 2;
		num -= (float)(skin.box.padding.top + skin.box.padding.bottom);
		num -= (float)(skin.box.margin.top + skin.box.margin.bottom);
		num -= skin.textField.CalcHeight(new GUIContent(string.Empty), 10f);
		linesVisible = (int)(num / skin.label.CalcHeight(new GUIContent(string.Empty), 10f)) - 2;
		float num2 = Screen.width - 10;
		num2 -= (float)hierarchyWidth;
		num2 -= skin.verticalScrollbar.CalcSize(new GUIContent(string.Empty)).x;
		Console.Instance.maxLineWidth = (int)(num2 / skin.label.CalcSize(new GUIContent("A")).x);
	}

	private void OnGUI()
	{
		GUI.skin = skin;
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
		{
			returnPressed = true;
		}
		else
		{
			returnPressed = false;
		}
		bool flag = false;
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
		{
			flag = true;
			Event.current.Use();
		}
		bool flag2 = false;
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
		{
			flag2 = true;
			Event.current.Use();
		}
		bool flag3 = false;
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			flag3 = true;
			Event.current.Use();
		}
		if (isOpen)
		{
			GUI.depth = -100;
			GUILayout.BeginArea(new Rect(5f, 5f, Screen.width - 10, Screen.height / 2), (GUIStyle)"box");
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			CircularBuffer<string> lines = Console.Instance.Lines;
			for (int i = lines.Count() - Mathf.Min(linesVisible, lines.Count()) - historyScrollValue; i < lines.Count() - historyScrollValue; i++)
			{
				GUILayout.Label(lines.GetItemAt(i));
			}
			GUILayout.EndVertical();
			if (lines.Count() > linesVisible)
			{
				historyScrollValue = (int)GUILayout.VerticalScrollbar(historyScrollValue, linesVisible, lines.Count(), 0f, GUILayout.ExpandHeight(expand: true));
			}
			if (showHierarchy)
			{
				GUILayout.BeginVertical(GUILayout.Width(hierarchyWidth), GUILayout.ExpandHeight(expand: true));
				int num = command.IndexOf('.');
				if (num == -1 || command.IndexOf('.', num + 1) == -1)
				{
					hierarchyScrollValue = GUILayout.BeginScrollView(hierarchyScrollValue, "box");
					string[] array = displayObjects;
					foreach (string text in array)
					{
						if (GUILayout.Button(text, "GameObjectListLabel"))
						{
							if (command.LastIndexOf('/') >= 0)
							{
								command = command.Substring(0, command.LastIndexOf('/'));
							}
							command = command + "/" + text.Replace(" ", "\\ ") + "/";
							displayObjects = Console.Instance.GetGameobjectsAtPath(command);
							displayComponents = Console.Instance.GetComponentsOfGameobject(command);
							moveCursorToEnd = true;
						}
					}
					GUILayout.EndScrollView();
					componentScrollValue = GUILayout.BeginScrollView(componentScrollValue, "box");
					string[] array2 = displayComponents;
					foreach (string text2 in array2)
					{
						if (GUILayout.Button(text2, "GameObjectListLabel"))
						{
							if (num > 0)
							{
								command = command.Substring(0, num);
							}
							if (command.EndsWith("/"))
							{
								command = command.Substring(0, command.Length - 1);
							}
							command = command + "." + text2 + ".";
							displayObjects = Console.Instance.GetGameobjectsAtPath(command);
							displayComponents = Console.Instance.GetComponentsOfGameobject(command);
							displayMethods = Console.Instance.GetMethodsOfComponent(command);
							moveCursorToEnd = true;
						}
					}
					GUILayout.EndScrollView();
				}
				else
				{
					methodScrollValue = GUILayout.BeginScrollView(methodScrollValue, "box");
					string[] array3 = displayMethods;
					foreach (string text3 in array3)
					{
						if (GUILayout.Button(text3, "GameObjectListLabel"))
						{
							command = command.Substring(0, command.IndexOf('.', num + 1));
							command = command + "." + text3;
							moveCursorToEnd = true;
						}
					}
					GUILayout.EndScrollView();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("CommandTextField");
			string text4 = command;
			command = GUILayout.TextField(command);
			if (command != text4)
			{
				displayObjects = Console.Instance.GetGameobjectsAtPath(command);
				displayComponents = Console.Instance.GetComponentsOfGameobject(command);
				displayMethods = Console.Instance.GetMethodsOfComponent(command);
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null)
				{
					commandLastPos = textEditor.pos;
					commandLastSelectPos = textEditor.selectPos;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			if (Event.current.type == EventType.Repaint && moveCursorToEnd)
			{
				TextEditor textEditor2 = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor2 != null)
				{
					textEditor2.MoveTextEnd();
					textEditor2.pos = textEditor2.selectPos;
					textEditor2.graphicalCursorPos = textEditor2.style.GetCursorPixelPosition(new Rect(0f, 0f, textEditor2.position.width, textEditor2.position.height), textEditor2.content, textEditor2.pos);
					commandLastPos = textEditor2.pos;
					commandLastSelectPos = textEditor2.selectPos;
				}
				moveCursorToEnd = false;
			}
			if (GUI.GetNameOfFocusedControl() == "CommandTextField" && returnPressed)
			{
				Console.Instance.Print("> " + command);
				Console.Instance.Eval(command);
				command = string.Empty;
				commandIndex = 0;
				displayObjects = Console.Instance.GetGameobjectsAtPath(command);
				displayComponents = Console.Instance.GetComponentsOfGameobject(command);
			}
			if (GUI.GetNameOfFocusedControl() == "CommandTextField" && flag)
			{
				if (commandIndex == 0)
				{
					partialCommand = command;
				}
				commandIndex++;
				int num2 = Console.Instance.Commands.Count();
				if (num2 > 0)
				{
					if (commandIndex > num2)
					{
						commandIndex--;
					}
					command = Console.Instance.Commands.GetItemAt(num2 - 1 - (commandIndex - 1));
					moveCursorToEnd = true;
				}
			}
			if (GUI.GetNameOfFocusedControl() == "CommandTextField" && flag2)
			{
				commandIndex--;
				int num3 = Console.Instance.Commands.Count();
				if (commandIndex < 0)
				{
					commandIndex = 0;
				}
				if (num3 > 0)
				{
					if (commandIndex > 0)
					{
						command = Console.Instance.Commands.GetItemAt(num3 - 1 - (commandIndex - 1));
					}
					else
					{
						command = partialCommand;
					}
					moveCursorToEnd = true;
				}
			}
		}
		if (!isOpen && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.BackQuote)
		{
			isOpen = true;
			Event.current.Use();
			Event.current.type = EventType.Used;
			wasCursorVisible = Cursor.visible;
		}
		if (isOpen)
		{
			Cursor.visible = true;
		}
		if (isOpen && flag3)
		{
			isOpen = false;
			Cursor.visible = wasCursorVisible;
		}
		if (isOpen && Event.current.type == EventType.Layout && GUI.GetNameOfFocusedControl() != "CommandTextField")
		{
			GUI.FocusControl("CommandTextField");
			TextEditor textEditor3 = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
			if (textEditor3 != null)
			{
				textEditor3.pos = commandLastPos;
				textEditor3.selectPos = commandLastSelectPos;
			}
		}
	}

	private int test(int value)
	{
		return value + 1;
	}
}
