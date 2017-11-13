using UnityEngine;
using System.Collections;

/* Starter Project
 * This script represents what your GameLogic.cs script should look like at the beginning of the course.
 *
 * Do not use this script directly as the file and class names are incorrect.
 */
public class GameLogic_1 : MonoBehaviour
{
	public GameObject startUI, restartUI;

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
