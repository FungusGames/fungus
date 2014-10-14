using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rotorz.ReorderableList;

namespace Fungus
{

	[CustomEditor (typeof(Sequence))]
	public class SequenceEditor : Editor 
	{
		protected class AddCommandOperation
		{
			public Sequence sequence;
			public Type commandType;
			public int index;
		}

		protected SerializedProperty sequenceNameProp;

		protected bool showContextMenu;

		public virtual void OnEnable()
		{
			sequenceNameProp = serializedObject.FindProperty("sequenceName");
		}

		public virtual void DrawInspectorGUI(FungusScript fungusScript)
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(sequenceNameProp);
			EditorGUILayout.Separator();

			serializedObject.ApplyModifiedProperties();
		}

		public virtual void DrawCommandListGUI(FungusScript fungusScript)
		{
			serializedObject.Update();

			Sequence sequence = target as Sequence;
			UpdateIndentLevels(sequence);

			ReorderableListGUI.Title(sequence.sequenceName);
			SerializedProperty commandListProperty = serializedObject.FindProperty("commandList");
			CommandListAdaptor adaptor = new CommandListAdaptor(commandListProperty, 0);
			adaptor.nodeRect = sequence.nodeRect;

			ReorderableListFlags flags = ReorderableListFlags.HideAddButton | ReorderableListFlags.HideRemoveButtons;
			ReorderableListControl.DrawControlFromState(adaptor, null, flags);

			Rect bottomBoxRect = GUILayoutUtility.GetRect(sequence.nodeRect.width, 20);
			bottomBoxRect.y -= 7;
			bottomBoxRect.x += 5;
			bottomBoxRect.width -= 10;
			if (sequence.commandList.Count == 0)
			{
				bottomBoxRect.y -= 16;
			}

			GUI.backgroundColor = new Color32(200, 200, 200, 255);
			GUI.Box(bottomBoxRect, "");

			if (!Application.isPlaying &&
			    sequence == fungusScript.selectedSequence)
			{
				// Show add command button
				{
					Rect plusRect = bottomBoxRect;
					plusRect.x = plusRect.width - 19;
					plusRect.y += 2;
					plusRect.width = 16;
					plusRect.height = 16;

					if (GUI.Button(plusRect, "", new GUIStyle("OL Plus")))
					{
						ShowCommandMenu();
					}
				}

				// Show delete sequence button
				{
					Rect deleteRect = new Rect();
					deleteRect.x = sequence.nodeRect.width - 28;
					deleteRect.y = 7;
					deleteRect.width = 16;
					deleteRect.height = 16;
					
					if (GUI.Button(deleteRect, "", new GUIStyle("WinBtnCloseWin")))
					{
						DeleteSequence();
					}
				}
			}

			if (!Application.isPlaying)
			{
				if (Event.current.button == 1 && 
				    Event.current.type == EventType.MouseDown)
				{
					showContextMenu = !showContextMenu;
					Event.current.Use();
				}
			}

			if (showContextMenu)
			{
				ShowContextMenu();
			}

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void UpdateIndentLevels(Sequence sequence)
		{
			int indentLevel = 0;
			foreach(Command command in sequence.commandList)
			{
				indentLevel += command.GetPreIndent();
				command.indentLevel = Math.Max(indentLevel, 0);
				indentLevel += command.GetPostIndent();
			}
		}

		static public void SequenceField(SerializedProperty property, GUIContent label, GUIContent nullLabel, FungusScript fungusScript)
		{
			if (fungusScript == null)
			{
				return;
			}

			Sequence sequence = property.objectReferenceValue as Sequence;
		
			// Build dictionary of child sequences
			List<GUIContent> sequenceNames = new List<GUIContent>();
			
			int selectedIndex = 0;
			sequenceNames.Add(nullLabel);
			Sequence[] sequences = fungusScript.GetComponentsInChildren<Sequence>(true);
			for (int i = 0; i < sequences.Length; ++i)
			{
				sequenceNames.Add(new GUIContent(sequences[i].sequenceName));
				
				if (sequence == sequences[i])
				{
					selectedIndex = i + 1;
				}
			}
			
			selectedIndex = EditorGUILayout.Popup(label, selectedIndex, sequenceNames.ToArray());
			if (selectedIndex == 0)
			{
				sequence = null; // Option 'None'
			}
			else
			{
				sequence = sequences[selectedIndex - 1];
			}
			
			property.objectReferenceValue = sequence;
		}

		static public Sequence SequenceField(Rect position, GUIContent nullLabel, FungusScript fungusScript, Sequence sequence)
		{
			if (fungusScript == null)
			{
				return null;
			}
			
			Sequence result = sequence;
			
			// Build dictionary of child sequences
			List<GUIContent> sequenceNames = new List<GUIContent>();
			
			int selectedIndex = 0;
			sequenceNames.Add(nullLabel);
			Sequence[] sequences = fungusScript.GetComponentsInChildren<Sequence>();
			for (int i = 0; i < sequences.Length; ++i)
			{
				sequenceNames.Add(new GUIContent(sequences[i].name));
				
				if (sequence == sequences[i])
				{
					selectedIndex = i + 1;
				}
			}
			
			selectedIndex = EditorGUI.Popup(position, selectedIndex, sequenceNames.ToArray());
			if (selectedIndex == 0)
			{
				result = null; // Option 'None'
			}
			else
			{
				result = sequences[selectedIndex - 1];
			}
			
			return result;
		}

		protected void ShowContextMenu()
		{
			bool showCut = false;
			bool showCopy = false;
			bool showDelete = false;
			bool showPaste = false;

			Sequence sequence = target as Sequence;
			if (sequence.GetFungusScript().selectedCommands.Count > 0)
			{
				showCut = true;
				showCopy = true;
				showDelete = true;
			}

			CommandCopyBuffer commandCopyBuffer = CommandCopyBuffer.GetInstance();

			if (commandCopyBuffer.HasCommands())
			{
				showPaste = true;
			}

			GenericMenu commandMenu = new GenericMenu();

			if (showCut)
			{
				commandMenu.AddItem (new GUIContent ("Cut"), false, Cut);
			}
			else
			{
				commandMenu.AddDisabledItem(new GUIContent ("Cut"));
			}

			if (showCopy)
			{
				commandMenu.AddItem (new GUIContent ("Copy"), false, Copy);
			}
			else
			{
				commandMenu.AddDisabledItem(new GUIContent ("Copy"));
			}

			if (showPaste)
			{
				commandMenu.AddItem (new GUIContent ("Paste"), false, Paste);
			}
			else
			{
				commandMenu.AddDisabledItem(new GUIContent ("Paste"));
			}

			if (showDelete)
			{
				commandMenu.AddItem (new GUIContent ("Delete"), false, Delete);
			}
			else
			{
				commandMenu.AddDisabledItem(new GUIContent ("Delete"));
			}

			commandMenu.AddSeparator("");

			commandMenu.AddItem (new GUIContent ("Select All"), false, SelectAll);
			commandMenu.AddItem (new GUIContent ("Select None"), false, SelectNone);

			commandMenu.AddSeparator("");

			commandMenu.AddItem (new GUIContent ("Delete Sequence"), false, DeleteSequence);
			commandMenu.AddItem (new GUIContent ("Duplicate Sequence"), false, DuplicateSequence);

			commandMenu.ShowAsContext();

			Repaint();
		}

		protected virtual void SelectAll()
		{
			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();

			fungusScript.ClearSelectedCommands();
			Undo.RecordObject(fungusScript, "Select All");
			foreach (Command command in sequence.commandList)
			{
				fungusScript.selectedCommands.Add(command);
			}
		}

		protected virtual void SelectNone()
		{
			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();
			if (fungusScript != null)
			{
				Undo.RecordObject(fungusScript, "Select None");
				fungusScript.ClearSelectedCommands();
			}
		}

		protected virtual void Cut()
		{
			Copy();
			Delete();
		}

		protected virtual void Copy()
		{
			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();

			CommandCopyBuffer commandCopyBuffer = CommandCopyBuffer.GetInstance();
			commandCopyBuffer.Clear();

			foreach (Command command in fungusScript.selectedCommands)
			{
				System.Type type = command.GetType();
				Command newCommand = Undo.AddComponent(commandCopyBuffer.gameObject, type) as Command;
				System.Reflection.FieldInfo[] fields = type.GetFields();
				foreach (System.Reflection.FieldInfo field in fields)
				{
					field.SetValue(newCommand, field.GetValue(command));
				}
			}
		}

		protected virtual void Paste()
		{
			CommandCopyBuffer commandCopyBuffer = CommandCopyBuffer.GetInstance();

			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();

			// Find where to paste commands in sequence (either at end or after last selected command)
			int pasteIndex = sequence.commandList.Count;
			if (fungusScript.selectedCommands.Count > 0)
			{
				for (int i = 0; i < sequence.commandList.Count; ++i)
				{
					Command command = sequence.commandList[i];

					foreach (Command selectedCommand in fungusScript.selectedCommands)
					{
						if (command == selectedCommand)
						{
							pasteIndex = i + 1;
						}
					}
				}
			}

			foreach (Command command in commandCopyBuffer.GetCommands())
			{
				System.Type type = command.GetType();
				Command newCommand = Undo.AddComponent(sequence.gameObject, type) as Command;
				System.Reflection.FieldInfo[] fields = type.GetFields();
				foreach (System.Reflection.FieldInfo field in fields)
				{
					field.SetValue(newCommand, field.GetValue(command));
				}

				Undo.RecordObject(sequence, "Paste");
				sequence.commandList.Insert(pasteIndex++, newCommand);
			}
		}

		protected virtual void Delete()
		{
			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();

			for (int i = sequence.commandList.Count - 1; i >= 0; --i)
			{
				Command command = sequence.commandList[i];
				foreach (Command selectedCommand in fungusScript.selectedCommands)
				{
					if (command == selectedCommand)
					{
						Undo.RecordObject(sequence, "Delete");
						sequence.commandList.RemoveAt(i);
						Undo.DestroyObjectImmediate(command);

						break;
					}
				}
			}

			Undo.RecordObject(fungusScript, "Delete");
			fungusScript.ClearSelectedCommands();
			fungusScript.selectedSequence = null;
		}

		protected virtual void DeleteSequence()
		{
			Sequence sequence = target as Sequence;
			FungusScriptWindow.deleteList.Add(sequence);
		}

		protected virtual void DuplicateSequence()
		{
			Sequence sequence = target as Sequence;
			FungusScript fungusScript = sequence.GetFungusScript();

			Vector2 newPosition = new Vector2(sequence.nodeRect.position.x + sequence.nodeRect.width + 20, sequence.nodeRect.y);
			Sequence newSequence = FungusScriptWindow.CreateSequence(fungusScript, newPosition);
			newSequence.sequenceName = sequence.sequenceName + " (Copy)";
			
			foreach (Command command in sequence.commandList)
			{
				System.Type type = command.GetType();
				Command newCommand = Undo.AddComponent(fungusScript.gameObject, type) as Command;
				System.Reflection.FieldInfo[] fields = type.GetFields();
				foreach (System.Reflection.FieldInfo field in fields)
				{
					field.SetValue(newCommand, field.GetValue(command));
				}
				newSequence.commandList.Add(newCommand);
			}
		}

		void ShowCommandMenu()
		{
			Sequence sequence = target as Sequence;
			int index = sequence.commandList.Count;

			GenericMenu commandMenu = new GenericMenu();
			
			// Build menu list
			List<System.Type> menuTypes = EditorExtensions.FindDerivedTypes(typeof(Command)).ToList();
			foreach(System.Type type in menuTypes)
			{
				object[] attributes = type.GetCustomAttributes(false);
				foreach (object obj in attributes)
				{
					CommandInfoAttribute infoAttr = obj as CommandInfoAttribute;
					if (infoAttr != null)
					{
						AddCommandOperation commandOperation = new AddCommandOperation();
						
						commandOperation.sequence = sequence;
						commandOperation.commandType = type;
						commandOperation.index = index;
						
						commandMenu.AddItem (new GUIContent (infoAttr.Category + "/" + infoAttr.CommandName), 
						                     false, AddCommandCallback, commandOperation);
					}
				}
			}
			
			commandMenu.ShowAsContext();
		}
		
		void AddCommandCallback(object obj)
		{
			AddCommandOperation commandOperation = obj as AddCommandOperation;
			
			Sequence sequence = commandOperation.sequence;
			if (sequence == null)
			{
				return;
			}
			
			sequence.GetFungusScript().ClearSelectedCommands();
			
			Command newCommand = Undo.AddComponent(sequence.gameObject, commandOperation.commandType) as Command;
			sequence.GetFungusScript().selectedCommands.Add(newCommand);

			Undo.RecordObject(sequence, "Set command type");
			if (commandOperation.index < sequence.commandList.Count - 1)
			{
				sequence.commandList[commandOperation.index] = newCommand;
			}
			else
			{
				sequence.commandList.Add(newCommand);
			}
		}

	}

}