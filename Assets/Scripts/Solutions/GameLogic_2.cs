using UnityEngine;
using System.Collections;

/* Let's Get Ready to Move
 * This script represents what your GameLogic.cs script should look like starting from the Let's Get Ready to Move
 * lesson.
 * 
 * Do not use this script directly as the file and class names are incorrect.
 */
public class GameLogic_2 : MonoBehaviour
{
	public GameObject player;
	public GameObject startUI, restartUI;
	public GameObject startPoint, playPoint, restartPoint;

	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. GvrEditorEmulator, instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		player = player.transform.parent.gameObject;

		// Move player to the start position.
		player.transform.position = startPoint.transform.position;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && player.transform.position == playPoint.transform.position)
		{
			PuzzleSuccess();
		}
	}

	// Begin the puzzle sequence.
	public void StartPuzzle()
	{
		ToggleUI();
		iTween.MoveTo(player, 
			iTween.Hash(
				"position", playPoint.transform.position, 
				"time", 2, 
				"easetype", "linear"
			)
		);
	}

	// Reset the puzzle sequence.
	public void ResetPuzzle()
	{
		player.transform.position = startPoint.transform.position;
		ToggleUI();
	}

	// Do this when the player solves the puzzle.
	public void PuzzleSuccess()
	{
		iTween.MoveTo(player, 
			iTween.Hash(
				"position", restartPoint.transform.position, 
				"time", 2, 
				"easetype", "linear"
			)
		);
	}

	public void ToggleUI()
	{
		startUI.SetActive(!startUI.activeSelf);
		restartUI.SetActive(!restartUI.activeSelf);
	}

	// Placeholder method to prevent compiler errors caused by this method being called from LightUp.cs.
	public void PlayerSelection(GameObject sphere)
	{
		// Will be completed later in the course.
	}
}
