// https://github.com/marijnz/unity-autocomplete-search-field
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// namespace AutocompleteSearchField
// {
	[Serializable]
	public class AutocompleteSearchField
	{
		static class Styles
		{
			public const float resultHeight = 20f;
			public const float resultsBorderWidth = 2f;
			public const float resultsMargin = 15f;
			public const float resultsLabelOffset = 2f;

			public static readonly GUIStyle entryEven;
			public static readonly GUIStyle entryOdd;
			public static readonly GUIStyle labelStyle;
			public static readonly GUIStyle resultsBorderStyle;

			static Styles()
			{
				entryOdd = new GUIStyle("CN EntryBackOdd");
				entryEven = new GUIStyle("CN EntryBackEven");
				resultsBorderStyle = new GUIStyle("hostview");

				labelStyle = new GUIStyle(EditorStyles.label)
				{
					alignment = TextAnchor.MiddleLeft,
					richText = true
				};
			}
		}

		public Action<string> onInputChanged;
		public Action<string> onConfirm;
		public string searchString;
		public int maxResults = 15;
		public Rect searchRect = new Rect(0, 0, 140f, 20f);

		[SerializeField]
		List<string> results = new List<string>();

		[SerializeField]
		int selectedIndex = -1;

		SearchField searchField;

		Vector2 previousMousePosition;
		bool selectedIndexByMouse;

		bool showResults;

		public void AddResult(string result)
		{
			results.Add(result);
		}

		public void ClearResults()
		{
			results.Clear();
		}

		public void OnToolbarGUI(Rect rect)
		{
			Draw(asToolbar:true, rect);
		}

		public void OnGUI(Rect rect)
		{
			Draw(asToolbar:false, rect);
		}

		void Draw(bool asToolbar, Rect rect)
		{
			// var rect = GUILayoutUtility.GetRect(searchRect.x, searchRect.y, searchRect.width, searchRect.height, GUILayout.ExpandWidth(true));
			//GUILayout.BeginHorizontal();
			DoSearchField(rect, asToolbar);
			//GUILayout.EndHorizontal();
			rect.y += searchRect.height;
			DoResults(rect);
		}

		void DoSearchField(Rect rect, bool asToolbar)
		{
			if(searchField == null)
			{
				searchField = new SearchField();
				searchField.downOrUpArrowKeyPressed += OnDownOrUpArrowKeyPressed;
			}

			var result = asToolbar
				? searchField.OnToolbarGUI(rect, searchString)
				: searchField.OnGUI(rect, searchString);

			if (result != searchString && onInputChanged != null)
			{
				onInputChanged(result);
				selectedIndex = -1;
				showResults = true;
			}

			searchString = result;

			if(HasSearchbarFocused())
			{
				RepaintFocusedWindow();
			}
		}

		public void SetFocus()
		{
			if (searchField != null)
				searchField.SetFocus();
		}

		void OnDownOrUpArrowKeyPressed()
		{
			var current = Event.current;

			if (current.keyCode == KeyCode.UpArrow)
			{
				current.Use();
				selectedIndex--;
				selectedIndexByMouse = false;
			}
			else
			{
				current.Use();
				selectedIndex++;
				selectedIndexByMouse = false;
			}

			if (selectedIndex >= results.Count) selectedIndex = results.Count - 1;
			else if (selectedIndex < 0) selectedIndex = -1;
		}

		void DoResults(Rect rect)
		{
			if(results.Count <= 0 || !showResults) return;

			var current = Event.current;
			rect.height = Styles.resultHeight * Mathf.Min(maxResults, results.Count);
			rect.x += Styles.resultsMargin;
			rect.width -= Styles.resultsMargin * 2;

			var elementRect = rect;

			rect.height += Styles.resultsBorderWidth;
			GUI.Label(rect, "", Styles.resultsBorderStyle);

			var mouseIsInResultsRect = rect.Contains(current.mousePosition);

			if(mouseIsInResultsRect)
			{
				RepaintFocusedWindow();
			}

			var movedMouseInRect = previousMousePosition != current.mousePosition;

			elementRect.x += Styles.resultsBorderWidth;
			elementRect.width -= Styles.resultsBorderWidth * 2;
			elementRect.height = Styles.resultHeight;

			var didJustSelectIndex = false;

			for (var i = 0; i < results.Count && i < maxResults; i++)
			{
				if(current.type == EventType.Repaint)
				{
					var style = i % 2 == 0 ? Styles.entryOdd : Styles.entryEven;

					style.Draw(elementRect, false, false, i == selectedIndex, false);

					var labelRect = elementRect;
					labelRect.x += Styles.resultsLabelOffset;
					GUI.Label(labelRect, results[i], Styles.labelStyle);
				}
				if(elementRect.Contains(current.mousePosition))
				{
					if(movedMouseInRect)
					{
						selectedIndex = i;
						selectedIndexByMouse = true;
						didJustSelectIndex = true;
					}
					if(current.type == EventType.MouseDown)
					{
						OnConfirm(results[i]);
					}
				}
				elementRect.y += Styles.resultHeight;
			}

			if(current.type == EventType.Repaint && !didJustSelectIndex && !mouseIsInResultsRect && selectedIndexByMouse)
			{
				selectedIndex = -1;
			}

			if((GUIUtility.hotControl != searchField.searchFieldControlID && GUIUtility.hotControl > 0)
				|| (current.rawType == EventType.MouseDown && !mouseIsInResultsRect))
			{
				showResults = false;
			}

			// default select first
			if (current.type == EventType.KeyUp && current.keyCode == KeyCode.Return && selectedIndex == -1 && results.Count > 0)
			{
				selectedIndex = 0;
			}

			if(current.type == EventType.KeyUp && current.keyCode == KeyCode.Return && selectedIndex >= 0)
			{
				OnConfirm(results[selectedIndex]);
			}

			if(current.type == EventType.Repaint)
			{
				previousMousePosition = current.mousePosition;
			}
		}

		void OnConfirm(string result)
		{
			searchString = result;
			showResults = false;
			if(onConfirm != null) onConfirm(result);
			if(onInputChanged != null) onInputChanged(result);
			RepaintFocusedWindow();
			GUIUtility.keyboardControl = 0; // To avoid Unity sometimes not updating the search field text
		}

		bool HasSearchbarFocused()
		{
			return GUIUtility.keyboardControl == searchField.searchFieldControlID;
		}

		static void RepaintFocusedWindow()
		{
			if(EditorWindow.focusedWindow != null)
			{
				EditorWindow.focusedWindow.Repaint();
			}
		}
	}
// }