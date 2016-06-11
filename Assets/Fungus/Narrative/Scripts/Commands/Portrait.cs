using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	public struct PortraitState
	{
		public bool onScreen;
		public bool dimmed;
		public DisplayType display;
		public Sprite portrait;
		public RectTransform position;
		public FacingDirection facing;
		public Image portraitImage;
	}
	
	public enum DisplayType
	{
		None,
		Show,
		Hide,
		Replace,
		MoveToFront
	}

	public enum FacingDirection
	{
		None,
		Left,
		Right
	}
	
	public enum PositionOffset
	{
		None,
		OffsetLeft,
		OffsetRight
	}
	
	[CommandInfo("Narrative", 
				 "Portrait", 
				 "Controls a character portrait. ")]
	public class Portrait : ControlWithDisplay<DisplayType>
	{
		[Tooltip("Stage to display portrait on")]
		public Stage stage;
		
		[Tooltip("Character to display")]
		public Character character;
		
		[Tooltip("Character to swap with")]
		public Character replacedCharacter;
		
		[Tooltip("Portrait to display")]
		public Sprite portrait;
		
		[Tooltip("Move the portrait from/to this offset position")]
		public PositionOffset offset;
		
		[Tooltip("Move the portrait from this position")]
		public RectTransform fromPosition;

		[Tooltip("Move the portrait to this positoin")]
		public RectTransform toPosition;

		[Tooltip("Direction character is facing")]
		public FacingDirection facing;
		
		[Tooltip("Use Default Settings")]
		public bool useDefaultSettings = true;
		
		[Tooltip("Fade Duration")]
		public float fadeDuration = 0.5f;
		
		[Tooltip("Movement Duration")]
		public float moveDuration = 1f;
		
		[Tooltip("Shift Offset")]
		public Vector2 shiftOffset;
		
		[Tooltip("Move")]
		public bool move;
		
		[Tooltip("Start from offset")]
		public bool shiftIntoPlace;
		
		[Tooltip("Wait until the tween has finished before executing the next command")]
		public bool waitUntilFinished = false;

		// Abstracted controller logic for portraits
		private PortraitController portraitController;

		public override void OnEnter()
		{
			// If no display specified, do nothing
			if (IsDisplayNone(display))
			{
				Continue();
				return;
			}

			// If no character specified, do nothing
			if (character == null)
			{
				Continue();
				return;
			}

			// If Replace and no replaced character specified, do nothing
			if (display == DisplayType.Replace && replacedCharacter == null)
			{
				Continue();
				return;
			}

			// Selected "use default Portrait Stage"
			if (stage == null)
			{
				// If no default specified, try to get any portrait stage in the scene
				stage = FindObjectOfType<Stage>();
				// If portrait stage does not exist, do nothing
				if (stage == null)
				{
					Continue();
					return;
				}

				//Get the Stage's Portrait Controller
				portraitController = stage.GetComponent<PortraitController>();
			}

			// Early out if hiding a character that's already hidden
			if (display == DisplayType.Hide &&
				!character.state.onScreen)
			{
				Continue();
				return;
			}

			// Use default settings
			if (useDefaultSettings)
			{
				fadeDuration = stage.fadeDuration;
				moveDuration = stage.moveDuration;
				shiftOffset = stage.shiftOffset;
			}

			if (character.state.portraitImage == null)
			{
				portraitController.CreatePortraitObject(character, fadeDuration);
			}

			// if no previous portrait, use default portrait
			if (character.state.portrait == null) 
			{
				character.state.portrait = character.profileSprite;
			}

			// Selected "use previous portrait"
			if (portrait == null) 
			{
				portrait = character.state.portrait;
			}

			// if no previous position, use default position
			if (character.state.position == null)
			{
				character.state.position = stage.defaultPosition.rectTransform;
			}

			// Selected "use previous position"
			if (toPosition == null)
			{
				toPosition = character.state.position;
			}

			if (replacedCharacter != null)
			{
				// if no previous position, use default position
				if (replacedCharacter.state.position == null)
				{
					replacedCharacter.state.position = stage.defaultPosition.rectTransform;
				}
			}

			// If swapping, use replaced character's position
			if (display == DisplayType.Replace)
			{
				toPosition = replacedCharacter.state.position;
			}

			// Selected "use previous position"
			if (fromPosition == null)
			{
				fromPosition = character.state.position;
			}

			// if portrait not moving, use from position is same as to position
			if (!move)
			{
				fromPosition = toPosition;
			}

			if (display == DisplayType.Hide)
			{
				fromPosition = character.state.position;
			}

			// if no previous facing direction, use default facing direction
			if (character.state.facing == FacingDirection.None) 
			{
				character.state.facing = character.portraitsFace;
			}

			// Selected "use previous facing direction"
			if (facing == FacingDirection.None)
			{
				facing = character.state.facing;
			}

			switch(display)
			{
			case (DisplayType.Show):
				Show(character, fromPosition, toPosition);
				character.state.onScreen = true;
				if (!stage.charactersOnStage.Contains(character))
				{
					stage.charactersOnStage.Add(character);
				}
				break;

			case (DisplayType.Hide):
				Hide(character, fromPosition, toPosition);
				character.state.onScreen = false;
				stage.charactersOnStage.Remove(character);
				break;

			case (DisplayType.Replace):
				Show(character, fromPosition, toPosition);
				Hide(replacedCharacter, replacedCharacter.state.position, replacedCharacter.state.position);
				character.state.onScreen = true;
				replacedCharacter.state.onScreen = false;
				stage.charactersOnStage.Add(character);
				stage.charactersOnStage.Remove(replacedCharacter);
				break;

			case (DisplayType.MoveToFront):
				portraitController.MoveToFront(character);
				break;
			}
			
			if (display == DisplayType.Replace)
			{
				character.state.display = DisplayType.Show;
				replacedCharacter.state.display = DisplayType.Hide;
			}
			else
			{
				character.state.display = display;
			}

			character.state.portrait = portrait;
			character.state.facing = facing;
			character.state.position = toPosition;
			
			if (!waitUntilFinished)
			{
				Continue();
			}
			else
			{
				StartCoroutine(portraitController.WaitUntilFinished(fadeDuration, Continue));
			}
		}

		
//-------------------------------------------


		protected void Show(Character character, RectTransform fromPosition, RectTransform toPosition) 
		{
			if (shiftIntoPlace)
			{
				fromPosition = Instantiate(toPosition) as RectTransform;
				if (offset == PositionOffset.OffsetLeft)
				{
					fromPosition.anchoredPosition = new Vector2(fromPosition.anchoredPosition.x - Mathf.Abs(shiftOffset.x), fromPosition.anchoredPosition.y - Mathf.Abs(shiftOffset.y));
				}
				else if (offset == PositionOffset.OffsetRight)
				{
					fromPosition.anchoredPosition = new Vector2(fromPosition.anchoredPosition.x + Mathf.Abs(shiftOffset.x), fromPosition.anchoredPosition.y + Mathf.Abs(shiftOffset.y));
				}
				else
				{
					fromPosition.anchoredPosition = new Vector2(fromPosition.anchoredPosition.x, fromPosition.anchoredPosition.y);
				}
			}

			portraitController.SetupPortrait(character, fromPosition, facing);

			// LeanTween doesn't handle 0 duration properly
			float duration = (fadeDuration > 0f) ? fadeDuration : float.Epsilon;
			
			// Fade out a duplicate of the existing portrait image
			if (character.state.portraitImage != null)
			{
				GameObject tempGO = GameObject.Instantiate(character.state.portraitImage.gameObject);
				tempGO.transform.SetParent(character.state.portraitImage.transform, false);
				tempGO.transform.localPosition = Vector3.zero;
				tempGO.transform.localScale = character.state.position.localScale;

				Image tempImage = tempGO.GetComponent<Image>();
				tempImage.sprite = character.state.portraitImage.sprite;
				tempImage.preserveAspect = true;
				tempImage.color = character.state.portraitImage.color;

				LeanTween.alpha(tempImage.rectTransform, 0f, duration).setEase(stage.fadeEaseType).setOnComplete(() => {
					Destroy(tempGO);
				});
			}

			// Fade in the new sprite image
			character.state.portraitImage.sprite = portrait;
			character.state.portraitImage.color = new Color(1f, 1f, 1f, 0f);
			LeanTween.alpha(character.state.portraitImage.rectTransform, 1f, duration).setEase(stage.fadeEaseType);

			portraitController.DoMoveTween(character, fromPosition, toPosition, moveDuration, waitUntilFinished);
		}

		protected void Hide(Character character, RectTransform fromPosition, RectTransform toPosition)
		{
			if (character.state.display == DisplayType.None)
			{
				return;
			}

			portraitController.SetupPortrait(character, fromPosition, facing);

			// LeanTween doesn't handle 0 duration properly
			float duration = (fadeDuration > 0f) ? fadeDuration : float.Epsilon;
			
			LeanTween.alpha(character.state.portraitImage.rectTransform, 0f, duration).setEase(stage.fadeEaseType);

			portraitController.DoMoveTween(character, fromPosition, toPosition, moveDuration, waitUntilFinished);
		}



		public static void SetDimmed(Character character, Stage stage, bool dimmedState)
		{
			if (character.state.dimmed == dimmedState)
			{
				return;
			}

			character.state.dimmed = dimmedState;

			Color targetColor = dimmedState ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;

			// LeanTween doesn't handle 0 duration properly
			float duration = (stage.fadeDuration > 0f) ? stage.fadeDuration : float.Epsilon;
			
			LeanTween.color(character.state.portraitImage.rectTransform, targetColor, duration).setEase(stage.fadeEaseType);
		}

		public override string GetSummary()
		{
			if (display == DisplayType.None && character == null)
			{
				return "Error: No character or display selected";
			}
			else if (display == DisplayType.None)
			{
				return "Error: No display selected";
			}
			else if (character == null)
			{
				return "Error: No character selected";
			}

			string displaySummary = "";
			string characterSummary = "";
			string fromPositionSummary = "";
			string toPositionSummary = "";
			string stageSummary = "";
			string portraitSummary = "";
			string facingSummary = "";
			
			displaySummary = StringFormatter.SplitCamelCase(display.ToString());

			if (display == DisplayType.Replace)
			{
				if (replacedCharacter != null)
				{
					displaySummary += " \"" + replacedCharacter.name + "\" with";
				}
			}

			characterSummary = character.name;
			if (stage != null)
			{
				stageSummary = " on \"" + stage.name + "\"";
			}
			
			if (portrait != null)
			{
				portraitSummary = " " + portrait.name;
			}

			if (shiftIntoPlace)
			{
				if (offset != 0)
				{
					fromPositionSummary = offset.ToString();
					fromPositionSummary = " from " + "\"" + fromPositionSummary + "\"";
				}
			}
			else if (fromPosition != null)
			{
				fromPositionSummary = " from " + "\"" + fromPosition.name + "\"";
			}

			if (toPosition != null)
			{
				string toPositionPrefixSummary = "";
				if (move)
				{
					toPositionPrefixSummary = " to ";
				}
				else
				{
					toPositionPrefixSummary = " at ";
				}

				toPositionSummary = toPositionPrefixSummary + "\"" + toPosition.name + "\"";
			}

			if (facing != FacingDirection.None)
			{
				if (facing == FacingDirection.Left)
				{
					facingSummary = "<--";
				}
				if (facing == FacingDirection.Right)
				{
					facingSummary = "-->";
				}

				facingSummary = " facing \"" + facingSummary + "\"";
			}

			return displaySummary + " \"" + characterSummary + portraitSummary + "\"" + stageSummary + facingSummary + fromPositionSummary + toPositionSummary;
		}
		
		public override Color GetButtonColor()
		{
			return new Color32(230, 200, 250, 255);
		}
		
		public override void OnCommandAdded(Block parentBlock)
		{
			//Default to display type: show
			display = DisplayType.Show;
		}
	}
}